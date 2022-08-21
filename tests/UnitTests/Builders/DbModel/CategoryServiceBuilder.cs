using Microsoft.Extensions.Logging;
using MovieManager.Infrastructure.Logging;
using MovieManager.Infrastructure.Repositories;
using MovieManager.Infrastructure.DbContext;
using MovieManager.Core.Services;
using MovieManager.Core.Interfaces;

namespace UnitTests.Builders
{
	public class CategoryServiceBuilder
	{
		private CategoryRepo _categoryRepo;
		private DapperContext _dbContext;
		private readonly IAppLogger<CategoryService> _logger;

		public CategoryServiceBuilder()
		{
			_dbContext = new DapperContext(new ConfigBuilder().Build());
			_categoryRepo = new CategoryRepo(_dbContext);
			_logger = new LoggerBuilder<CategoryService>().Build();
		}

		public CategoryService Build()
		{
			return new CategoryService(_logger, _categoryRepo);
		}
	}
}
