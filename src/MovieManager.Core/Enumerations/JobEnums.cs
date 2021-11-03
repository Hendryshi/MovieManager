using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieManager.Core.Enumerations
{
	public enum HangfireJob : short
	{
		ScrapeNewReleasedMovie = 1,
		ScrapeMovieMagnet,
		MonitorMovieDownload
	}
}
