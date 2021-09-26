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
				Cloudflare = "sGcwmjgbREW8Zg1EuuG3gV6H9hmbSQ0BKOvBvGkDqg0-1632418033-0-250",
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
