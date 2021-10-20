using Microsoft.Extensions.Options;
using MovieManager.Core.Interfaces;
using MovieManager.Core.Settings;
using QBittorrent.Client;
using System;
using System.Collections.Generic;
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

		public QbittorrentService(IAppLogger<QbittorrentService> logger, IOptions<QbittorrentSettings> qbittorrentSetting)
		{
			_logger = logger;
			_qbittorrentSetting = qbittorrentSetting.Value;
			_client = new QBittorrentClient(new Uri(_qbittorrentSetting.WebUrl));
		}

		public async Task EnsureLoggedIn()
		{
			try
			{
				await _client.GetApiVersionAsync();
			}
			catch(QBittorrentClientRequestException e)
			{
				if(e.StatusCode == HttpStatusCode.Forbidden)
				{
					_logger.LogWarning("Qbittorrent logged out, logging in again");
					await _client.LoginAsync(_qbittorrentSetting.Username, _qbittorrentSetting.Password);
				}
			}
		}

		
	}
}
