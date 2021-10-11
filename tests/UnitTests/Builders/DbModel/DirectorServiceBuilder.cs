using Microsoft.Extensions.Logging;
using MovieManager.Infrastructure.Logging;
using MovieManager.Infrastructure.Repositories;
using MovieManager.Infrastructure.DbContext;
using MovieManager.Core.Services;
using MovieManager.Core.Interfaces;

namespace UnitTests.Builders
{
	class DirectorServiceBuilder
	{
		private DirectorRepo _directorRepo;
		private DapperContext _dbContext;
		private readonly IAppLogger<DirectorService> _logger;

		public DirectorServiceBuilder()
		{
			_dbContext = new DapperContext(new ConfigBuilder().Build());
			_directorRepo = new DirectorRepo(_dbContext);
			_logger = new LoggerBuilder<DirectorService>().Build();
		}

		public DirectorService Build()
		{
			return new DirectorService(_logger, _directorRepo);
		}
	}
}
