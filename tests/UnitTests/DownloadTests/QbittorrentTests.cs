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
using MovieManager.Infrastructure.Services;
using MovieManager.Core.Settings;
using Microsoft.Extensions.Options;
using MovieManager.Infrastructure.Logging;
using System.Threading.Tasks;

namespace UnitTests
{
	public class QbittorrentTests
	{
		private readonly ITestOutputHelper _output;
		private readonly LoggerAdapter<QbittorrentService> _logger;
		private QbittorrentService _qbittorrentService;
		private readonly IOptionsSnapshot<QbittorrentSettings> _qbittorrentSettings;

		public QbittorrentTests(ITestOutputHelper output)
		{
			_output = output;
			_logger = new LoggerBuilder<QbittorrentService>().Build();
			_qbittorrentSettings = new qbittorrentSettingBuilder().Build();
			_qbittorrentService = new QbittorrentService(_logger, _qbittorrentSettings);
		}

		[Fact]
		public void test()
		{
			_output.WriteLine("enter");
			_qbittorrentService.EnsureLoggedInAsync().Wait();
			_output.WriteLine("exit");
		}

		[Fact]
		public void TestDeleteTorrent()
		{
			string hash = "0422922C697158878253C2BDF87AB057652035F1";
			_qbittorrentService.DeleteTorrentAsync(hash).Wait();
		}

		[Fact]
		public void TestAddTorrent()
		{
			List<Task> taskList = new List<Task>();
			MovieMagnet magnet = new MovieMagnet() { IdMovieMag = 247,
				IdMovie = 157, MagName = "mide-978-C", MovieNumber = "MIDE-978", Size = 5376, IsHD = true, HasSub = true, IdMagSource = MagnetSource.Javbus, IdStatus = MagnetStatus.IsReady,
				MagnetUrl = "magnet:?xt=urn:btih:D731203D3B903F675F96CD1D4FDC547A6428EB80&dn=mide-978-C", Hash = "D731203D3B903F675F96CD1D4FDC547A6428EB80" };

			var _movieMagnetService = new MovieMagServiceBuilder().Build();

			_output.WriteLine("task created");
			taskList.Add(_qbittorrentService.AddTorrentAsync(magnet).ContinueWith(t => _movieMagnetService.SaveMovieMagnet(magnet)));
			
			_output.WriteLine("waiting task to over: {0}", taskList.Count());
			Task.WaitAll(taskList.ToArray());
		}

		[Fact]
		public void TestGetTorrentCanAddCount()
		{
			int expectedValue = 2;
			int realValue = _qbittorrentService.GetTorrentCanAddCount();
			Assert.Equal(expectedValue, realValue);
		}

		[Fact]
		public void testSetPriority()
		{
			_qbittorrentService.GetTorrentContent().Wait();
		}

		[Fact]
		public void TestGetTorrentInfoFromRemoteUrl()
		{
			var mockQbittorrentSetting = new Mock<IOptionsSnapshot<QbittorrentSettings>>();
			QbittorrentSettings setting = new QbittorrentSettings()
			{
				WebUrl = "http://192.168.1.22:8080/",
				Username = "admin",
				Password = "adminadmin",
			};
			mockQbittorrentSetting.Setup(ap => ap.Value).Returns(setting);
			var qbittorrentService = new QbittorrentService(_logger, mockQbittorrentSetting.Object);
			var hash = "7e8ec6418a9cc070de668a9e13649a97d10e5611";
			var torrentInfo = qbittorrentService.GetTorrentInfo(hash);
		}
	}
}
