using MovieManager.Core.Entities;
using MovieManager.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieManager.Core.Interfaces
{
	public interface IMovieService
	{
		Movie FindMovieById(int idMovie);
		Movie FindMovieByNumber(string movieNbr);
		List<Movie> LoadMovieToDownloadMag();
		Movie SaveMovie(Movie movie);
		void UpdateStatus(Movie movie, MovieStatus movieStatus);
	}
}
