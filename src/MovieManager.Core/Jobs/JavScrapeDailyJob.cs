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
		private readonly IJavScrapeService _javScrapeService;

		public JavScrapeDailyJob(IAppLogger<JavScrapeDailyJob> logger, IJavScrapeService javScrapeService)
		{
			_logger = logger;
			_javScrapeService = javScrapeService;
		}

		public void Run()
		{
			_logger.LogInformation("Job Enter");
			_javScrapeService.ScrapeNewReleasedMovie();
		}
	}
}
