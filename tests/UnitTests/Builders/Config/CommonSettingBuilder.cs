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
	class CommonSettingBuilder
	{
		private Mock<IOptions<CommonSettings>> _mock;

		public CommonSettingBuilder()
		{
			_mock = new Mock<IOptions<CommonSettings>>();
			CommonSettings setting = new CommonSettings()
			{
				DefaultUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/94.0.4606.71 Safari/537.36"
			};

			_mock.Setup(ap => ap.Value).Returns(setting);
		}

		public IOptions<CommonSettings> Build()
		{
			return _mock.Object;
		}
	}
}
