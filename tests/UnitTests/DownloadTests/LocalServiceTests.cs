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
using MovieManager.Infrastructure.Logging;
using MovieManager.Infrastructure.Services;
using System.IO;
using System.Threading.Tasks;

namespace UnitTests
{
	public class LocalServiceTests
	{
		private readonly ITestOutputHelper _output;
		private LocalFileService _localFileService;
		

		public LocalServiceTests(ITestOutputHelper output)
		{
			_output = output;
			_localFileService = new LocalFileServiceBuilder().Build();
		}

		[Fact]
		public void TestFindMovieFile()
		{
			string dir = @"E:\Media\Sister\Data\Finished\stars-314-C";
			string movieNumber = "Star-314";

			FileInfo result = _localFileService.FindMovieFile(dir, movieNumber);
			_output.WriteLine(result.Name);
		}

		[Fact]
		public async Task TestGenerateMovieFile()
		{
			MovieMagnet magnet = new MovieMagnet() { SavePath = @"E:\Media\Sister\Data\Finished\snis-842-C", HasSub = true, MovieNumber = "snis-842" };
			await _localFileService.GenerateMovieFile(magnet);
			Assert.Equal(MagnetStatus.Finished, magnet.IdStatus);
			_output.WriteLine(magnet.ToString());
		}

		[Fact]
		public async Task TestDeleteFolder()
		{
			string deletePath = @"E:\TEST\JavVideo\STARS-314";
			await _localFileService.DeleteFolder(deletePath);
		}
	}
}
