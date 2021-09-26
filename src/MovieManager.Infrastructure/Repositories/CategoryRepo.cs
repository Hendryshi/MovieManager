using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieManager.Core.Interfaces;
using MovieManager.Core.Entities;
using MovieManager.Infrastructure.DbContext;
using System.Transactions;

namespace MovieManager.Infrastructure.Repositories
{
	public class CategoryRepo : ICategoryRepo
	{
		private readonly DapperContext db;

		public CategoryRepo(DapperContext dbContext)
		{
			db = dbContext;
		}

		public Category Save(Category category)
		{
			category.DtUpdate = DateTime.Now;

			if(category.IdCategory == 0)
				category.IdCategory = (int)db.InsertEntity(category);
			else if(!db.UpdateEntity(category))
				throw new Exception($"Category not found in DB: {category.ToString()}");

			return category;
		}

		public Category FindById(int idCategory)
		{
			return db.GetEntityById<Category>(idCategory);
		}

		public Category FindByName(string name, string url = "")
		{
			var sql = new StringBuilder();
			sql.AppendLine(@"SELECT * FROM J_Category WHERE name = @name");

			if(!string.IsNullOrEmpty(url))
				sql.AppendLine("AND url = @url");

			return db.QuerySingleOrDefault<Category>(sql.ToString(), new { name = name, url = url });
		}

	}
}
