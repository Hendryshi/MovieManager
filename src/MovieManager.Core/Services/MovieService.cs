using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MovieManager.Core.Interfaces;
using MovieManager.Core.Entities;

namespace MovieManager.Core.Services
{
	public class MovieService : IMovieService
	{
		private readonly IMovieRepository _movieRepository;
		private readonly IAppLogger<MovieService> _logger;

		public MovieService(IAppLogger<MovieService> logger, IMovieRepository movieRepository)
		{
			_movieRepository = movieRepository;
			_logger = logger;
		}

		public Movie SaveMovie(Movie movie)
		{
			return _movieRepository.Save(movie);
		}

	}
}
