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
		Movie FindById(int idMovie);
		Movie FindByNumber(string movieNbr);
		Movie Save(Movie movie);
	}
}
