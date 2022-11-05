using MovieManager.Core.Services;
using MovieManager.Infrastructure.Logging;
using MovieManager.Infrastructure.Services;
using System;
using Xunit;
using Xunit.Abstractions;
using UnitTests.Builders;
using Microsoft.Extensions.Options;
using MovieManager.Core.Settings;

namespace FunctionalTests
{
	public class EmailSenderTests
	{

		private readonly ITestOutputHelper _output;
		private EmailSender _emailSender;
		private readonly LoggerAdapter<EmailSender> _logger;
		private readonly IOptionsSnapshot<EmailSettings> _emailSettings;

		public EmailSenderTests(ITestOutputHelper output)
		{
			_output = output;
			_logger = new LoggerBuilder<EmailSender>().Build();
			_emailSettings = new EmailSettingBuilder().Build();
			_emailSender = new EmailSender(_logger, _emailSettings);
		}

		[Fact]
		public void Test_Send_Email()
		{
			string to = "yejia.shi@hotmail.com";
			string subject = "Email Test Subject";
			string body = "This is a body";
			_emailSender.SendEmail(null, to, subject, body);
		}

	}
}
