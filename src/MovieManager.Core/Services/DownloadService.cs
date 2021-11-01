using Microsoft.Extensions.Options;
using MovieManager.Core.Interfaces;
using MovieManager.Core.Settings;
using MovieManager.Core.Enumerations;
using MovieManager.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using MovieManager.Core.Extensions;
using QBittorrent.Client;
using System.Globalization;

namespace MovieManager.Core.Services
{
	public class DownloadService : IDownloadService
	{
		private readonly IAppLogger<DownloadService> _logger;
		private readonly IMovieService _movieService;
		private readonly IMovieMagnetService _movieMagnetService;
		//private readonly MagnetSettings _magnetSettings;
		private readonly IQbittorrentService _qbittorrentService;
		private readonly ILocalFileService _localFileService;

		public DownloadService(IAppLogger<DownloadService> logger, 
			IMovieService movieService, IMovieMagnetService movieMagnetService, IQbittorrentService qbittorrentService, ILocalFileService localFileService)
		{
			_logger = logger;
			//_magnetSettings = magnetSettings.Value;
			_movieService = movieService;
			_movieMagnetService = movieMagnetService;
			_qbittorrentService = qbittorrentService;
			_localFileService = localFileService;
		}

		public void MonitorMovieDownload()
		{
			try
			{
				_logger.LogInformation("************** Start Monitoring Movie Download Job - {Date} **************", DateTime.Now.ToString("u", DateTimeFormatInfo.InvariantInfo));
				//Check every magnet in status downloading. If not found in qbittorrent or status = downloaded, do fileService
				int nbMovieAdded = 0;
				List<Task> lstGenerateMovieFileTask = new List<Task>();
				List<Task> lstDeleteTorrentTask = new List<Task>();
				List<Task> lstAddTorrentTask = new List<Task>();

				foreach(MovieMagnet magnet in _movieMagnetService.FindMovieMagnetByStatus(MagnetStatus.Downloading))
				{
					TorrentInfo torrentInfo = _qbittorrentService.GetTorrentInfo(magnet.Hash);
					if(torrentInfo == null || torrentInfo.State == TorrentState.Uploading || torrentInfo.Progress >= 1)
					{
						if(_localFileService.CheckMovieDownloaded(magnet.SavePath, magnet.MovieNumber))
						{
							_logger?.LogInformation("MovieMagnet {magnetId} for movie {movieNumber} has finished downloading", magnet.IdMovieMag, magnet.MovieNumber);
							lstGenerateMovieFileTask.Add(TreatDownloadedMovie(magnet));
						}
						else
						{
							_logger?.LogError("Cannot find movie {movieNumber} in path {moviePath}. Please check manually", magnet.MovieNumber, magnet.SavePath);
							magnet.IdStatus = MagnetStatus.InError;
						}
					}
					else if(TorrentIsNotActive(magnet, torrentInfo))
					{
						_logger?.LogWarning("MovieMagnet {magnetId} for movie {movieNumber} is no longer active, Mark it as dead", magnet.IdMovieMag, magnet.MovieNumber);
						lstDeleteTorrentTask.Add(_qbittorrentService.DeleteTorrentAsync(magnet.Hash, true));
						magnet.IdStatus = MagnetStatus.Dead;
					}

					_movieMagnetService.SaveMovieMagnet(magnet);
				}

				Task.WaitAll(lstDeleteTorrentTask.ToArray());
				Task.WaitAll(lstGenerateMovieFileTask.ToArray());

				//Send torrent to qbittorrent for the top nbFinished
				int nbMovieAddMax = _qbittorrentService.GetTorrentCanAddCount();
				foreach(Movie movie in _movieService.LoadMoviesToDownload())
				{
					if(nbMovieAdded >= nbMovieAddMax)
						break;

					MovieMagnet magnet = _movieMagnetService.FindBestMatchMagnetByMovie(movie);
					if(magnet != null)
					{
						//TODO: Here we can add antoher async function to get the torrentContent and set other files priority to notDownload
						lstAddTorrentTask.Add(_qbittorrentService.AddTorrentAsync(magnet)
												.ContinueWith(t => _movieMagnetService.SaveMovieMagnet(magnet)));
						nbMovieAdded++;
					}
				}

				Task.WaitAll(lstAddTorrentTask.ToArray());
			}
			catch(Exception ex)
			{
				_logger.LogError(ex, "Some errors occurred in MonitorMovieDownload");
			}
			finally
			{
				_logger.LogInformation("************** Monitoring Movie Download Job End - {Date} **************", DateTime.Now.ToString("u", DateTimeFormatInfo.InvariantInfo));
			}
		}

		public async Task TreatDownloadedMovie(MovieMagnet magnet)
		{
			try
			{
				await _localFileService.GenerateMovieFile(magnet);

				List<MovieMagnet> movieMagnets = new List<MovieMagnet>();

				if(magnet.HasSub)
				{
					_logger.LogInformation("Movie {movieNumber} has downloaded sub version. Checking non-sub folder and delete it.", magnet.MovieNumber);
					movieMagnets = _movieMagnetService.FindMovieMagnetByStatus(MagnetStatus.Finished, magnet.IdMovie);
					foreach(MovieMagnet movieMagnet in movieMagnets)
					{
						await _localFileService.DeleteFolder(movieMagnet.SavePath);
						movieMagnet.IdStatus = MagnetStatus.Archived;
					}
				}
				movieMagnets.Add(magnet);
				_movieMagnetService.SaveMovieMagnetList(movieMagnets);

				await _qbittorrentService.DeleteTorrentAsync(magnet.Hash, false);

				Movie movie = _movieService.FindMovieById(magnet.IdMovie);
				_movieService.UpdateStatus(movie, magnet.HasSub ? MovieStatus.Finished : MovieStatus.Downloaded);
				_movieService.SaveMovie(movie);
			}
			catch(Exception ex)
			{
				_logger.LogError(ex, "Error occurred when generating the downloaded movie file - {movieNumber}", magnet.MovieNumber);
				magnet.IdStatus = MagnetStatus.InError;
				_movieMagnetService.SaveMovieMagnet(magnet);
			}
		}

		public bool TorrentIsNotActive(MovieMagnet magnet, TorrentInfo torrent)
		{
			return (DateTime.Now - magnet.DtStart.Value).Days > 2 && torrent.Progress < 0.1 && torrent.EstimatedTime.Value.Days > 100;
		}	

    }
}
