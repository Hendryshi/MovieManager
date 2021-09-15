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
			var movie = new Movie() { Number = "CAWD-123", Category = "Cat1, Cat2" };
			var movieRepo = new MovieRepository(_dbContext);
			var movieInserted = movieRepo.Save(movie);
			Assert.True(movieInserted.IdMovie > 0);
		}

		[Fact]
		public void TesFindMovieById()
		{
			int idMovie = 7;
			var movieRepo = new MovieRepository(_dbContext);
			var movie = movieRepo.FindById(idMovie);
			Assert.Equal("CAWD-123", movie.Number);

			idMovie = 0;
			movie = movieRepo.FindById(idMovie);
			Assert.True(movie == null);
		}

		[Fact]
		public void TesFindMovieByNbr()
		{
			string movieNbr = "cawd-123";
			var movieRepo = new MovieRepository(_dbContext);
			var movie = movieRepo.FindByNumber(movieNbr);
			Assert.Equal("CAWD-123", movie.Number);

			movieNbr = "notfind";
			movie = movieRepo.FindByNumber(movieNbr);
			Assert.True(movie == null);
		}

		[Fact]
		public void TestUpdateMovie()
		{
			string movieNbr = "cawd-123";
			var movieRepo = new MovieRepository(_dbContext);
			var movie = movieRepo.FindByNumber(movieNbr);

			movie.Title = "title modified";
			movieRepo.Save(movie);
			
			var movieModified = movieRepo.FindByNumber(movieNbr);
			Assert.Equal("title modified", movie.Title);
		}
	}
}
