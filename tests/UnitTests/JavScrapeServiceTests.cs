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
	public class JavScrapeServiceTests
	{
		private readonly ITestOutputHelper _output;
		private readonly IOptions<JavlibSettings> _javlibSettings;
		private readonly IOptions<CommonSettings> _commonSettings;
		private readonly LoggerAdapter<JavScrapeService> _logger;
		private readonly HtmlService _htmlService;
		private readonly MovieService _movieService;
		private readonly JavScrapeService _javScrapeService;

		public JavScrapeServiceTests(ITestOutputHelper output)
		{
			_output = output;
			_logger = new LoggerBuilder<JavScrapeService>().Build();
			_javlibSettings = new JavlibSettingBuilder().Build();
			_commonSettings = new CommonSettingBuilder().Build();
			_htmlService = new HtmlService(null, _commonSettings);
			_movieService = new MovieServiceBuilder().Build();
			_javScrapeService = new JavScrapeService(_logger, _javlibSettings, _movieService, _htmlService);
		}

		[Fact]
		public void TestGetPageCount()	
		{
			int pageCount = _javScrapeService.GetPageCount(new UrlInfo() { EntryType = JavlibEntryType.NewRelease });
			Assert.Equal(25, pageCount);
		}

		[Fact]
		public void TestRetryGetHtmlDocument()
		{
			int pageCount = _javScrapeService.GetPageCount(new UrlInfo() { EntryType = JavlibEntryType.Other, ExactUrl = "https://www.javlibrary.com/cn/?v=javme5ym35555" });
			Assert.Equal(0, pageCount);
		}

		[Fact]
		public void TestScanPageList()
		{
			UrlInfo urlInfo = new UrlInfo() { EntryType = JavlibEntryType.NewRelease, Page = 2 };
			List<Movie> lstMovies = _javScrapeService.ScanPageList(urlInfo);

			foreach(Movie m in lstMovies)
				_output.WriteLine(m.Number);

			Assert.Equal(20, lstMovies.Count);
		}

		//TODO: This should be placed in Functional Tests
		[Fact]
		public void TestScrapeNewReleasedMovie()
		{
			_javScrapeService.ScrapeNewReleasedMovie();
		}

	}
}
