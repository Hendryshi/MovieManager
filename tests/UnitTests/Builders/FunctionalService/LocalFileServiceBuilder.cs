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
	public class LocalFileServiceBuilder
	{
		private readonly IAppLogger<LocalFileService> _logger;
		private readonly IOptionsSnapshot<LocalFileSettings> _localFileSettings;

		public LocalFileServiceBuilder()
		{
			_logger = new LoggerBuilder<LocalFileService>().Build();
			_localFileSettings = new localFileSettingBuilder().Build();
		}

		public LocalFileService Build()
		{
			return new LocalFileService(_logger, _localFileSettings);
		}
	}
}
