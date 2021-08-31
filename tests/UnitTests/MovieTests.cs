using System;
using Xunit;
using Moq;
using UnitTests.Builders;
using MovieManager.Core.Interfaces;
using MovieManager.Core.Services;
using MovieManager.Core.Entities;
using MovieManager.Infrastructure.DbContext;
using MovieManager.Infrastructure.Repositories;

namespace UnitTests
{
	public class MovieTests
	{
		private DapperContext _dbContext;
		
		public MovieTests()
		{
			_dbContext = new DapperContext(new ConfigBuilder().Build());
		}

		[Fact]
		public void TestInsertMovie()	
		{
			var movie = new Movie() { Title = "CAWD-123", Category = "Cat1, Cat2" };
			var movieRepo = new MovieRepository(_dbContext);
			var movieInserted = movieRepo.Save(movie);
			Assert.True(movieInserted.IdMovie > 0);
		}

	}
}
