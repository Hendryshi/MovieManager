using Microsoft.Extensions.Logging;
using MovieManager.Infrastructure.Logging;
using MovieManager.Infrastructure.Repositories;
using MovieManager.Infrastructure.DbContext;
using MovieManager.Core.Services;
using MovieManager.Core.Interfaces;

namespace UnitTests.Builders
{
	class MovieMagServiceBuilder
	{
		private MovieMagnetRepo _movieMagRepo;
		private DapperContext _dbContext;
		private readonly IAppLogger<MovieMagnetService> _logger;

		public MovieMagServiceBuilder()
		{
			_dbContext = new DapperContext(new ConfigBuilder().Build());
			_movieMagRepo = new MovieMagnetRepo(_dbContext);
			_logger = new LoggerBuilder<MovieMagnetService>().Build();
		}

		public MovieMagnetService Build()
		{
			return new MovieMagnetService(_logger, _movieMagRepo);
		}
	}
}
