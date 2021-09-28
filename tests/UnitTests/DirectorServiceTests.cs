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
	public class DirectorServiceTest
	{
		private readonly ITestOutputHelper _output;
		private DirectorService _directorService;
		
		public DirectorServiceTest(ITestOutputHelper output)
		{
			_output = output;
			_directorService = new DirectorServiceBuilder().Build();
		}

		[Fact]
		public void TestInsertDirector()	
		{
			var director = new Director() { Name = "DirectorTest", Description = "description", FavLevel = JavlibFavLevel.DlMovie, Url = "google.fr" };
			var directorInserted = _directorService.SaveDirector(director);
			Assert.True(directorInserted.IdDirector > 0);
		}

		[Fact]
		public void TesFindDirectorById()
		{
			int idDirector = 1;
			var director = _directorService.FindDirectorById(idDirector);
			Assert.Equal("DirectorTest", director.Name);

			idDirector = 0;
			director = _directorService.FindDirectorById(idDirector);
			Assert.True(director == null);
		}

		[Fact]
		public void TesFindDirectorByName()
		{
			string name = "DirectorTest";
			var director = _directorService.FindDirectorByName(name);
			Assert.Equal(name, director.Name);

			string url = "urlfalse";
			director = _directorService.FindDirectorByName(name, url);
			Assert.True(director == null);
		}

		[Fact]
		public void TestUpdateDirector()
		{
			int idDirector = 1;
			var director = _directorService.FindDirectorById(idDirector);
			string expectedValue = director.Url += "r";
			
			director.Url = expectedValue;
			_directorService.SaveDirector(director);

			var directorModified = _directorService.FindDirectorById(idDirector);
			Assert.Equal(expectedValue, director.Url);
		}

	}
}
