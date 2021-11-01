using MovieManager.Core.Entities;
using MovieManager.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieManager.Core.Interfaces
{
	public interface IMovieMagnetRepo
	{
		MovieMagnet FindByHash(string hash);
		MovieMagnet FindById(int idMovieMag);
		List<MovieMagnet> FindByMovie(Movie movie, bool onlyAlive);
		MovieMagnet Save(MovieMagnet movieMagnet);
		string LoadMagnetSource(MagnetSource magnetSource);
		List<MovieMagnet> SaveList(List<MovieMagnet> movieMagnets);
		List<MovieMagnet> FindByStatus(MagnetStatus magnetStatus, int idMovie = 0);
		MovieMagnet FindByBestMatch(int idMovie, bool mustHasHD = false, bool mustHasSub = false);
	}
}
