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
	public class EmailSettingBuilder
	{
		private Mock<IOptionsSnapshot<EmailSettings>> _mock;

		public EmailSettingBuilder()
		{
			_mock = new Mock<IOptionsSnapshot<EmailSettings>>();
			EmailSettings setting = new EmailSettings()
			{
				FromEmail = "yejia.shi@hotmail.com",
				EnableSsl = true,
				MailServer = "smtp-mail.outlook.com",
				SmtpPort = 587,
				NetworkCredentialuserName = "yejia.shi@hotmail.com",
				NetworkCredentialpassword = "Paulshi28"
			};

			_mock.Setup(ap => ap.Value).Returns(setting);
		}

		public IOptionsSnapshot<EmailSettings> Build()
		{
			return _mock.Object;
		}
	}
}
