using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieManager.Core.Settings;
using Moq;

namespace UnitTests.Builders
{
	public class qbittorrentSettingBuilder
	{
		private Mock<IOptionsSnapshot<QbittorrentSettings>> _mockQbittorrentSetting;

		public qbittorrentSettingBuilder()
		{
			_mockQbittorrentSetting = new Mock<IOptionsSnapshot<QbittorrentSettings>>();
			QbittorrentSettings setting = new QbittorrentSettings()
			{
				WebUrl = "http://127.0.0.1:6882/",
				Username = "admin",
				Password = "adminadmin",
				MaxDownloadCount = 3,
				DownloadRootPath = @"E:\TEST\JavDownload",
				Category = "jav"
			};

			_mockQbittorrentSetting.Setup(ap => ap.Value).Returns(setting);
		}

		public IOptionsSnapshot<QbittorrentSettings> Build()
		{
			return _mockQbittorrentSetting.Object;
		}
	}
}
