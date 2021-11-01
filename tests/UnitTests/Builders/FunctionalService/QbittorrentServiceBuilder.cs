using Microsoft.Extensions.Logging;
using MovieManager.Infrastructure.Logging;
using MovieManager.Infrastructure.Repositories;
using MovieManager.Infrastructure.DbContext;
using MovieManager.Core.Services;
using MovieManager.Core.Interfaces;
using MovieManager.Infrastructure.Services;
using Microsoft.Extensions.Options;
using MovieManager.Core.Settings;

namespace UnitTests.Builders
{
	public class QbittorrentServiceBuilder
	{
		private readonly IAppLogger<QbittorrentService> _logger;
		private readonly IOptionsSnapshot<QbittorrentSettings> _qbittorrentSettings;

		public QbittorrentServiceBuilder()
		{
			_logger = new LoggerBuilder<QbittorrentService>().Build();
			_qbittorrentSettings = new qbittorrentSettingBuilder().Build();
		}

		public QbittorrentService Build()
		{
			return new QbittorrentService(_logger, _qbittorrentSettings);
		}
	}
}
