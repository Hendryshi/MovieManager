using MovieManager.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieManager.Core.Interfaces
{
	public interface ICategoryRepo
	{
		Category FindById(int idCategory);
		Category FindByName(string name, string url = "");
		Category Save(Category category);
	}
}
