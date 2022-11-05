using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieManager.Core.Settings
{
	public class EmailSettings
	{
		public string FromEmail { get; set; }
        public bool EnableSsl { get; set; }
        public string MailServer { get; set; }
        public string NetworkCredentialuserName { get; set; }
        public string NetworkCredentialpassword { get; set; }
        public int SmtpPort { get; set; }

    }
}
