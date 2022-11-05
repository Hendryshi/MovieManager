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
	public class ReportSettingBuilder
	{
		private Mock<IOptionsSnapshot<ReportSettings>> _mock;

		public ReportSettingBuilder()
		{
			_mock = new Mock<IOptionsSnapshot<ReportSettings>>();
			ReportSettings setting = new ReportSettings()
			{
				ReportEmail = "yejia.shi@hotmail.com",
				ScrapeReportSubject = "Daily JAV Scrape Report"
			};

			_mock.Setup(ap => ap.Value).Returns(setting);
		}

		public IOptionsSnapshot<ReportSettings> Build()
		{
			return _mock.Object;
		}
	}
}
