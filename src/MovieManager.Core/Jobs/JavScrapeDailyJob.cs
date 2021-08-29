using Microsoft.Extensions.Logging;
using MovieManager.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieManager.Core.Jobs
{
	public class JavScrapeDailyJob : IJavScrapeDailyJob
	{
		private readonly IAppLogger<JavScrapeDailyJob> _logger;
		private readonly IMovieService _movieService;

		public JavScrapeDailyJob(IAppLogger<JavScrapeDailyJob> logger, 
			IMovieService movieService)
		{
			_logger = logger;
			_movieService = movieService;
		}

		public void Run()
		{
			_logger.LogInformation("Job Enter");
		}
	}
}
