using System;
using Xunit;
using Moq;
using UnitTests.Builders;
using MovieManager.Core.Interfaces;
using MovieManager.Core.Services;
using MovieManager.Core.Entities;
using MovieManager.Infrastructure.DbContext;
using MovieManager.Infrastructure.Repositories;
using Xunit.Abstractions;
using System.Collections.Generic;
using System.Linq;
using MovieManager.Core.Enumerations;
using MovieManager.Infrastructure.Services;
using MovieManager.Core.Settings;
using Microsoft.Extensions.Options;
using MovieManager.Infrastructure.Logging;

namespace UnitTests
{
	public class QbittorrentServiceTests
	{
		private readonly ITestOutputHelper _output;
		private readonly LoggerAdapter<QbittorrentService> _logger;
		private QbittorrentService _qbittorrentService;
		private readonly IOptions<QbittorrentSettings> _qbittorrentSettings;

		public QbittorrentServiceTests(ITestOutputHelper output)
		{
			_output = output;
			_logger = new LoggerBuilder<QbittorrentService>().Build();
			_qbittorrentSettings = new qbittorrentSettingBuilder().Build();
			_qbittorrentService = new QbittorrentService(_logger, _qbittorrentSettings);
		}

		[Fact]
		public void test()
		{
			_output.WriteLine("enter");
			_qbittorrentService.EnsureLoggedIn().Wait();
			_output.WriteLine("exit");
		}
	}
}
