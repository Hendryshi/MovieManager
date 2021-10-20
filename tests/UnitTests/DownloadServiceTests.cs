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
	public class DownloadServiceTests
	{
		private readonly ITestOutputHelper _output;
		private DownloadService _downloadService;
		
		public DownloadServiceTests(ITestOutputHelper output)
		{
			_output = output;
			_downloadService = new DownloadService();
		}

		[Fact]
		public void test()
		{
			_output.WriteLine("test start");
			_downloadService.TestAsync();
			_output.WriteLine("test end");
		}
	}
}
