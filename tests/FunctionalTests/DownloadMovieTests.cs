using MovieManager.Core.Services;
using MovieManager.Infrastructure.Logging;
using MovieManager.Infrastructure.Services;
using System;
using Xunit;
using Xunit.Abstractions;
using UnitTests.Builders;

namespace FunctionalTests
{
	public class DownloadMovieTests
	{

		private readonly ITestOutputHelper _output;
		private DownloadService _downloadService;
		private readonly LoggerAdapter<DownloadService> _logger;
		private readonly MovieService _movieService;
		private readonly MovieMagnetService _movieMagnetService;
		private readonly QbittorrentService _qbittorrentService;
		private readonly LocalFileService _localFileService;

		public DownloadMovieTests(ITestOutputHelper output)
		{
			_output = output;
			_logger = new LoggerBuilder<DownloadService>().Build();
			_movieService = new MovieServiceBuilder().Build();
			_movieMagnetService = new MovieMagServiceBuilder().Build();
			_qbittorrentService = new QbittorrentServiceBuilder().Build();
			_localFileService = new LocalFileServiceBuilder().Build();
			_downloadService = new DownloadService(_logger, _movieService, _movieMagnetService, _qbittorrentService, _localFileService);
		}

		[Fact]
		public void TestMonitorMovieDownload()
		{
			_downloadService.MonitorMovieDownload();
		}

	}
}
