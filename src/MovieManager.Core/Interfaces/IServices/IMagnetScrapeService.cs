using MovieManager.Core.Entities;
using System.Collections.Generic;

namespace MovieManager.Core.Interfaces
{
	public interface IMagnetScrapeService
	{
		void DailyDownloadMovieMagnet();
		List<MovieMagnet> SearchMagnetFromSukebei(Movie movie);
	}
}