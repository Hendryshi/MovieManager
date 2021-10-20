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

		public Movie SaveMovie(Movie movie)
		{
			return _movieRepository.Save(movie);
		}

		public Movie FindMovieById(int idMovie)
		{
			return _movieRepository.FindById(idMovie);
		}

		public Movie FindMovieByNumber(string movieNbr)
		{
			return _movieRepository.FindByNumber(movieNbr);
		}

		public List<Movie> LoadMovieToDownloadMag()
		{
			return _movieRepository.LoadMovieToDownloadMag();
		}
	}
}
