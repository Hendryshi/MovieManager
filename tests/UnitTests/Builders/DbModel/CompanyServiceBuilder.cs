using Microsoft.Extensions.Logging;
using MovieManager.Infrastructure.Logging;
using MovieManager.Infrastructure.Repositories;
using MovieManager.Infrastructure.DbContext;
using MovieManager.Core.Services;
using MovieManager.Core.Interfaces;

namespace UnitTests.Builders
{
	public class CompanyServiceBuilder
	{
		private CompanyRepo _companyRepo;
		private DapperContext _dbContext;
		private readonly IAppLogger<CompanyService> _logger;

		public CompanyServiceBuilder()
		{
			_dbContext = new DapperContext(new ConfigBuilder().Build());
			_companyRepo = new CompanyRepo(_dbContext);
			_logger = new LoggerBuilder<CompanyService>().Build();
		}

		public CompanyService Build()
		{
			return new CompanyService(_logger, _companyRepo);
		}
	}
}
