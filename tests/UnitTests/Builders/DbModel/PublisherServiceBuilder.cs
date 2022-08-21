using Microsoft.Extensions.Logging;
using MovieManager.Infrastructure.Logging;
using MovieManager.Infrastructure.Repositories;
using MovieManager.Infrastructure.DbContext;
using MovieManager.Core.Services;
using MovieManager.Core.Interfaces;

namespace UnitTests.Builders
{
	public class PublisherServiceBuilder
	{
		private PublisherRepo _publisherRepo;
		private DapperContext _dbContext;
		private readonly IAppLogger<PublisherService> _logger;

		public PublisherServiceBuilder()
		{
			_dbContext = new DapperContext(new ConfigBuilder().Build());
			_publisherRepo = new PublisherRepo(_dbContext);
			_logger = new LoggerBuilder<PublisherService>().Build();
		}

		public PublisherService Build()
		{
			return new PublisherService(_logger, _publisherRepo);
		}
	}
}
