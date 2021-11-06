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
	class JavlibSettingBuilder
	{
		private Mock<IOptionsSnapshot<JavlibSettings>> _mockJavlibSetting;

		public JavlibSettingBuilder()
		{
			_mockJavlibSetting = new Mock<IOptionsSnapshot<JavlibSettings>>();
			JavlibSettings setting = new JavlibSettings()
			{
				BaseAddress = "http://www.javlibrary.com/cn/",
				Cloudflare = "fjZrsz79hQ21c6ruXKHZq4EmLa3QeQpWUAx11D5teHY-1633465963-0-250",
				NewReleaseUrl = "http://www.javlibrary.com/cn/vl_newrelease.php",
				DownloadTorrentPoint = 500,
				DownloadMoviePoint = 1000,
				DownloadSubPoint = 1200
			};

			_mockJavlibSetting.Setup(ap => ap.Value).Returns(setting);
		}

		public IOptionsSnapshot<JavlibSettings> Build()
		{
			return _mockJavlibSetting.Object;
		}
	}
}
