using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieManager.Core.Settings
{
	public class QbittorrentSettings
	{
        public string WebUrl { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }

		public int MaxDownloadCount { get; set; }
		public string DownloadRootPath { get; set; }
		public string Category { get; set; }
	}
}
