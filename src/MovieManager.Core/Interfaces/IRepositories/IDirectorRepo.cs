using MovieManager.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieManager.Core.Interfaces
{
	public interface IDirectorRepo
	{
		Director FindById(int idDirector);
		Director FindByName(string name, string url = "");
		Director Save(Director director);
	}
}
