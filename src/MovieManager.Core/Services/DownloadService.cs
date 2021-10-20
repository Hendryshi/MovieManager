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

namespace MovieManager.Core.Services
{
	public class DownloadService
	{
		private readonly IAppLogger<MagnetScrapeService> _logger;
		private readonly IMovieService _movieService;
		private readonly IMovieMagnetService _movieMagnetService;
		private readonly MagnetSettings _magnetSettings;
		private readonly IQbittorrentService _qbittorrentService;

		public DownloadService() { }

		public DownloadService(IAppLogger<MagnetScrapeService> logger, IOptions<MagnetSettings> magnetSettings,
			IMovieService movieService, IMovieMagnetService movieMagnetService, IQbittorrentService qbittorrentService)
		{
			_logger = logger;
			_magnetSettings = magnetSettings.Value;
			_movieService = movieService;
			_movieMagnetService = movieMagnetService;
			_qbittorrentService = qbittorrentService;
		}

		public void MonitorMovieDownload()
		{
			
		}

		public void TestAsync()
		{
			var client = new QBittorrentClient(new Uri("http://127.0.0.1:6882/"));
			client.LoginAsync("admin", "adminadmin").Wait();

			var hash = "56577925B3A600F93F198CB9684F43177FFE74C5";
			var request = new AddTorrentUrlsRequest(new Uri("magnet:?xt=urn:btih:56577925B3A600F93F198CB9684F43177FFE74C5&dn=MIDE-979-C"));

			//await client.AddTorrentsAsync(request);

			//await client.SetTorrentCategoryAsync(hash, "jav");

			//await client.ResumeAsync(hash);

			var torrent = client.GetTorrentListAsync().Result;

			var torrent1 = torrent.First();

			var pieces = client.GetTorrentContentsAsync(hash).Result;

			

			foreach(TorrentContent content in pieces)
			{
				content.Priority = TorrentContentPriority.Skip;
			}

			client.Dispose();
		}

    }
}
