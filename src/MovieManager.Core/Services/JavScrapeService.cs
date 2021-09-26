using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using HtmlAgilityPack;
using Microsoft.Extensions.Options;
using MovieManager.Core.Interfaces;
using MovieManager.Core.Settings;
using MovieManager.Core.Enumerations;
using MovieManager.Core.Entities;
using MovieManager.Core.Extensions;

namespace MovieManager.Core.Services
{
	public class JavScrapeService : IJavScrapeService
	{
		private readonly JavlibSettings _javlibSettings;
		private readonly IAppLogger<JavScrapeService> _logger;
		private readonly IMovieService _movieService;
		private readonly IHtmlService _htmlService;

		public JavScrapeService(IAppLogger<JavScrapeService> logger, IOptions<JavlibSettings> javlibSettings,
			IMovieService movieService, IHtmlService htmlService)
		{
			_logger = logger;
			_javlibSettings = javlibSettings.Value;
			_movieService = movieService;
			_htmlService = htmlService;
		}

		//TODO: Create a new object Report and insert it into DB
		public void ScrapeNewReleasedMovie()
		{
			UrlInfo urlInfo = new UrlInfo() { EntryType = JavlibEntryType.NewRelease };
			int pageCount = GetPageCount(urlInfo);

			if(pageCount > 0)
			{
				_logger?.LogInformation($"Found {pageCount} pages. Now scanning movie on each page");
				for(int currentPage = 1; currentPage <= pageCount; currentPage++)
				{
					List<Movie> lstMovieCurrentPage = ScanPageList(new UrlInfo { EntryType = JavlibEntryType.NewRelease, Page = currentPage }).GroupBy(x => x.Url).Select(x => x.First()).ToList();
					if(lstMovieCurrentPage.Count > 0)
					{
						_logger?.LogInformation("Treating {pageCount} movies in page {currentPage}", lstMovieCurrentPage.Count, currentPage);
						foreach(Movie movie in lstMovieCurrentPage)
						{
							if(_movieService.FindMovieByNumber(movie.Number) == null)
							{
								ScanMovieDetails(new UrlInfo() { EntryType = JavlibEntryType.Movie, ExactUrl = movie.Url }, movie);
								_movieService.SaveMovie(movie);
							}
						}
					}
				}
			}
			else
				_logger?.LogWarning("Nothing found when scraping new released movie. UrlInfo: {0}", urlInfo);
		}

