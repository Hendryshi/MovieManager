using Microsoft.Extensions.Logging;
using MovieManager.Infrastructure.Logging;
using MovieManager.Infrastructure.Repositories;
using MovieManager.Infrastructure.DbContext;
using MovieManager.Core.Services;
using MovieManager.Core.Interfaces;

namespace UnitTests.Builders
{
	public class ActorServiceBuilder
	{
		private ActorRepo _actorRepo;
		private DapperContext _dbContext;
		private readonly IAppLogger<ActorService> _logger;

		public ActorServiceBuilder()
		{
			_dbContext = new DapperContext(new ConfigBuilder().Build());
			_actorRepo = new ActorRepo(_dbContext);
			_logger = new LoggerBuilder<ActorService>().Build();
		}

		public ActorService Build()
		{
			return new ActorService(_logger, _actorRepo);
		}
	}
}
