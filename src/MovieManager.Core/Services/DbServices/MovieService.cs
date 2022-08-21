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
	public class MovieService : IMovieService
	{
		private readonly IMovieRepo _movieRepository;
		private readonly IAppLogger<MovieService> _logger;

		public MovieService(IAppLogger<MovieService> logger, IMovieRepo movieRepository)
		{
			_movieRepository = movieRepository;
			_logger = logger;
		}

		public Movie SaveMovie(Movie movie, bool updateRelation = false)
		{
			return _movieRepository.Save(movie, updateRelation);
		}

		public Movie UpdateStatus(Movie movie, MovieStatus newStatus)
		{
			bool changeAllowed = false;

			if(movie.IdStatus != newStatus)
			{
				switch(newStatus)
				{
					case MovieStatus.InError:
						changeAllowed = true;
						break;
					case MovieStatus.Downloaded:
						if(movie.IdStatus == MovieStatus.HasTorrent)
							changeAllowed = true;
						break;
					case MovieStatus.Finished:
						if(movie.IdStatus == MovieStatus.HasTorrent || movie.IdStatus == MovieStatus.Downloaded)
							changeAllowed = true;
						break;
					case MovieStatus.HasTorrent:
						if(movie.IdStatus == MovieStatus.Scanned)
							changeAllowed = true;
						break;
					case MovieStatus.Scanned:
						if(movie.IdStatus == MovieStatus.NotScanned)
							changeAllowed = true;
						break;
					default:
						changeAllowed = true;
						break;
				}

				if(changeAllowed)
					movie.IdStatus = newStatus;
			}

			return movie;
		}

		public Movie FindMovieById(int idMovie)
		{
			return _movieRepository.FindById(idMovie);
		}

		public Movie FindMovieByNumber(string movieNbr)
		{
			return _movieRepository.FindByNumber(movieNbr);
		}

		public List<Movie> LoadMoviesToScrapeMagnet()
		{
			return _movieRepository.LoadMoviesToScrapeMagnet();
		}

		public List<Movie> LoadMoviesToDownload()
		{
			return _movieRepository.LoadMoviesToDownload();
		}

		public List<Movie> FindMovieByCriteria(DateTime? dtReleaseMin = null, DateTime? dtReleaseMax = null)
		{
			return _movieRepository.FindByCriteria(dtReleaseMin, dtReleaseMax);
		}
	}
}