		public void ScanMovieDetails(UrlInfo urlInfo, Movie movie)
		{
			string requestUrl = GetJavLibraryUrl(urlInfo);
			HtmlDocument htmlDocument = TryGetHtmlDocument(requestUrl);
			if(htmlDocument != null)
			{
				var titlePath = "//h3[@class='post-title text']";
				var picPath = "//img[@id='video_jacket_img']";

				var dtReleasePath = "//div[@id='video_date']//td[@class='text']";
				var lengthPath = "//div[@id='video_length']//span[@class='text']";

				var dirPath = "//span[@class='director']//a";
				var comPath = "//span[@class='maker']//a";
				var pubPath = "//span[@class='label']//a";

				var catPath = "//span[@class='genre']//a";
				var starPath = "//span[@class='star']//a";

				var titleNode = htmlDocument.DocumentNode.SelectSingleNode(titlePath);
				var title = titleNode.InnerText.Trim();
				var number = title.Substring(0, title.IndexOf(" "));
				title = title.Substring(title.IndexOf(" ") + 1).ReplaceInvalidChar();
				var picUrl = htmlDocument.DocumentNode.SelectSingleNode(picPath);

				movie.PictureUrl = picUrl.Attributes["src"].Value;
				movie.PictureUrl = movie.PictureUrl.StartsWith("http") ? movie.PictureUrl : "http:" + movie.PictureUrl;

				if(movie.Title == null)
					movie.Title = title;

				if(movie.Number == null)
					movie.Number = number;

				var release = htmlDocument.DocumentNode.SelectSingleNode(dtReleasePath);
				DateTime rDate = DateTime.MinValue;
				if(release != null && !string.IsNullOrEmpty(release.InnerText))
				{
					if(DateTime.TryParse(release.InnerText.Trim(), out rDate))
						movie.DtRelease = rDate;
				}

				var duration = htmlDocument.DocumentNode.SelectSingleNode(lengthPath);
				if(duration != null && !string.IsNullOrEmpty(duration.InnerText))
				{
					movie.Duration = int.Parse(duration.InnerText.Trim());
				}

				//var dirNode = htmlDocument.DocumentNode.SelectNodes(dirPath);
				//if(dirNode != null)
				//{
				//	foreach(var dir in dirNode)
				//	{
				//		var name = dir.InnerHtml.Trim();
				//		var url = dir.Attributes["href"].Value;
				//		movie.Director += name + ",";

				//		Director d = LoadDirector(name, url);
				//		if(d == null)
				//		{
				//			Log.Information($"Found new Director: {name}");
				//			d = new Director() { Name = name, Url = url, DtUpdate = DateTime.Now };
				//			SaveDirector(d);
				//		}
				//		movie.MovieRelation.Add(new MovieRelation() { IdRelation = d.IdDirector, IdTyRole = (int)RoleType.Director });
				//	}
				//	movie.Director = movie.Director.Trim(',');
				//}

				//var comNode = htmlDocument.DocumentNode.SelectNodes(comPath);
				//if(comNode != null)
				//{
				//	foreach(var com in comNode)
				//	{
				//		var name = com.InnerHtml.Trim();
				//		var url = com.Attributes["href"].Value;
				//		movie.Company += name + ",";

				//		Company d = LoadCompany(name, url);
				//		if(d == null)
				//		{
				//			Log.Information($"Found new Company: {name}; Insert it into DB");
				//			d = new Company() { Name = name, Url = url, DtUpdate = DateTime.Now };
				//			SaveCompany(d);
				//		}
				//		movie.MovieRelation.Add(new MovieRelation() { IdRelation = d.IdCompany, IdTyRole = (int)RoleType.Company });
				//	}
				//	movie.Company = movie.Company.Trim(',');
				//}

				//var pubNode = htmlDocument.DocumentNode.SelectNodes(pubPath);
				//if(pubNode != null)
				//{
				//	foreach(var pub in pubNode)
				//	{
				//		var name = pub.InnerHtml.Trim();
				//		var url = pub.Attributes["href"].Value;
				//		movie.Publisher += name + ",";

				//		Publisher d = LoadPublisher(name, url);
				//		if(d == null)
				//		{
				//			Log.Information($"Found new Publisher: {name}; Insert it into DB");
				//			d = new Publisher() { Name = name, Url = url, DtUpdate = DateTime.Now };
				//			SavePublisher(d);
				//		}
				//		movie.MovieRelation.Add(new MovieRelation() { IdRelation = d.IdPublisher, IdTyRole = (int)RoleType.Publisher });
				//	}
				//	movie.Publisher = movie.Publisher.Trim(',');
				//}

				//var catNodes = htmlDocument.DocumentNode.SelectNodes(catPath);
				//if(catNodes != null)
				//{
				//	foreach(var cat in catNodes)
				//	{
				//		var name = cat.InnerHtml.Trim();
				//		var url = cat.Attributes["href"].Value;
				//		movie.Category += name + ",";

				//		Category d = LoadCategory(name, url);
				//		if(d == null)
				//		{
				//			Log.Information($"Found new Category: {name}; Insert it into DB");
				//			d = new Category() { Name = name, Url = url, DtUpdate = DateTime.Now };
				//			SaveCategory(d);
				//		}
				//		movie.MovieRelation.Add(new MovieRelation() { IdRelation = d.IdCategory, IdTyRole = (int)RoleType.Category });
				//	}
				//	movie.Category = movie.Category.Trim(',');
				//}

				//var starNodes = htmlDocument.DocumentNode.SelectNodes(starPath);
				//if(starNodes != null)
				//{
				//	foreach(var star in starNodes)
				//	{
				//		var name = star.InnerHtml.Trim();
				//		var url = star.Attributes["href"].Value;
				//		movie.Star += name + ",";

				//		Star d = LoadStar(name, url);
				//		if(d == null)
				//		{
				//			Log.Information($"Found new Star: {name}; Insert it into DB");
				//			d = new Star() { Name = name, Url = url, DtUpdate = DateTime.Now };
				//			SaveStar(d);
				//		}
				//		movie.MovieRelation.Add(new MovieRelation() { IdRelation = d.idStar, IdTyRole = (int)RoleType.Star });
				//	}
				//	movie.Star = movie.Star.Trim(',');
				//}
			}
			//Update Movie status to scanned
		}

