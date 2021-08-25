using Microsoft.Extensions.Logging;
using MovieManager.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieManager.Core.Job
{
	public class TestJob : ITestJob
	{
		private MovieManager.Infrastructure.Repositories.BaseRepository _baseRepository;
		private readonly ILogger<TestJob> _logger;


		public TestJob(Infrastructure.Repositories.BaseRepository baseRepository, ILogger<TestJob> logger)
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
