using MovieManager.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieManager.Core.Interfaces
{
	public interface IMovieHistoryRepo
	{
		List<MovieHistory> LoadByIdMovie(int idMovie);
		MovieHistory Save(int idMovie, MovieHistory history);
		List<MovieHistory> SaveList(int idMovie, List<MovieHistory> movieHistories);
	}
}
