using MovieManager.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieManager.Core.Interfaces
{
	public interface IDirectorService
	{
		Director FindDirectorByName(string name, string url = "");
		Director FindDirectorById(int idDirector);
		Director SaveDirector(Director director);
	}
}
