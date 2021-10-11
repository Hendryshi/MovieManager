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
	public class MovieMagServiceTests
	{
		private readonly ITestOutputHelper _output;
		private MovieMagnetService _movieMagService;
		
		public MovieMagServiceTests(ITestOutputHelper output)
		{
			_output = output;
			_movieMagService = new MovieMagServiceBuilder().Build();
		}

		[Fact]
		public void TestInsertMagnet()	
		{
			var magnet = new MovieMagnet() { MovieNumber = "CAWD-321", IdMovie = 1492, MagName = "magname", MagnetUrl = "123456&dl", Hash = "123456", IdMagSource = MagnetSource.Sukebei };
			var magnetInserted = _movieMagService.SaveMovieMagnet(magnet);
			Assert.True(magnetInserted.IdMagSource > 0);
		}

		[Fact]
		public void TesFindMagnetById()
		{
			int idMovieMag = 9;
			var movieMag = _movieMagService.FindMovieMagnetById(idMovieMag);
			Assert.Equal("miaa-506", movieMag.MagName);
			_output.WriteLine(movieMag.Hash);

			idMovieMag = 0;
			movieMag = _movieMagService.FindMovieMagnetById(idMovieMag);
			Assert.True(movieMag == null);
		}

		[Fact]
		public void TestFindMagnetByHash()
		{
			string hash = "123456";
			var magnet = _movieMagService.FindMovieMagnetByHash(hash);
			Assert.Equal("magname", magnet.MagName);

			hash = "urlfalse";
			magnet = _movieMagService.FindMovieMagnetByHash(hash);
			Assert.True(magnet == null);
		}

		[Fact]
		public void TestFindMagnetByMovie()
		{
			Movie movie = new Movie() { IdMovie = 1492 };
			var magnets = _movieMagService.FindMovieMagnetByMovie(movie, false);
			Assert.Single(magnets);
		}

		[Fact]
		public void TestUpdateMagnet()
		{
			int idMovieMag = 4;
			var movieMag = _movieMagService.FindMovieMagnetById(idMovieMag);
			string expectedValue = movieMag.MagnetUrl += "r";

			movieMag.MagnetUrl = expectedValue;
			_movieMagService.SaveMovieMagnet(movieMag);

			var movieMagModified = _movieMagService.FindMovieMagnetById(idMovieMag);
			Assert.Equal(expectedValue, movieMagModified.MagnetUrl);
		}

		[Fact]
		public void TestLoadMagnetSource()
		{
			var expectedValue = "https://www.javbus.com/cawd-123";
			Assert.Equal(expectedValue, string.Format(_movieMagService.LoadMagSourceUrl(MagnetSource.Javbus), "cawd-123"));
		}

	}
}
