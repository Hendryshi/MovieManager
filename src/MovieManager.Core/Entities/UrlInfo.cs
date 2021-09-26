using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieManager.Core.Enumerations;

namespace MovieManager.Core.Entities
{
	public class UrlInfo
	{
		public JavlibEntryType EntryType { get; set; }
		public int? Page { get; set; }
		public int? Mode { get; set; }
		public string ExactUrl { get; set; }

	}
}
