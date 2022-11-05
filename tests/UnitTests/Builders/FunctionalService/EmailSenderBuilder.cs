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
	public class EmailSenderBuilder
	{
		private readonly LoggerAdapter<EmailSender> _logger;
		private readonly IOptionsSnapshot<EmailSettings> _emailSettings;

		public EmailSenderBuilder()
		{
			_logger = new LoggerBuilder<EmailSender>().Build();
			_emailSettings = new EmailSettingBuilder().Build();
		}

		public EmailSender Build()
		{
			return new EmailSender(_logger, _emailSettings);
		}
	}
}
