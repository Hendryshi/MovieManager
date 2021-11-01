using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MovieManager.Core.Interfaces;
using MovieManager.Core.Entities;
using MovieManager.Core.Enumerations;

namespace MovieManager.Core.Services
{
	public class MovieMagnetService : IMovieMagnetService
	{
		private readonly IMovieMagnetRepo _movieMagnetRepo;
		private readonly IAppLogger<MovieMagnetService> _logger;

		public MovieMagnetService(IAppLogger<MovieMagnetService> logger, IMovieMagnetRepo movieMagnetRepo)
		{
			_movieMagnetRepo = movieMagnetRepo;
			_logger = logger;
		}

		public MovieMagnet SaveMovieMagnet(MovieMagnet movieMagnet)
		{
			return _movieMagnetRepo.Save(movieMagnet);
		}

		public List<MovieMagnet> SaveMovieMagnetList(List<MovieMagnet> movieMagnets)
		{
			return _movieMagnetRepo.SaveList(movieMagnets);
		}

		public MovieMagnet FindMovieMagnetById(int idMovieMag)
		{
			return _movieMagnetRepo.FindById(idMovieMag);
		}

		public MovieMagnet FindMovieMagnetByHash(string hash)
		{
			return _movieMagnetRepo.FindByHash(hash);
		}

		public List<MovieMagnet> FindMovieMagnetByMovie(Movie movie, bool onlyAlive = true)
		{
			return _movieMagnetRepo.FindByMovie(movie, onlyAlive);
		}

		public List<MovieMagnet> FindMovieMagnetByStatus(MagnetStatus magnetStatus, int idMovie = 0)
		{
			return _movieMagnetRepo.FindByStatus(magnetStatus, idMovie);
		}

		public string LoadMagSourceUrl(MagnetSource magnetSource)
		{
			return _movieMagnetRepo.LoadMagnetSource(magnetSource);
		}

		public string GenerateHashFromMagnet(string magnetUrl)
		{
			return null;
		}

		public MovieMagnet FindBestMatchMagnetByMovie(Movie movie)
		{
			bool mustHasHD = movie.IdStatus == MovieStatus.Downloaded;
			bool mustHasSub = (movie.IdStatus == MovieStatus.Downloaded || movie.IdStatus == MovieStatus.HasTorrent);
			return _movieMagnetRepo.FindByBestMatch(movie.IdMovie, mustHasHD, mustHasSub);
		}
	}
}
