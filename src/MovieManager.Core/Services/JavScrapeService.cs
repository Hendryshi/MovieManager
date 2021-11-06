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
using System.Globalization;

namespace MovieManager.Core.Services
{
	public class JavScrapeService : IJavScrapeService
	{
		private readonly JavlibSettings _javlibSettings;
		private readonly IAppLogger<JavScrapeService> _logger;
		private readonly IMovieService _movieService;
		private readonly IHtmlService _htmlService;
		private readonly IActorService _actorService;
		private readonly ICategoryService _categoryService;
		private readonly ICompanyService _companyService;
		private readonly IDirectorService _directorService;
		private readonly IPublisherService _publisherService;

		public JavScrapeService(IAppLogger<JavScrapeService> logger, IOptionsSnapshot<JavlibSettings> javlibSettings,
			IMovieService movieService, 
			IActorService actorService,
			ICategoryService categoryService,
			ICompanyService companyService,
			IDirectorService directorService,
			IPublisherService publisherService,
			IHtmlService htmlService)
		{
			_logger = logger;
			_javlibSettings = javlibSettings.Value;
			_movieService = movieService;
			_actorService = actorService;
			_categoryService = categoryService;
			_companyService = companyService;
			_directorService = directorService;
			_publisherService = publisherService;
			_htmlService = htmlService;
		}

		//TODO: Create a new object Report and insert it into DB
		public void ScrapeNewReleasedMovie()
		{
			_logger?.LogInformation("************** Start Scraping New Released Movies Job - {Date} **************", DateTime.Now.ToString("u", DateTimeFormatInfo.InvariantInfo));
			UrlInfo urlInfo = new UrlInfo() { EntryType = JavlibEntryType.NewRelease };
			int pageCount = GetPageCount(urlInfo);

			if(pageCount > 0)
			{
				_logger?.LogInformation($"Found {pageCount} pages. Now scanning movies on each page");
				for(int currentPage = 1; currentPage <= pageCount; currentPage++)
				{
					List<Movie> lstMovieCurrentPage = ScanPageList(new UrlInfo { EntryType = JavlibEntryType.NewRelease, Page = currentPage }).GroupBy(x => x.Url).Select(x => x.First()).ToList();
					if(lstMovieCurrentPage.Count > 0)
					{
						_logger?.LogInformation("Treating {pageCount} movies in page {currentPage}", lstMovieCurrentPage.Count, currentPage);
						foreach(Movie movie in lstMovieCurrentPage.GroupBy(x => x.Number.ToLower()).Select(x => x.First()))
						{
							ScanMovieDetails(new UrlInfo() { EntryType = JavlibEntryType.Movie, ExactUrl = movie.Url }, movie);
							_movieService.UpdateStatus(movie, MovieStatus.Scanned);
							_movieService.SaveMovie(movie);
						}
					}
				}
			}
			else
				_logger?.LogWarning("Nothing found when scraping new released movie. UrlInfo: {0}", urlInfo.ToString());

			_logger?.LogInformation("************** Scraping New Released Movies Job End - {Date} **************", DateTime.Now.ToString("u", DateTimeFormatInfo.InvariantInfo));
		}


		//TDOO: Update to exclude multiple actor, vr, 4h and over
		public void ScanMovieDetails(UrlInfo urlInfo, Movie movie)
		{
			try
			{
				string requestUrl = GetJavLibraryUrl(urlInfo);
				HtmlDocument htmlDocument = TryGetHtmlDocument(requestUrl);
				if(htmlDocument != null)
				{
					if(movie.IdMovie == 0)
					{
						var titlePath = "//h3[@class='post-title text']";
						var titleNode = htmlDocument.DocumentNode.SelectSingleNode(titlePath).InnerText.Trim();
						var number = titleNode.Substring(0, titleNode.IndexOf(" "));
						var title = titleNode.Substring(titleNode.IndexOf(" ") + 1).ReplaceInvalidChar();

						var picPath = "//img[@id='video_jacket_img']";
						var picUrl = htmlDocument.DocumentNode.SelectSingleNode(picPath).Attributes["src"].Value;
						movie.CoverUrl = picUrl.StartsWith("http") ? picUrl : "http:" + picUrl;

						if(movie.Title == null)
							movie.Title = title;

						if(movie.Number == null)
							movie.Number = number;

						var dtReleasePath = "//div[@id='video_date']//td[@class='text']";
						var release = htmlDocument.DocumentNode.SelectSingleNode(dtReleasePath);
						DateTime rDate = DateTime.MinValue;
						if(release != null && !string.IsNullOrEmpty(release.InnerText))
						{
							if(DateTime.TryParse(release.InnerText.Trim(), out rDate))
								movie.DtRelease = rDate;
						}

						var lengthPath = "//div[@id='video_length']//span[@class='text']";
						var duration = htmlDocument.DocumentNode.SelectSingleNode(lengthPath);
						if(duration != null && !string.IsNullOrEmpty(duration.InnerText))
							movie.Duration = int.Parse(duration.InnerText.Trim());

						var actorPath = "//span[@class='star']//a";
						movie.Actor = GenerateMovieRoles(htmlDocument, actorPath, movie, JavlibRoleType.Actor);

						var dirPath = "//span[@class='director']//a";
						movie.Director = GenerateMovieRoles(htmlDocument, dirPath, movie, JavlibRoleType.Director);

						var comPath = "//span[@class='maker']//a";
						movie.Company = GenerateMovieRoles(htmlDocument, comPath, movie, JavlibRoleType.Company);

						var pubPath = "//span[@class='label']//a";
						movie.Publisher = GenerateMovieRoles(htmlDocument, pubPath, movie, JavlibRoleType.Publisher);

						var catPath = "//span[@class='genre']//a";
						movie.Category = GenerateMovieRoles(htmlDocument, catPath, movie, JavlibRoleType.Category);
					}

					var nbWantPath = "//span[@id='subscribed']//a";
					movie.NbWant = int.Parse(htmlDocument.DocumentNode.SelectSingleNode(nbWantPath).InnerText.Trim());

					var nbWatchedPath = "//span[@id='watched']//a";
					movie.NbWatched = int.Parse(htmlDocument.DocumentNode.SelectSingleNode(nbWatchedPath).InnerText.Trim());

					var nbOwnedPath = "//span[@id='owned']//a";
					movie.NbOwned = int.Parse(htmlDocument.DocumentNode.SelectSingleNode(nbOwnedPath).InnerText.Trim());

					int? wantLevel = movie.NbWant + movie.NbWatched + movie.NbOwned;
					if(wantLevel.HasValue)
					{
						if(wantLevel.Value >= _javlibSettings.DownloadSubPoint)
							movie.FavLevel = JavlibFavLevel.DlChineseSub;
						else if(wantLevel.Value >= _javlibSettings.DownloadMoviePoint)
							movie.FavLevel = JavlibFavLevel.DlMovie;
						else if(wantLevel.Value >= _javlibSettings.DownloadTorrentPoint)
							movie.FavLevel = JavlibFavLevel.DlTorrent;
					}

				}
				else
					_logger?.LogWarning("Nothing found when scanning movie details. UrlInfo: {urlInfo}", urlInfo.ToString());
			}
			catch(Exception ex)
			{
				_logger?.LogError(ex, "Error occurred when scanning movie {movieNumber} details: {urlInfo}", movie.Number, urlInfo.ToString());
			}
		}

