using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieManager.Core.Interfaces;
using MovieManager.Core.Entities;
using MovieManager.Infrastructure.DbContext;

namespace MovieManager.Infrastructure.Repositories
{
	public class MovieRepository : IMovieRepository
	{
		private readonly DapperContext db;

		public MovieRepository(DapperContext dbContext)
		{
			db = dbContext;
		}

		public Movie Save(Movie movie)
		{
			movie.DtUpdate = DateTime.Now;

			if(movie.IdMovie == 0)
				movie.IdMovie = (int)db.InsertEntity(movie);
			else if(!db.UpdateEntity(movie))
				throw new Exception($"Movie not found in DB: {movie.ToString()}");

			return movie;
		}

		public Movie FindById(int idMovie)
		{
			return db.GetEntityById<Movie>(idMovie);
		}

		public Movie FindByNumber(string movieNbr)
		{
			var sql = @"SELECT * FROM J_Movie WHERE number = @number";
			return db.QuerySingleOrDefault<Movie>(sql, new { number = movieNbr });
		}
	}
}
