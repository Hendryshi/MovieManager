using System;
using Xunit;
using Moq;
using UnitTests.Builders;
using MovieManager.Core.Interfaces;
using MovieManager.Core.Services;
using MovieManager.Core.Entities;
using MovieManager.Infrastructure.DbContext;
using MovieManager.Infrastructure.Repositories;
using Xunit.Abstractions;
using System.Collections.Generic;
using System.Linq;
using MovieManager.Core.Enumerations;

namespace UnitTests
{
	public class CategoryServiceTest
	{
		private readonly ITestOutputHelper _output;
		private CategoryService _categoryService;
		
		public CategoryServiceTest(ITestOutputHelper output)
		{
			_output = output;
			_categoryService = new CategoryServiceBuilder().Build();
		}

		[Fact]
		public void TestInsertCategory()	
		{
			var category = new Category() { Name = "CategoryTest", Description = "description", FavLevel = JavlibFavLevel.DlMovie, Url = "google.fr" };
			var categoryInserted = _categoryService.SaveCategory(category);
			Assert.True(categoryInserted.IdCategory > 0);
		}

		[Fact]
		public void TesFindCategoryById()
		{
			int idCategory = 1;
			var category = _categoryService.FindCategoryById(idCategory);
			Assert.Equal("CategoryTest", category.Name);

			idCategory = 0;
			category = _categoryService.FindCategoryById(idCategory);
			Assert.True(category == null);
		}

		[Fact]
		public void TesFindCategoryByName()
		{
			string name = "CategoryTest";
			var category = _categoryService.FinCategoryByName(name);
			Assert.Equal(name, category.Name);

			string url = "urlfalse";
			category = _categoryService.FinCategoryByName(name, url);
			Assert.True(category == null);
		}

		[Fact]
		public void TestUpdateCategory()
		{
			int idCategory = 1;
			var category = _categoryService.FindCategoryById(idCategory);
			string expectedValue = category.Url += "r";
			
			category.Url = expectedValue;
			_categoryService.SaveCategory(category);

			var categoryModified = _categoryService.FindCategoryById(idCategory);
			Assert.Equal(expectedValue, category.Url);
		}

	}
}
