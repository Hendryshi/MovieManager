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
	public class DirectorRepo : IDirectorRepo
	{
		private readonly DapperContext db;

		public DirectorRepo(DapperContext dbContext)
		{
			db = dbContext;
		}

		public Director Save(Director director)
		{
			director.DtUpdate = DateTime.Now;

			if(director.IdDirector == 0)
				director.IdDirector = (int)db.InsertEntity(director);
			else if(!db.UpdateEntity(director))
				throw new Exception($"Director not found in DB: {director.ToString()}");

			return director;
		}

		public Director FindById(int idDirector)
		{
			return db.GetEntityById<Director>(idDirector);
		}

		public Director FindByName(string name, string url = "")
		{
			var sql = new StringBuilder();
			sql.AppendLine(@"SELECT * FROM J_Director WHERE name = @name");

			if(!string.IsNullOrEmpty(url))
				sql.AppendLine("AND url = @url");

			return db.QuerySingleOrDefault<Director>(sql.ToString(), new { name = name, url = url });
		}

	}
}
