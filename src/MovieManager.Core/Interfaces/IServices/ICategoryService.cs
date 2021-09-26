using MovieManager.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieManager.Core.Interfaces
{
	public interface ICategoryService
	{
		Category FinCategoryByName(string name, string url = "");
		Category FindCategoryById(int idCategory);
		Category SaveCategory(Category category);
	}
}
