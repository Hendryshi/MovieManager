using MovieManager.Core.Entities;
using System.Collections.Generic;

namespace MovieManager.Core.Interfaces
{
	public interface IJavScrapeService
	{
		void DailyScrape();
		int GetPageCount(UrlInfo urlInfo);
		void ScanMovieDetails(UrlInfo urlInfo, Movie movie);
		List<Movie> ScanPageList(UrlInfo urlInfo);
		void ScrapeNewReleasedMovie();
		List<Movie> ScrapreMoviesByKeyWord(string keyword);
	}
}