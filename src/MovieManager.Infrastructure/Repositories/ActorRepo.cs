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
	public class ActorRepo : IActorRepo
	{
		private readonly DapperContext db;

		public ActorRepo(DapperContext dbContext)
		{
			db = dbContext;
		}

		public Actor Save(Actor actor)
		{
			actor.DtUpdate = DateTime.Now;

			if(actor.IdActor == 0)
				actor.IdActor = (int)db.InsertEntity(actor);
			else if(!db.UpdateEntity(actor))
				throw new Exception($"Actor not found in DB: {actor.ToString()}");

			return actor;
		}

		public Actor FindById(int idActor)
		{
			return db.GetEntityById<Actor>(idActor);
		}

		public Actor FindByName(string name, string url = "")
		{
			var sql = new StringBuilder();
			sql.AppendLine(@"SELECT * FROM J_Actor WHERE name = @name");

			if(!string.IsNullOrEmpty(url))
				sql.AppendLine("AND url = @url");

			return db.QuerySingleOrDefault<Actor>(sql.ToString(), new { name = name, url = url });
		}

	}
}
