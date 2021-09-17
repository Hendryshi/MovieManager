using System;
using Xunit;
using Moq;
using UnitTests.Builders;
using MovieManager.Core.Interfaces;
using MovieManager.Core.Services;
using MovieManager.Core.Settings;
using MovieManager.Infrastructure.Services;
using Microsoft.Extensions.Options;

namespace UnitTests
{
	public class JavScrapeServiceTests
	{
		private readonly IOptions<JavlibSettings> _javlibSettings;
		private readonly IOptions<CommonSettings> _commonSettings;
		private readonly HtmlService _htmlService;
		private readonly JavScrapeService _javScrapeService;

		public JavScrapeServiceTests()
		{
			_javlibSettings = new JavlibSettingBuilder().Build();
			_commonSettings = new CommonSettingBuilder().Build();
			_htmlService = new HtmlService(null, _commonSettings);
			_javScrapeService = new JavScrapeService(null, _javlibSettings, null, _htmlService);
		}

		[Fact]
		public void TestGetPageCount()	
		{
			int pageCount = _javScrapeService.GetPageCount(_javlibSettings.Value.NewReleaseUrl);
			Assert.Equal(25, pageCount);
		}

	}
}
