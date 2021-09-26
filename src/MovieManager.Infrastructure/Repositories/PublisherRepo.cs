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
	public class PublisherRepo : IPublisherRepo
	{
		private readonly DapperContext db;

		public PublisherRepo(DapperContext dbContext)
		{
			db = dbContext;
		}

		public Publisher Save(Publisher publisher)
		{
			publisher.DtUpdate = DateTime.Now;

			if(publisher.IdPublisher == 0)
				publisher.IdPublisher = (int)db.InsertEntity(publisher);
			else if(!db.UpdateEntity(publisher))
				throw new Exception($"Publisher not found in DB: {publisher.ToString()}");

			return publisher;
		}

		public Publisher FindById(int idPublisher)
		{
			return db.GetEntityById<Publisher>(idPublisher);
		}

		public Publisher FindByName(string name, string url = "")
		{
			var sql = new StringBuilder();
			sql.AppendLine(@"SELECT * FROM J_Publisher WHERE name = @name");

			if(!string.IsNullOrEmpty(url))
				sql.AppendLine("AND url = @url");

			return db.QuerySingleOrDefault<Publisher>(sql.ToString(), new { name = name, url = url });
		}

	}
}
