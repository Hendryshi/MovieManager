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
	class qbittorrentSettingBuilder
	{
		private Mock<IOptions<QbittorrentSettings>> _mockQbittorrentSetting;

		public qbittorrentSettingBuilder()
		{
			_mockQbittorrentSetting = new Mock<IOptions<QbittorrentSettings>>();
			QbittorrentSettings setting = new QbittorrentSettings()
			{
				WebUrl = "http://127.0.0.1:6882/",
				Username = "admin",
				Password = "adminadmin"
			};

			_mockQbittorrentSetting.Setup(ap => ap.Value).Returns(setting);
		}

		public IOptions<QbittorrentSettings> Build()
		{
			return _mockQbittorrentSetting.Object;
		}
	}
}
