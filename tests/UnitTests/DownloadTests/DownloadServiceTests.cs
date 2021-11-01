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
using MovieManager.Infrastructure.Logging;
using MovieManager.Infrastructure.Services;

namespace UnitTests
{
	public class DownloadServiceTests
	{
		private readonly ITestOutputHelper _output;
		private DownloadService _downloadService;
		private readonly LoggerAdapter<DownloadService> _logger;
		private readonly MovieService _movieService;
		private readonly MovieMagnetService _movieMagnetService;
		private readonly QbittorrentService _qbittorrentService;

		public DownloadServiceTests(ITestOutputHelper output)
		{
			_output = output;
			_logger = new LoggerBuilder<DownloadService>().Build();
			_movieService = new MovieServiceBuilder().Build();
			_movieMagnetService = new MovieMagServiceBuilder().Build();
			_qbittorrentService = new QbittorrentServiceBuilder().Build();
			_downloadService = new DownloadService(_logger, _movieService, _movieMagnetService, _qbittorrentService, null);
		}

		[Fact]
		public void testMonitorMovieDownload()
		{
			_downloadService.MonitorMovieDownload();
		}
	}
}
