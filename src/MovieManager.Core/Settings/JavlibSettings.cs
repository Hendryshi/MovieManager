﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieManager.Core.Settings
{
	public class JavlibSettings
	{
		public string BaseAddress { get; set; }
		public string Cloudflare { get; set; }
		public string NewReleaseUrl { get; set; }
		public int DownloadTorrentPoint { get; set; }
		public int DownloadMoviePoint { get; set; }
		public int DownloadSubPoint { get; set; }
		public int UpdatePointMaxDay { get; set; }
	}
}
