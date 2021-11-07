using Microsoft.Extensions.Options;
using MovieManager.Core.Entities;
using MovieManager.Core.Enumerations;
using MovieManager.Core.Interfaces;
using MovieManager.Core.Settings;
using QBittorrent.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MovieManager.Infrastructure.Services
{
	public class QbittorrentService : IQbittorrentService
	{
		private readonly QbittorrentSettings _qbittorrentSetting;
		private readonly IAppLogger<QbittorrentService> _logger;
		private readonly QBittorrentClient _client;

		public QbittorrentService(IAppLogger<QbittorrentService> logger, IOptionsSnapshot<QbittorrentSettings> qbittorrentSetting)
		{
			_logger = logger;
			_qbittorrentSetting = qbittorrentSetting.Value;
			_client = new QBittorrentClient(new Uri(_qbittorrentSetting.WebUrl));
		}

		public async Task EnsureLoggedInAsync()
		{
			try
			{
				await _client.GetApiVersionAsync();
			}
			catch(QBittorrentClientRequestException e)
			{
				if(e.StatusCode == HttpStatusCode.Forbidden)
				{
					_logger.LogWarning("Qbittorrent logged out, logging in Now");
					await _client.LoginAsync(_qbittorrentSetting.Username, _qbittorrentSetting.Password);
				}
			}
		}

		public TorrentInfo GetTorrentInfo(string hash)
		{
			EnsureLoggedInAsync().Wait();
			var torrentQuery = new TorrentListQuery() { Hashes = new string[] { hash } };
			return _client.GetTorrentListAsync(torrentQuery).Result.FirstOrDefault();
		}

		public async Task DeleteTorrentAsync(string hash, bool deleteDownloaded = false)
		{
			_logger?.LogInformation("Delete Torrent start: {hash}", hash);
			await EnsureLoggedInAsync();

			if(GetTorrentInfo(hash) != null)
			{
				await _client.DeleteAsync(hash, deleteDownloaded);
			}
			_logger?.LogInformation("Delete Torrent End: {hash}", hash);
		}

		public async Task AddTorrentAsync(MovieMagnet magnet)
		{
			_logger?.LogInformation("Add Torrent for movie {movieNumber}: {hash}", magnet.MovieNumber, magnet.Hash);
			await EnsureLoggedInAsync();
			AddTorrentUrlsRequest request = new AddTorrentUrlsRequest(new Uri(magnet.MagnetUrl));
			await _client.AddTorrentsAsync(request);
			string savePath = Path.Combine(_qbittorrentSetting.DownloadRootPath, magnet.MovieNumber.ToUpper() + "_" + magnet.IdMovieMag);
			await _client.SetLocationAsync(magnet.Hash, savePath);
			await _client.SetTorrentCategoryAsync(magnet.Hash, _qbittorrentSetting.Category);

			magnet.IdStatus = MagnetStatus.Downloading;
			magnet.SavePath = savePath;
			magnet.DtStart = DateTime.Now;
			_logger?.LogInformation("Add Torrent for movie {movieNumber} ended", magnet.MovieNumber);
		}
			
		public async Task GetTorrentContent()
		{
			string hash = "6C4182E5495D49382A529ABFB1C7A5A766416D68";
			await EnsureLoggedInAsync();
			var torrents = await _client.GetTorrentContentsAsync(hash);
			var torrentsdInfo = await _client.GetTorrentListAsync();
			foreach(TorrentContent content in torrents)
			{
				Console.WriteLine(content.ToString());
			}

			//await _client.SetFilePriorityAsync(hash, 0, TorrentContentPriority.Skip);

			await _client.RenameFileAsync(hash, 10, "mytest.jpg");
		}

		public int GetTorrentCanAddCount()
		{
			EnsureLoggedInAsync().Wait();
			var query = new TorrentListQuery() { Category = _qbittorrentSetting.Category };
			return _qbittorrentSetting.MaxDownloadCount - _client.GetTorrentListAsync(query).Result.Count;
		}
	}
}
