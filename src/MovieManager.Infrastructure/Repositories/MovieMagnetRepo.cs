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
using MovieManager.Core.Helper;

namespace MovieManager.Infrastructure.Repositories
{
	public class MovieMagnetRepo : IMovieMagnetRepo
	{
		private readonly DapperContext db;
		private readonly IMovieHistoryRepo _movieHistoryRepo;

		public MovieMagnetRepo(DapperContext dbContext, IMovieHistoryRepo movieHistoryRepo)
		{
			db = dbContext;
			_movieHistoryRepo = movieHistoryRepo;
		}


		public MovieMagnet Save(MovieMagnet movieMagnet)
		{
			movieMagnet.DtUpdate = DateTime.Now;

			using(var trans = new TransactionScope())
			{
				List<MovieHistory> movieHistories = AddHistory(movieMagnet);
				if(movieMagnet.IdMovieMag == 0)
					movieMagnet.IdMovieMag = (int)db.InsertEntity(movieMagnet);
				else if(!db.UpdateEntity(movieMagnet))
					throw new Exception($"MovieMagnet not found in DB: {movieMagnet.ToString()}");

				_movieHistoryRepo.SaveList(movieMagnet.IdMovie, movieHistories);
			}
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

		public List<MovieMagnet> FindByStatus(MagnetStatus magnetStatus, int idMovie = 0)
		{
			var sql = new StringBuilder();
			sql.AppendLine(@"SELECT * FROM J_MovieMagnet WHERE idStatus = @status");

			if(idMovie != 0)
				sql.AppendLine("AND idMovie = @idMovie");

			return db.Query<MovieMagnet>(sql.ToString(), new { status = (short)magnetStatus, idMovie = idMovie });
		}

		public string LoadMagnetSource(MagnetSource magnetSource)
		{
			var sql = new StringBuilder();
			sql.AppendLine(@"SELECT descMagSource FROM D_MagnetSource WHERE idMagSource = @magnetSource");
			return db.QuerySingleOrDefault<string>(sql.ToString(), new { magnetSource = (short)magnetSource });
		}

		public MovieMagnet FindByBestMatch(int idMovie, bool mustHasHD = false, bool mustHasSub = false)
		{
			var sql = new StringBuilder();
			sql.AppendLine(@"SELECT TOP 1 * FROM J_MovieMagnet");
			sql.AppendLine("WHERE idMovie = @idMovie");
			sql.AppendLine("AND idStatus = 1");

			if(mustHasHD)
				sql.AppendLine("AND isHD = 1");

			if(mustHasSub)
				sql.AppendLine("AND hasSub = 1");

			sql.AppendLine("ORDER BY hasSub, isHD DESC, idMagSource ASC, size, dtMagnet DESC");

			return db.QuerySingleOrDefault<MovieMagnet>(sql.ToString(), new { idMovie = idMovie });
		}

		//TODO: Consider to group the history into one record
		public List<MovieHistory> AddHistory(MovieMagnet magnet)
		{
			List<MovieHistory> movieHistories = new List<MovieHistory>();

			if(magnet.IdMovieMag == 0)
			{
				movieHistories.Add(new MovieHistory()
				{
					DescHistory = string.Format("Create new movie magnet {0}", magnet.Hash)
				});
			}
			else
			{
				MovieMagnet origin = FindById(magnet.IdMovieMag);
				string result = string.Empty;
				
				if(HistoryDiffHelpers.GetDifferencesFieldString(string.Format("MagnetId {0} - {1}", magnet.IdMovieMag, "MagName"), origin.MagName, magnet.MagName, ref result))
					movieHistories.Add(new MovieHistory() { DescHistory = result });

				if(HistoryDiffHelpers.GetDifferencesFieldString(string.Format("MagnetId {0} - {1}", magnet.IdMovieMag, "MagnetUrl"), origin.MagnetUrl, magnet.MagnetUrl, ref result))
					movieHistories.Add(new MovieHistory() { DescHistory = result });

				if(HistoryDiffHelpers.GetDifferencesFieldString(string.Format("MagnetId {0} - {1}", magnet.IdMovieMag, "Hash"), origin.Hash, magnet.Hash, ref result))
					movieHistories.Add(new MovieHistory() { DescHistory = result });

				if(HistoryDiffHelpers.GetDifferencesFieldDemical(string.Format("MagnetId {0} - {1}", magnet.IdMovieMag, "Size"), origin.Size, magnet.Size, ref result))
					movieHistories.Add(new MovieHistory() { DescHistory = result });

				if(HistoryDiffHelpers.GetDifferencesFieldDate(string.Format("MagnetId {0} - {1}", magnet.IdMovieMag, "Magnet Date"), origin.DtMagnet, magnet.DtMagnet, ref result))
					movieHistories.Add(new MovieHistory() { DescHistory = result });

				if(HistoryDiffHelpers.GetDifferencesFieldBoolean(string.Format("MagnetId {0} - {1}", magnet.IdMovieMag, "IsHD"), origin.IsHD, magnet.IsHD, ref result))
					movieHistories.Add(new MovieHistory() { DescHistory = result });

				if(HistoryDiffHelpers.GetDifferencesFieldBoolean(string.Format("MagnetId {0} - {1}", magnet.IdMovieMag, "HasSub"), origin.HasSub, magnet.HasSub, ref result))
					movieHistories.Add(new MovieHistory() { DescHistory = result });

				if(HistoryDiffHelpers.GetDifferencesFieldString(string.Format("MagnetId {0} - {1}", magnet.IdMovieMag, "Magnet Source"), origin.IdMagSource.ToString(), magnet.IdMagSource.ToString(), ref result))
					movieHistories.Add(new MovieHistory() { DescHistory = result });

				if(HistoryDiffHelpers.GetDifferencesFieldDate(string.Format("MagnetId {0} - {1}", magnet.IdMovieMag, "DtStart"), origin.DtStart, magnet.DtStart, ref result))
					movieHistories.Add(new MovieHistory() { DescHistory = result });

				if(HistoryDiffHelpers.GetDifferencesFieldDate(string.Format("MagnetId {0} - {1}", magnet.IdMovieMag, "DtFinish"), origin.DtFinish, magnet.DtFinish, ref result))
					movieHistories.Add(new MovieHistory() { DescHistory = result });

				if(HistoryDiffHelpers.GetDifferencesFieldString(string.Format("MagnetId {0} - {1}", magnet.IdMovieMag, "SavePath"), origin.SavePath, magnet.SavePath, ref result))
					movieHistories.Add(new MovieHistory() { DescHistory = result });

				if(HistoryDiffHelpers.GetDifferencesFieldString(string.Format("MagnetId {0} - {1}", magnet.IdMovieMag, "Magnet Status"), origin.IdStatus.ToString(), magnet.IdStatus.ToString(), ref result))
					movieHistories.Add(new MovieHistory() { DescHistory = result });

			}
			return movieHistories;
		}
	}
}
