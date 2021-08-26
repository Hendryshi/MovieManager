using Microsoft.Extensions.Logging;
using MovieManager.Infrastructure.Repositories;
using MovieManager.Core.Interfaces;
using MovieManager.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieManager.Core.Jobs
{
	public class JavScrapeDailyJob : IJavScrapeDailyJob
	{
		private BaseRepository _baseRepository;
		private readonly ILogger<JavScrapeDailyJob> _logger;


		public JavScrapeDailyJob(BaseRepository baseRepository, ILogger<JavScrapeDailyJob> logger)
		{
			_baseRepository = baseRepository;
			_logger = logger;
		}

		public void Run()
		{
			_logger.LogInformation("Job Enter");
		}
	}
}
