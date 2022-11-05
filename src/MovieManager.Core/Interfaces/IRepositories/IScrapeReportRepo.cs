using MovieManager.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieManager.Core.Interfaces
{
	public interface IScrapeReportRepo
	{
		ScrapeReport FindById(int idReport);
		ScrapeReport FindByDate(DateTime? dtReport = null);
		ScrapeReport GetReportToSend(DateTime? dtReport = null);
		ScrapeReport Save(ScrapeReport report);
	}
}
