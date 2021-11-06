using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieManager.Core.Interfaces;
using MovieManager.Core.Entities;
using MovieManager.Infrastructure.DbContext;
using System.Transactions;
using MovieManager.Core.Helper;

namespace MovieManager.Infrastructure.Repositories
{
	public class MovieRepo : IMovieRepo
	{
		private readonly DapperContext db;
		private readonly IMovieRelationRepo _movieRelationRepo;
		private readonly IMovieHistoryRepo _movieHistoryRepo;

		public MovieRepo(DapperContext dbContext, IMovieRelationRepo movieRelationRepo, IMovieHistoryRepo movieHistoryRepo)
		{
			db = dbContext;
			_movieRelationRepo = movieRelationRepo;
			_movieHistoryRepo = movieHistoryRepo;
		}

		public Movie Save(Movie movie)
		{
			movie.DtUpdate = DateTime.Now;

			using(var trans = new TransactionScope())
			{
				List<MovieHistory> movieHistories = AddHistory(movie);
				if(movie.IdMovie == 0)
					movie.IdMovie = (int)db.InsertEntity(movie);
				else if(!db.UpdateEntity(movie))
					throw new Exception($"Movie not found in DB: {movie.ToString()}");

				_movieRelationRepo.SaveAllRelations(movie.IdMovie, movie.MovieRelations);
				_movieHistoryRepo.SaveList(movie.IdMovie, movieHistories);
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

		public List<Movie> LoadMoviesToScrapeMagnet()
		{
			var sql = new StringBuilder();
			sql.AppendLine(@"SELECT idMovie FROM vMovieInfo");
			sql.AppendLine("WHERE (idFavLevel > 0 AND nbMag = 0) OR (idFavLevel > 2 AND hasSub = 0)");
			sql.AppendLine("AND DATEADD(MONTH, 2, dtRelease) > GETDATE()");
			sql.AppendLine("ORDER BY isUrgent DESC, dtRelease DESC, hasSub, hasHD");
			List<int> lstIdMovies = db.Query<int>(sql.ToString());
			return lstIdMovies.Select(id => FindById(id)).ToList();
		}

		public List<Movie> LoadMoviesToDownload()
		{
			var sql = new StringBuilder();
			sql.AppendLine("SELECT idMovie FROM vMovieInfo");
			sql.AppendLine("WHERE idMovie NOT IN (SELECT idMovie FROM J_MovieMagnet WHERE idStatus IN (2,4))");
			sql.AppendLine("AND ((idStMovie = 3 AND idFavLevel = 3 AND hasSub = 1 AND hasHD = 1) OR (idStMovie = 2 AND idFavLevel > 1 AND hasHD = 1))");
			sql.AppendLine("AND DATEADD(YEAR, 1, dtRelease) > GETDATE()");
			sql.AppendLine("ORDER BY isUrgent DESC, dtRelease DESC, idStMovie DESC");
			List<int> lstIdMovies = db.Query<int>(sql.ToString());
			return lstIdMovies.Select(id => FindById(id)).ToList();
		}

		public List<MovieHistory> AddHistory(Movie movie)
		{
			List<MovieHistory> movieHistories = new List<MovieHistory>();

			if(movie.IdMovie == 0)
			{
				movieHistories.Add(new MovieHistory()
				{
					DescHistory = string.Format("Create new movie item {0}", movie.Number)
				});
			}
			else
			{
				Movie origin = FindById(movie.IdMovie);
				string result = string.Empty;

				if(HistoryDiffHelpers.GetDifferencesFieldString("Title", origin.Title, movie.Title, ref result))
					movieHistories.Add(new MovieHistory() { DescHistory = result });

				if(HistoryDiffHelpers.GetDifferencesFieldString("Company", origin.Company, movie.Company, ref result))
					movieHistories.Add(new MovieHistory() { DescHistory = result });

				if(HistoryDiffHelpers.GetDifferencesFieldString("Director", origin.Director, movie.Director, ref result))
					movieHistories.Add(new MovieHistory() { DescHistory = result });

				if(HistoryDiffHelpers.GetDifferencesFieldString("Publisher", origin.Publisher, movie.Publisher, ref result))
					movieHistories.Add(new MovieHistory() { DescHistory = result });

				if(HistoryDiffHelpers.GetDifferencesFieldString("Category", origin.Category, movie.Category, ref result))
					movieHistories.Add(new MovieHistory() { DescHistory = result });

				if(HistoryDiffHelpers.GetDifferencesFieldString("Actor", origin.Actor, movie.Actor, ref result))
					movieHistories.Add(new MovieHistory() { DescHistory = result });

				if(HistoryDiffHelpers.GetDifferencesFieldDate("Release Date", origin.DtRelease, movie.DtRelease, ref result))
					movieHistories.Add(new MovieHistory() { DescHistory = result });

				if(HistoryDiffHelpers.GetDifferencesFieldInteger("NbWant", origin.NbWant, movie.NbWant, ref result))
					movieHistories.Add(new MovieHistory() { DescHistory = result });

				if(HistoryDiffHelpers.GetDifferencesFieldInteger("NbWatched", origin.NbWatched, movie.NbWatched, ref result))
					movieHistories.Add(new MovieHistory() { DescHistory = result });

				if(HistoryDiffHelpers.GetDifferencesFieldInteger("NbOwned", origin.NbOwned, movie.NbOwned, ref result))
					movieHistories.Add(new MovieHistory() { DescHistory = result });

				if(HistoryDiffHelpers.GetDifferencesFieldInteger("Duration", origin.Duration, movie.Duration, ref result))
					movieHistories.Add(new MovieHistory() { DescHistory = result });

				if(HistoryDiffHelpers.GetDifferencesFieldString("ThumbnailUrl", origin.ThumbnailUrl, movie.ThumbnailUrl, ref result))
					movieHistories.Add(new MovieHistory() { DescHistory = result });

				if(HistoryDiffHelpers.GetDifferencesFieldString("CoverUrl", origin.CoverUrl, movie.CoverUrl, ref result))
					movieHistories.Add(new MovieHistory() { DescHistory = result });

				if(HistoryDiffHelpers.GetDifferencesFieldString("Favorite level", origin.FavLevel.ToString(), movie.FavLevel.ToString(), ref result))
					movieHistories.Add(new MovieHistory() { DescHistory = result });

				if(HistoryDiffHelpers.GetDifferencesFieldString("Movie status", origin.IdStatus.ToString(), movie.IdStatus.ToString(), ref result))
					movieHistories.Add(new MovieHistory() { DescHistory = result });
			}
			return movieHistories;
		}
	}
}
