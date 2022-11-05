using Microsoft.Extensions.Logging;
using MovieManager.Infrastructure.Logging;
using MovieManager.Infrastructure.Repositories;
using MovieManager.Infrastructure.DbContext;
using MovieManager.Core.Services;
using MovieManager.Infrastructure.Services;
using Microsoft.Extensions.Options;
using MovieManager.Core.Settings;

namespace UnitTests.Builders
{
	public class ReportServiceBuilder
	{
		private ScrapeReportRepo _scrapeReportRepo;
		private DapperContext _dbContext;
		private EmailSender _emailSender;
		private readonly LoggerAdapter<ReportService> _logger;
		private readonly IOptionsSnapshot<ReportSettings> _reportSettings;

		public ReportServiceBuilder()
		{
			_dbContext = new DapperContext(new ConfigBuilder().Build());
			_scrapeReportRepo = new ScrapeReportRepo(_dbContext);
			_emailSender = new EmailSenderBuilder().Build();
			_reportSettings = new ReportSettingBuilder().Build();
			_logger = new LoggerBuilder<ReportService>().Build();
		}

		public ReportService Build()
		{
			return new ReportService(_logger, _scrapeReportRepo, _reportSettings, _emailSender);
		}
	}
}
