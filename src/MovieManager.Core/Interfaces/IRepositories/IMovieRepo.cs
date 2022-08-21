using MovieManager.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieManager.Core.Interfaces
{
	public interface IMovieRepo
	{
		List<Movie> FindByCriteria(DateTime? dtReleaseMin = null, DateTime? dtReleaseMax = null);
		Movie FindById(int idMovie);
		Movie FindByNumber(string movieNbr);
		List<Movie> LoadMoviesToDownload();
		List<Movie> LoadMoviesToScrapeMagnet();
		Movie Save(Movie movie, bool updateRelation);
	}
}
