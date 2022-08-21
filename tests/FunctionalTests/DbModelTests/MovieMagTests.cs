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

namespace FunctionalTests
{
	public class MovieMagTests
	{
		private readonly ITestOutputHelper _output;
		private MovieMagnetService _movieMagService;
		private MovieService movieService;
		
		public MovieMagTests(ITestOutputHelper output)
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
		public void TestFindMagnetByStatus()
		{
			var magnets = _movieMagService.FindMovieMagnetByStatus(MagnetStatus.Downloading);
			Assert.Empty(magnets);

			magnets = _movieMagService.FindMovieMagnetByStatus(MagnetStatus.IsReady);
			Assert.NotEmpty(magnets);
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

		[Fact]
		public void TestFindBestMatchMagnetByMovie()
		{
			Movie movie = new Movie() { IdMovie = 71, IdStatus = MovieStatus.Downloaded };
			var magnet = _movieMagService.FindBestMatchMagnetByMovie(movie);

			var expectedValue = 1506;
			Assert.Equal(expectedValue, magnet.IdMovieMag);
		}

		[Fact]
		public void TestAddMovieMagnetHistory()
		{
			var dbContext = new DapperContext(new ConfigBuilder().Build());
			var movieHistoryRepo = new MovieHistoryRepo(dbContext);
			var movieMagRepo = new MovieMagnetRepo(dbContext, movieHistoryRepo);

			MovieMagnet magnet = new MovieMagnet() { IdMovieMag = 9, IdMovie = 73, MovieNumber = "FSDSS-298", MagName = "magnamesf", Size = 5400, DtMagnet = new DateTime(2020, 1, 1) };

			List<MovieHistory> movieHistories = movieMagRepo.AddHistory(magnet);
			
			movieHistories.ForEach(h => _output.WriteLine(h.DescHistory));

		}

		[Fact]
		public void TestSaveListMagnet()
		{
			List<MovieMagnet> movieMagnets = new List<MovieMagnet>();
			MovieMagnet magnet = new MovieMagnet() { IdMovieMag = 9, IdMovie = 73, MovieNumber = "FSDSS-298", MagName = "magnadsdsdesf", MagnetUrl = "dsfdsfdsf", Hash = "dvsffvdsv", Size = 1234, IdMagSource = MagnetSource.Javbus
				, DtMagnet = new DateTime(2021, 1, 1) };
			MovieMagnet magnet2 = new MovieMagnet() { IdMovieMag = 25, IdMovie = 82, MovieNumber = "GVH-301", MagName = "sdfezfdsf", MagnetUrl = "dsfdsfdsfezfzef", Hash = "dvsvdsv", Size = 3154, IdMagSource = MagnetSource.Javbus
				, DtMagnet = new DateTime(2021, 1, 2) };

			movieMagnets.Add(magnet);
			movieMagnets.Add(magnet2);

			_movieMagService.SaveMovieMagnetList(movieMagnets);
		}

		[Fact]
		public void TestSaveListMagnetFailed()
		{
			List<MovieMagnet> movieMagnets = new List<MovieMagnet>();
			MovieMagnet magnet = new MovieMagnet()
			{
				IdMovieMag = 9,
				IdMovie = 73,
				MovieNumber = "FSDSS-298",
				MagName = "magnadsdsdesf",
				MagnetUrl = "dsfdsfdsf",
				Hash = "dvsffvdsv",
				Size = 1234,
				IdMagSource = MagnetSource.Sukebei,
				DtMagnet = new DateTime(2021, 1, 1)
			};
			MovieMagnet magnet2 = new MovieMagnet()
			{
				IdMovieMag = 25,
				IdMovie = 82,
				MovieNumber = "GVH-301",
				MagName = "sdfezfdsf",
				MagnetUrl = "dsfdsfdsfezfzef",
				Hash = "dvsffvdsv",
				Size = 3154,
				IdMagSource = MagnetSource.Javbus,
				DtMagnet = new DateTime(2021, 1, 2)
			};

			movieMagnets.Add(magnet);
			movieMagnets.Add(magnet2);

			_movieMagService.SaveMovieMagnetList(movieMagnets);
		}
	}
}
