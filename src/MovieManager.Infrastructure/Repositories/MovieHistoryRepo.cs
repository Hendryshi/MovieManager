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
	public class MovieHistoryRepo : IMovieHistoryRepo
	{
		private readonly DapperContext db;

		public MovieHistoryRepo(DapperContext dbContext)
		{
			db = dbContext;
		}

		public MovieHistory Save(int idMovie, MovieHistory history)
		{
			history.DtCreation = DateTime.Now;
			history.IdMovie = idMovie;

			if(history.IdMovieHistory == 0)
				history.IdMovieHistory = (int)db.InsertEntity(history);
			else
				throw new Exception($"Already have an ID for this movieHistory: {history.IdMovieHistory}");

			return history;
		}

		public List<MovieHistory> SaveList(int idMovie, List<MovieHistory> movieHistories)
		{
			List<MovieHistory> lstMovieHistories = new List<MovieHistory>();
			using(var trans = new TransactionScope())
			{
				foreach(MovieHistory history in movieHistories)
					lstMovieHistories.Add(Save(idMovie, history));

				trans.Complete();
			}
			return lstMovieHistories;
		}

		public List<MovieHistory> LoadByIdMovie(int idMovie)
		{
			var sql = new StringBuilder();
			sql.AppendLine(@"SELECT * FROM J_MovieHistory WHERE idMovie = @idMovie");

			return db.Query<MovieHistory>(sql.ToString(), new { idMovie = idMovie});
		}

	}
}
