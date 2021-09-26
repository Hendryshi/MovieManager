using MovieManager.Core.Entities;
using System.Collections.Generic;

namespace MovieManager.Core.Interfaces
{
	public interface IJavScrapeService
	{
		int GetPageCount(UrlInfo urlInfo);
		List<Movie> ScanPageList(UrlInfo urlInfo);
		void ScrapeNewReleasedMovie();
	}
}