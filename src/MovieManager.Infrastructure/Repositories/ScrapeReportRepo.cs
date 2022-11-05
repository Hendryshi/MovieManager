using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieManager.Core.Interfaces;
using MovieManager.Core.Entities;
using MovieManager.Infrastructure.DbContext;
using System.Transactions;

namespace MovieManager.Infrastructure.Repositories
{
	public class ScrapeReportRepo : IScrapeReportRepo
	{
		private readonly DapperContext db;

		public ScrapeReportRepo(DapperContext dbContext)
		{
			db = dbContext;
		}

		public ScrapeReport Save(ScrapeReport report)
		{
			if(report.IdReport == 0)
				report.IdReport = (int)db.InsertEntity(report);
			else if(!db.UpdateEntity(report))
				throw new Exception($"Report not found in DB: {report.ToString()}");

			return report;
		}


		public Actor FindByName(string name, string url = "")
		{
			var sql = new StringBuilder();
			sql.AppendLine(@"SELECT * FROM J_Actor WHERE name = @name");

			if(!string.IsNullOrEmpty(url))
				sql.AppendLine("AND url = @url");

			return db.QuerySingleOrDefault<Actor>(sql.ToString(), new { name = name, url = url });
		}

		public ScrapeReport FindById(int idReport)
		{
			return db.GetEntityById<ScrapeReport>(idReport);
		}

		public ScrapeReport GetReportToSend(DateTime? dtReport = null)
		{
			var sql = new StringBuilder();

			sql.AppendLine(@"SELECT * FROM J_ScrapeReport WHERE isSent = 0");
			sql.AppendLine("AND dtReport = @dtReport");

			return db.QuerySingleOrDefault<ScrapeReport>(sql.ToString(), new { dtReport = dtReport ?? DateTime.Today });
		}

		public ScrapeReport FindByDate(DateTime? dtReport = null)
		{
			var sql = new StringBuilder();
			sql.AppendLine(@"SELECT * FROM J_ScrapeReport WHERE dtReport = @dtReport");
			
			return db.QuerySingleOrDefault<ScrapeReport>(sql.ToString(), new { dtReport = dtReport ?? DateTime.Today });
		}
	}
}
