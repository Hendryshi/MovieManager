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
using System.Linq;

namespace UnitTests
{
	public class JavScrapeTests
	{
		private readonly ITestOutputHelper _output;
		private readonly IOptionsSnapshot<JavlibSettings> _javlibSettings;
		private readonly IOptionsSnapshot<CommonSettings> _commonSettings;
		private readonly LoggerAdapter<JavScrapeService> _logger;
		private readonly HtmlService _htmlService;
		private readonly MovieService _movieService;
		private readonly CategoryService _categoryService;
		private readonly CompanyService _companyService;
		private readonly DirectorService _directorService;
		private readonly ActorService _actorService;
		private readonly PublisherService _publisherService;
		private readonly JavScrapeService _javScrapeService;
		private readonly ReportService _reportService;

		public JavScrapeTests(ITestOutputHelper output)
		{
			_output = output;
			_logger = new LoggerBuilder<JavScrapeService>().Build();
			_javlibSettings = new JavlibSettingBuilder().Build();
			_commonSettings = new CommonSettingBuilder().Build();
			_htmlService = new HtmlService(null, _commonSettings);
			_movieService = new MovieServiceBuilder().Build();
			_categoryService = new CategoryServiceBuilder().Build();
			_companyService = new CompanyServiceBuilder().Build();
			_directorService = new DirectorServiceBuilder().Build();
			_actorService = new ActorServiceBuilder().Build();
			_publisherService = new PublisherServiceBuilder().Build();
			_reportService = new ReportServiceBuilder().Build();
			
			_javScrapeService = new JavScrapeService(_logger, _javlibSettings, _movieService, _actorService, _categoryService, _companyService, _directorService, _publisherService, _htmlService, _reportService);
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
				_output.WriteLine(m.ThumbnailUrl);

			Assert.Equal(20, lstMovies.Count);
		}

		[Fact]
		public void Test_ListMovie_RemoveAll_IdMovie_Not_Equal_Zero()
		{
			List<Movie> movies = new List<Movie>() {
				new Movie(),
				new Movie() { IdMovie = 1 },
				new Movie() { IdMovie = 2 }
			};
			movies.RemoveAll(x => x.IdMovie != 0);
			Assert.Single(movies);
		}

		[Fact]
		public void Test_ListMovie_Select_Max_Point()
		{
			List<Movie> movies = new List<Movie>() {
				new Movie() { IdMovie = 1, Number = "1", NbWatched = 1, NbOwned = 1, NbWant = 1 },
				new Movie() { IdMovie = 2, Number = "1", NbWatched = 100, NbWant = 1, NbOwned = 1 },
				new Movie() { IdMovie = 3, Number = "2", NbWatched = 1, NbOwned = 1, NbWant = 1 }
			};

			movies = movies.GroupBy(x => x.Number.ToUpper(),
				(key, g) => g.OrderByDescending(e => e.NbWant + e.NbOwned + e.NbWatched).First()).ToList();

			movies.ForEach(m => _output.WriteLine(m.ToString()));

			Assert.Equal(2, movies.Count);
			Assert.Equal(100, movies.Find(x => x.IdMovie == 2).NbWatched);
		}

		[Fact]
		public void TestScanMovieDetails()
		{
			UrlInfo urlInfo = new UrlInfo() { EntryType = JavlibEntryType.Movie, ExactUrl = "?v=javme5yc4ufdfdfd" };
			Movie movie = new Movie();

			_javScrapeService.ScanMovieDetails(urlInfo, movie);
			_output.WriteLine(movie.ToString());
		}

		//TODO: This should be placed in Functional Tests
		[Fact]
		public void TestScrapeNewReleasedMovie()
		{
			_javScrapeService.ScrapeNewReleasedMovie();
		}

		[Fact]
		public void TestScrapeMoviesByActor()
		{
			string name = "Ð¡»¨¤Î¤ó";
			List<Movie> movies = _javScrapeService.ScrapreMoviesByActor(name);

			List<Movie> results = movies.OrderByDescending(x => x.NbOwned + x.NbWant + x.NbWatched).Take(20).ToList();
			results.ForEach(x => _output.WriteLine($"{x.Number}: {x.NbOwned + x.NbWant + x.NbWatched}"));
		}

		[Fact]
		public void TestScrapeMoviesByKeyWord()
		{
			string name = "STAR";
			List<Movie> movies = _javScrapeService.ScrapreMoviesByKeyWord(name);

			List<Movie> results = movies.OrderByDescending(x => x.NbOwned + x.NbWant + x.NbWatched).Take(30).ToList();
			results.ForEach(x => _output.WriteLine($"{x.Number}: {x.NbOwned + x.NbWant + x.NbWatched}"));
		}
	}
}
