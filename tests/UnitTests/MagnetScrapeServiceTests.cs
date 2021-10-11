using System;
using Xunit;
using Moq;
using UnitTests.Builders;
using Microsoft.Extensions.Options;
using MovieManager.Core.Services;
using MovieManager.Core.Settings;
using MovieManager.Core.Entities;
using MovieManager.Core.Enumerations;
using MovieManager.Infrastructure.Services;
using MovieManager.Infrastructure.Logging;
using System.Collections.Generic;
using Xunit.Abstractions;

namespace UnitTests
{
	public class MagnetScrapeServiceTests
	{
		private readonly ITestOutputHelper _output;
		private readonly IOptions<MagnetSettings> _magnetSettings;
		private readonly IOptions<CommonSettings> _commonSettings;
		private readonly MagnetScrapeService _magnetScrapeService;
		private readonly LoggerAdapter<MagnetScrapeService> _logger;
		private readonly HtmlService _htmlService;
		private readonly MovieService _movieService;
		private readonly MovieMagnetService _movieMagService;

		public MagnetScrapeServiceTests(ITestOutputHelper output)
		{
			_output = output;
			_logger = new LoggerBuilder<MagnetScrapeService>().Build();
			_magnetSettings = new MagnetSettingBuilder().Build();
			_commonSettings = new CommonSettingBuilder().Build();
			_htmlService = new HtmlService(null, _commonSettings);
			_movieService = new MovieServiceBuilder().Build();
			_movieMagService = new MovieMagServiceBuilder().Build();

			_magnetScrapeService = new MagnetScrapeService(_logger, _magnetSettings, _movieService, _movieMagService, _htmlService);
		}

		[Fact]
		public void TestGetSearchSources()	
		{
			_magnetSettings.Value.GetSearchSources().ForEach(s => _output.WriteLine(s.ToString()));
		}

		[Fact]
		public void TestScrapeMovieMagnet()
		{
			Movie movie = new Movie() { IdMovie = 1492, Number = "miaa-506" };
			_magnetScrapeService.ScrapeMovieMagnet(movie);
		}
		
		[Fact]
		public void TestSearchMagnetFromSukebei()
		{
			Movie movie = new Movie() { IdMovie = 1492, Number = "ipx-486" };
			_magnetScrapeService.SearchMagnetFromSukebei(movie).ForEach(m => _output.WriteLine($"Size: {m.Size}, isHD: {m.IsHD}, Sub: {m.HasSub} "));
		}

		[Fact]
		public void TestDailyDownloadMagnet()
		{
			List<Movie> movies = _movieService.LoadMovieToDownloadMag();
			movies.ForEach(m => _magnetScrapeService.ScrapeMovieMagnet(m));
		}

	}
}
