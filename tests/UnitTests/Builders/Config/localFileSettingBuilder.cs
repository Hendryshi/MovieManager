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
	class localFileSettingBuilder
	{
		private Mock<IOptionsSnapshot<LocalFileSettings>> _mockLocalFileSetting;

		public localFileSettingBuilder()
		{
			_mockLocalFileSetting = new Mock<IOptionsSnapshot<LocalFileSettings>>();
			LocalFileSettings setting = new LocalFileSettings()
			{
				DestSaveRootPath = @"E:\TEST\JavVideo",
				ArchivedDownloadPath = @"E:\TEST\JavDownload\Archived",
			};

			_mockLocalFileSetting.Setup(ap => ap.Value).Returns(setting);
		}

		public IOptionsSnapshot<LocalFileSettings> Build()
		{
			return _mockLocalFileSetting.Object;
		}
	}
}
