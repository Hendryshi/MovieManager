using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieManager.Core.Interfaces;
using MovieManager.Core.Entities;
using MovieManager.Infrastructure.DbContext;
using System.Transactions;
using MovieManager.Core.Enumerations;

namespace MovieManager.Infrastructure.Repositories
{
	public class MovieMagnetRepo : IMovieMagnetRepo
	{
		private readonly DapperContext db;

		public MovieMagnetRepo(DapperContext dbContext)
		{
			db = dbContext;
		}


		public MovieMagnet Save(MovieMagnet movieMagnet)
		{
			movieMagnet.DtUpdate = DateTime.Now;

			if(movieMagnet.IdMovieMag == 0)
				movieMagnet.IdMovieMag = (int)db.InsertEntity(movieMagnet);
			else if(!db.UpdateEntity(movieMagnet))
				throw new Exception($"MovieMagnet not found in DB: {movieMagnet.ToString()}");

			return movieMagnet;
		}

		public List<MovieMagnet> SaveList(List<MovieMagnet> movieMagnets)
		{
			List<MovieMagnet> lstMovieMagnets = new List<MovieMagnet>();
			using(var trans = new TransactionScope())
			{
				foreach(MovieMagnet magnet in movieMagnets)
					lstMovieMagnets.Add(Save(magnet));
				 
				trans.Complete();
			}
			return lstMovieMagnets;
		}


		public MovieMagnet FindById(int idMovieMag)
		{
			return db.GetEntityById<MovieMagnet>(idMovieMag);
		}

		public MovieMagnet FindByHash(string hash)
		{
			var sql = @"SELECT * FROM J_MovieMagnet WHERE hash = @magHash";
			return db.QuerySingleOrDefault<MovieMagnet>(sql, new { magHash = hash });
		}

		public List<MovieMagnet> FindByMovie(Movie movie, bool onlyAlive)
		{
			var sql = new StringBuilder();
			sql.AppendLine(@"SELECT * FROM J_MovieMagnet WHERE idMovie = @idMovie");

			if(onlyAlive)
				sql.AppendLine("AND idStatus > 0");

			return db.Query<MovieMagnet>(sql.ToString(), new { idMovie = movie.IdMovie });
		}

		public string LoadMagnetSource(MagnetSource magnetSource)
		{
			var sql = new StringBuilder();
			sql.AppendLine(@"SELECT descMagSource FROM D_MagnetSource WHERE idMagSource = @magnetSource");
			return db.QuerySingleOrDefault<string>(sql.ToString(), new { magnetSource = (short)magnetSource });
		}

	}
}
