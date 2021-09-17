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
		private Mock<IOptions<JavlibSettings>> _mockJavlibSetting;

		public JavlibSettingBuilder()
		{
			_mockJavlibSetting = new Mock<IOptions<JavlibSettings>>();
			JavlibSettings setting = new JavlibSettings()
			{
				BaseAddress = "http://www.javlibrary.com/cn/",
				Cloudflare = "f53f6a3c885015a0ea27d551536973b7ca687f0c-1626960859-0-150",
				NewReleaseUrl = "http://www.javlibrary.com/cn/vl_newrelease.php",
			};

			_mockJavlibSetting.Setup(ap => ap.Value).Returns(setting);
		}

		public IOptions<JavlibSettings> Build()
		{
			return _mockJavlibSetting.Object;
		}
	}
}
