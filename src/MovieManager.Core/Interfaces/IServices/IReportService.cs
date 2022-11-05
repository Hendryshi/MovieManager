using MovieManager.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieManager.Core.Interfaces
{
	public interface IReportService
	{
		ScrapeReport FindScrapeReportById(int idReport);
		ScrapeReport FindScrapeReportByDate(DateTime? dtReport = null);
		ScrapeReport GetScrapeReportToSend(DateTime? dtReport = null);
		ScrapeReport SaveScrapeReport(ScrapeReport report);
		void SendScrapeReport(DateTime? dtReport = null);
	}
}
