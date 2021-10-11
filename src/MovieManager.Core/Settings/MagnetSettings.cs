using MovieManager.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieManager.Core.Settings
{
	public class MagnetSettings
	{
		public string SearchSources { get; set; }
		public int MaxSearchCount { get; set; }

		public List<MagnetSource> GetSearchSources()
		{
			List<MagnetSource> searchSources = new List<MagnetSource>();

			foreach(string source in SearchSources.Split(","))
			{
				if(Enum.TryParse(source, true, out MagnetSource magnetSource))
					searchSources.Add(magnetSource);
			}
			return searchSources;
		}
    }
}
