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
	public class MovieRepo : IMovieRepo
	{
		private readonly DapperContext db;
		private readonly IMovieRelationRepo _movieRelationRepo;

		public MovieRepo(DapperContext dbContext, IMovieRelationRepo movieRelationRepo)
		{
			db = dbContext;
			_movieRelationRepo = movieRelationRepo;
		}

		public Movie Save(Movie movie)
		{
			movie.DtUpdate = DateTime.Now;

			using(var trans = new TransactionScope())
			{
				if(movie.IdMovie == 0)
					movie.IdMovie = (int)db.InsertEntity(movie);
				else if(!db.UpdateEntity(movie))
					throw new Exception($"Movie not found in DB: {movie.ToString()}");

				_movieRelationRepo.SaveAllRelations(movie.IdMovie, movie.MovieRelations);
				trans.Complete();
			}
			return movie;
		}

		public Movie FindById(int idMovie)
		{
			Movie movie = db.GetEntityById<Movie>(idMovie);
			if(movie != null)
				movie.MovieRelations = _movieRelationRepo.LoadAllRelations(idMovie);

			return movie;
		}

		public Movie FindByNumber(string movieNbr)
		{
			var sql = @"SELECT * FROM J_Movie WHERE number = @number";
			Movie movie = db.QuerySingleOrDefault<Movie>(sql, new { number = movieNbr });

			if(movie != null)
				movie.MovieRelations = _movieRelationRepo.LoadAllRelations(movie.IdMovie);

			return movie;
		}

		public List<Movie> LoadMovieToDownloadMag()
		{
			var sql = new StringBuilder();
			sql.AppendLine(@"SELECT idMovie FROM vMovieToDownloadMag");
			sql.AppendLine("ORDER BY dtRelease DESC, hasSub, hasHD");
			List<int> lstIdMovies = db.Query<int>(sql.ToString());
			return lstIdMovies.Select(id => FindById(id)).ToList();
		}
	}
}
