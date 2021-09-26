using Microsoft.Extensions.Logging;
using MovieManager.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieManager.Core.Entities;
using MovieManager.Core.Enumerations;

namespace MovieManager.Core.Jobs
{
	public class JavScrapeDailyJob : IJavScrapeDailyJob
	{
		private readonly IAppLogger<JavScrapeDailyJob> _logger;
		private readonly IMovieService _movieService;
		private readonly IJavScrapeService _javScrapeService;

		public JavScrapeDailyJob(IAppLogger<JavScrapeDailyJob> logger, 
			IMovieService movieService,
			IJavScrapeService javScrapeService)
		{
			_logger = logger;
			_movieService = movieService;
			_javScrapeService = javScrapeService;
		}

		public void Run()
		{
			_logger.LogInformation("Job Enter");
			//_javScrapeService.GetPageCount(new UrlInfo() { EntryType = JavlibEntryType.Other, ExactUrl = "https://www.javlibrary.com/cn/?v=javme5ym35555" });
		}
	}
}