		private string GenerateMovieRoles(HtmlDocument htmlDocument, string rolePath, Movie movie, JavlibRoleType roleType)
		{
			var roleNodes = htmlDocument.DocumentNode.SelectNodes(rolePath);
			string roleStr = string.Empty;
			if(roleNodes != null)
			{
				foreach(var roleNode in roleNodes)
				{
					var name = roleNode.InnerHtml.Trim();
					var url = roleNode.Attributes["href"].Value;
					roleStr += name + ",";

					switch(roleType)
					{
						case JavlibRoleType.Actor:
							Actor actor = _actorService.FindActorByName(name, url) ?? _actorService.SaveActor(new Actor() { Name = name, Url = url });
							movie.MovieRelations.Add(new MovieRelation() { IdRelation = actor.IdActor, IdTyRole = JavlibRoleType.Actor });
							break;
						case JavlibRoleType.Category:
							Category category = _categoryService.FindCategoryByName(name, url) ?? _categoryService.SaveCategory(new Category() { Name = name, Url = url });
							movie.MovieRelations.Add(new MovieRelation() { IdRelation = category.IdCategory, IdTyRole = JavlibRoleType.Category });
							break;
						case JavlibRoleType.Company:
							Company company = _companyService.FinCompanyByName(name, url) ?? _companyService.SaveCompany(new Company() { Name = name, Url = url });
							movie.MovieRelations.Add(new MovieRelation() { IdRelation = company.IdCompany, IdTyRole = JavlibRoleType.Company });
							break;
						case JavlibRoleType.Director:
							Director director = _directorService.FindDirectorByName(name, url) ?? _directorService.SaveDirector(new Director() { Name = name, Url = url });
							movie.MovieRelations.Add(new MovieRelation() { IdRelation = director.IdDirector, IdTyRole = JavlibRoleType.Director });
							break;
						case JavlibRoleType.Publisher:
							Publisher publisher = _publisherService.FindPublisherByName(name, url) ?? _publisherService.SavePublisher(new Publisher() { Name = name, Url = url });
							movie.MovieRelations.Add(new MovieRelation() { IdRelation = publisher.IdPublisher, IdTyRole = JavlibRoleType.Publisher });
							break;
					}
				}
			}
			return roleStr.TrimEnd(',');
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
								var thumbnail = urlAndTitle.ChildNodes[1].Attributes["src"].Value;
								thumbnail = thumbnail.StartsWith("http") ? thumbnail : "http:" + thumbnail;
								var name = urlAndTitle.ChildNodes[2].InnerText.Trim().ReplaceInvalidChar();
								var avUrl = urlAndTitle.Attributes["href"].Value.Trim().Replace("./", "");

								if(!string.IsNullOrEmpty(avUrl) && !string.IsNullOrEmpty(name) && !string.IsNullOrWhiteSpace(number))
								{
									Movie movie = _movieService.FindMovieByNumber(number) ?? new Movie() { Number = number, Title = name, Url = avUrl, ThumbnailUrl = thumbnail };
									lstMovie.Add(movie);
								}
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
			var cookieContainer = new CookieContainer();
			var baseAddress = new Uri(_javlibSettings.BaseAddress);
			cookieContainer.Add(baseAddress, new Cookie("cf_clearance", _javlibSettings.Cloudflare));
			var headers = new Dictionary<string, string>() { { "Cookie", cookieContainer.GetCookieHeader(baseAddress) } };

			return _htmlService.GetHtmlDocumentAsync(requestUrl, headers).Result;
		}
	}
}
