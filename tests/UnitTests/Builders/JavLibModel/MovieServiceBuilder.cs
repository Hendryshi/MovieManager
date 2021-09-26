using Microsoft.Extensions.Logging;
using MovieManager.Infrastructure.Logging;
using MovieManager.Infrastructure.Repositories;
using MovieManager.Infrastructure.DbContext;
using MovieManager.Core.Services;
using MovieManager.Core.Interfaces;

namespace UnitTests.Builders
{
	class MovieServiceBuilder
	{
		private MovieRepo _movieRepo;
		private MovieRelationRepo _movieRelationRepo;
		private DapperContext _dbContext;
		private readonly IAppLogger<MovieService> _logger;

		public MovieServiceBuilder()
		{
			_dbContext = new DapperContext(new ConfigBuilder().Build());
			_movieRelationRepo = new MovieRelationRepo(_dbContext);
			_movieRepo = new MovieRepo(_dbContext, _movieRelationRepo);
			_logger = new LoggerBuilder<MovieService>().Build();
		}

		public MovieService Build()
		{
			return new MovieService(_logger, _movieRepo);
		}
	}
}
