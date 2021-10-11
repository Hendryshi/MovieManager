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
	class MagnetSettingBuilder
	{
		private Mock<IOptions<MagnetSettings>> _mockMagnetSetting;

		public MagnetSettingBuilder()
		{
			_mockMagnetSetting = new Mock<IOptions<MagnetSettings>>();
			MagnetSettings setting = new MagnetSettings()
			{
				SearchSources = "Javbus, Sukebei",
				MaxSearchCount = 3
			};

			_mockMagnetSetting.Setup(ap => ap.Value).Returns(setting);
		}

		public IOptions<MagnetSettings> Build()
		{
			return _mockMagnetSetting.Object;
		}
	}
}