		public int GetPageCount(UrlInfo urlInfo)
		{
			int lastPage = 0;
			string requestUrl = GetJavLibraryUrl(urlInfo);
			HtmlDocument htmlDocument = TryGetHtmlDocument(requestUrl);
			if(htmlDocument != null)
			{
				var lastPagePath = "//a[@class='page last']";
				var videoPath = "//div[@class='video']";

				var lastPageNode = htmlDocument.DocumentNode.SelectSingleNode(lastPagePath);

				if(lastPageNode != null)
				{
					var pageStr = lastPageNode.Attributes["href"].Value.Trim();

					if(!string.IsNullOrEmpty(pageStr))
					{
						pageStr = pageStr.Substring(pageStr.LastIndexOf("=") + 1);
						int.TryParse(pageStr, out lastPage);
					}
				}
				else
				{
					var videoNodes = htmlDocument.DocumentNode.SelectNodes(videoPath);
					if(videoNodes != null && videoNodes.Count > 0)
						lastPage = 1;
				}
			}

			return lastPage;
		}

		public List<Movie> ScanPageList(UrlInfo urlInfo)
		{
			List<Movie> lstMovie = new List<Movie>();
			string pageUrl = GetJavLibraryUrl(urlInfo);
			try
			{
				HtmlDocument htmlDocument = TryGetHtmlDocument(pageUrl);

				if(htmlDocument != null)
				{
					var videoPath = "//div[@class='video']";
					var videoNodes = htmlDocument.DocumentNode.SelectNodes(videoPath);

					if(videoNodes != null)
					{
						foreach(var node in videoNodes)
						{
							var urlAndTitle = node.ChildNodes[0];
							if(urlAndTitle != null && urlAndTitle.ChildNodes.Count >= 3)
							{
								var number = urlAndTitle.ChildNodes[0].InnerText.Trim();
								var name = urlAndTitle.ChildNodes[2].InnerText.Trim().ReplaceInvalidChar();
								var avUrl = urlAndTitle.Attributes["href"].Value.Trim().Replace("./", "");

								if(!string.IsNullOrEmpty(avUrl) && !string.IsNullOrEmpty(name) && !string.IsNullOrWhiteSpace(number))
									lstMovie.Add(new Movie() { Number = number, Title = name, Url = avUrl });
								else
									_logger?.LogError($"Movie missing information important: id: [{number}] name [{name}] movieUrl [{avUrl}]");
							}
						}
					}
				}
			}
			catch(Exception ex)
			{
				_logger?.LogError(ex, $"Error occurred when scanning page {pageUrl}, some movies may not be scanned");
			}
			return lstMovie;
		}

		private string GetJavLibraryUrl(UrlInfo urlInfo)
		{
			string result = string.Empty;
			switch(urlInfo.EntryType)
			{
				case JavlibEntryType.NewRelease:
					string mode = urlInfo.Mode.HasValue ? urlInfo.Mode.Value.ToString() : "";
					string page = urlInfo.Page.HasValue ? urlInfo.Page.Value.ToString() : "1";
					result = _javlibSettings.NewReleaseUrl + "?&mode=" + mode + "&page=" + page;
					break;
				case JavlibEntryType.Movie:
					result = _javlibSettings.BaseAddress + urlInfo.ExactUrl;
					break;
				case JavlibEntryType.Other:
					result = urlInfo.ExactUrl;
					break;
			}

			return result;
		}

		private HtmlDocument TryGetHtmlDocument(string requestUrl)
		{
			int retry = 0;
			int maxRetry = 2;
			HtmlDocument html = null;

			var cookieContainer = new CookieContainer();
			var baseAddress = new Uri(_javlibSettings.BaseAddress);
			cookieContainer.Add(baseAddress, new Cookie("cf_clearance", _javlibSettings.Cloudflare));
			var headers = new Dictionary<string, string>() { { "Cookie", cookieContainer.GetCookieHeader(baseAddress) } };

			while(retry <= maxRetry && html == null)
			{
				try
				{
					html = _htmlService.GetHtmlDocumentAsync(requestUrl, headers).Result;
				}
				catch(Exception ex)
				{
					_logger?.LogError(ex, $"Error when requesting page: {requestUrl}");
					retry++;
					if(retry <= maxRetry)
						_logger?.LogInformation($"Retrying {retry}/{maxRetry} times");
				}
			}

			return html;
		}
	}
}
