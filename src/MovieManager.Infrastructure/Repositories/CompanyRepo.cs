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
	public class CompanyRepo : ICompanyRepo
	{
		private readonly DapperContext db;

		public CompanyRepo(DapperContext dbContext)
		{
			db = dbContext;
		}

		public Company Save(Company company)
		{
			company.DtUpdate = DateTime.Now;

			if(company.IdCompany == 0)
				company.IdCompany = (int)db.InsertEntity(company);
			else if(!db.UpdateEntity(company))
				throw new Exception($"Company not found in DB: {company.ToString()}");

			return company;
		}

		public Company FindById(int idCompany)
		{
			return db.GetEntityById<Company>(idCompany);
		}

		public Company FindByName(string name, string url = "")
		{
			var sql = new StringBuilder();
			sql.AppendLine(@"SELECT * FROM J_Company WHERE name = @name");

			if(!string.IsNullOrEmpty(url))
				sql.AppendLine("AND url = @url");

			return db.QuerySingleOrDefault<Company>(sql.ToString(), new { name = name, url = url });
		}

	}
}
