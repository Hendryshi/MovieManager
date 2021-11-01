using MovieManager.Core.Entities;
using MovieManager.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieManager.Core.Interfaces
{
	public interface IMovieMagnetService
	{
		string LoadMagSourceUrl(MagnetSource javbus);
		MovieMagnet SaveMovieMagnet(MovieMagnet movieMagnet);
		MovieMagnet FindMovieMagnetById(int idMovieMag);
		MovieMagnet FindMovieMagnetByHash(string hash);
		List<MovieMagnet> FindMovieMagnetByMovie(Movie movie, bool onlyAlive = true);
		List<MovieMagnet> SaveMovieMagnetList(List<MovieMagnet> movieMagnets);
		List<MovieMagnet> FindMovieMagnetByStatus(MagnetStatus magnetStatus, int idMovie = 0);
		MovieMagnet FindBestMatchMagnetByMovie(Movie movie);
	}
}
