using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MovieManager.Core.Interfaces;
using MovieManager.Core.Entities;

namespace MovieManager.Core.Services
{
	public class CategoryService : ICategoryService
	{
		private readonly ICategoryRepo _categoryRepo;
		private readonly IAppLogger<CategoryService> _logger;

		public CategoryService(IAppLogger<CategoryService> logger, ICategoryRepo categoryRepo)
		{
			_categoryRepo = categoryRepo;
			_logger = logger;
		}

		public Category SaveCategory(Category category)
		{
			return _categoryRepo.Save(category);
		}

		public Category FindCategoryById(int idCategory)
		{
			return _categoryRepo.FindById(idCategory);
		}

		public Category FinCategoryByName(string name, string url = "")
		{
			return _categoryRepo.FindByName(name, url);
		}

	}
}
