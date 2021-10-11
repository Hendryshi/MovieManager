using System;
using Xunit;
using Moq;
using UnitTests.Builders;
using MovieManager.Core.Interfaces;
using MovieManager.Core.Services;
using MovieManager.Core.Entities;
using MovieManager.Core.Enumerations;
using MovieManager.Infrastructure.DbContext;
using MovieManager.Infrastructure.Repositories;
using Xunit.Abstractions;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests
{
	public class MovieTests
	{
		private readonly ITestOutputHelper _output;
		private MovieService _movieService;
		
		public MovieTests(ITestOutputHelper output)
		{
			_output = output;
			_movieService = new MovieServiceBuilder().Build();
		}

		[Fact]
		public void TestInsertMovie()	
		{
			var movie = new Movie() { Number = "CAWD-1236", Category = "Cat1, Cat2" };
			movie.MovieRelations =  new List<MovieRelation>() { 
				new MovieRelation() { IdTyRole = JavlibRoleType.Director, IdRelation = 300 }, 
				new MovieRelation() { IdTyRole = JavlibRoleType.Company, IdRelation = 300 } };

			var movieInserted = _movieService.SaveMovie(movie);
			Assert.True(movieInserted.IdMovie > 0);
		}

		[Fact]
		public void TesFindMovieById()
		{
			int idMovie = 7;
			var movie = _movieService.FindMovieById(idMovie);
			Assert.Equal("CAWD-123", movie.Number);
			Assert.Equal(4, movie.MovieRelations.Count);

			idMovie = 0;
			movie = _movieService.FindMovieById(idMovie);
			Assert.True(movie == null);
		}

		[Fact]
		public void TesFindMovieByNbr()
		{
			string movieNbr = "cawd-123";
			var movie = _movieService.FindMovieByNumber(movieNbr);
			Assert.Equal("CAWD-123", movie.Number);

			movieNbr = "notfind";
			movie = _movieService.FindMovieByNumber(movieNbr);
			Assert.True(movie == null);
		}

		[Fact]
		public void TestUpdateMovie()
		{
			int idMovie = 1488;
			var movie = _movieService.FindMovieById(idMovie);

			_output.WriteLine("Modified before: ");
			movie.MovieRelations.ForEach(mr => _output.WriteLine(mr.ToString()));

			movie.Title = "title modified";
			movie.MovieRelations.Add(new MovieRelation() { IdMovie = idMovie, IdTyRole = JavlibRoleType.Director, IdRelation = 500 });
			movie.MovieRelations.RemoveAt(1);

			_movieService.SaveMovie(movie);
			
			var movieModified = _movieService.FindMovieById(idMovie);
			Assert.Equal("title modified", movie.Title);

			_output.WriteLine("Modified after: ");
			movieModified.MovieRelations.ForEach(mr => _output.WriteLine(mr.ToString()));
		}

		[Fact]
		public void TestLoadMovieToDownloadMagnet()
		{
			List<Movie> lstMovies = _movieService.LoadMovieToDownloadMag();
			Assert.True(lstMovies.Count > 0);
			lstMovies.ForEach(m => _output.WriteLine(m.Number));
		}

		[Fact]
		public void TestStringJoin()
		{
			List<MovieRelation> movieRelations = new List<MovieRelation>() { new MovieRelation() { IdMovie = 1, IdTyRole = JavlibRoleType.Director, IdRelation = 300 }, new MovieRelation() { IdMovie = 3, IdTyRole = JavlibRoleType.Director, IdRelation = 300 } };
			_output.WriteLine(string.Join(",", movieRelations.Select(mr => $"({mr.IdMovie}, {mr.IdTyRole}, {mr.IdRelation})")));
		}
	}
}
