using Microsoft.Extensions.Options;
using MovieManager.Core.Entities;
using MovieManager.Core.Interfaces;
using MovieManager.Core.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieManager.Core.Services
{
	public class ReportService : IReportService
	{
		private readonly IScrapeReportRepo _scrapeReportRepo;
		private readonly IAppLogger<ReportService> _logger;
		private readonly ReportSettings _reportSettings;
		private readonly IEmailSender _emailSender;

		public ReportService(IAppLogger<ReportService> logger, IScrapeReportRepo scrapeReportRepo, IOptionsSnapshot<ReportSettings> reportSettings, IEmailSender emailSender)
		{
			_scrapeReportRepo = scrapeReportRepo;
			_reportSettings = reportSettings.Value;
			_emailSender = emailSender;
			_logger = logger;
		}

		public ScrapeReport SaveScrapeReport(ScrapeReport report)
		{
			return _scrapeReportRepo.Save(report);
		}

		public ScrapeReport FindScrapeReportById(int idReport)
		{
			return _scrapeReportRepo.FindById(idReport);
		}

		public ScrapeReport GetScrapeReportToSend(DateTime? dtReport = null)
		{
			return _scrapeReportRepo.GetReportToSend(dtReport);
		}

		public ScrapeReport FindScrapeReportByDate(DateTime? dtReport = null)
		{
			return _scrapeReportRepo.FindByDate(dtReport);
		}

		public void SendScrapeReport(DateTime? dtReport = null)
		{
			ScrapeReport report = GetScrapeReportToSend(dtReport);

			if(report != null)
			{
				var reportBody = new StringBuilder();
				reportBody.AppendFormat("New released movie number: {0}<br />", report.NbReleased);
				reportBody.AppendFormat("New interested movie number: {0}<br />", report.NbInterest);
				reportBody.AppendFormat("New downloaded movie number: {0}", report.NbDownload);

				var reportSubject = String.Format("{0} - {1}", _reportSettings.ScrapeReportSubject, report.DtReport.ToShortDateString());
				_emailSender.SendEmail(null, _reportSettings.ReportEmail, reportSubject, reportBody.ToString());

				report.IsSent = true;
				SaveScrapeReport(report);
			}
		}
	}
}
