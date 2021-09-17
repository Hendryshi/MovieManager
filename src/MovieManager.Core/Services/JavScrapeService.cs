using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.Extensions.Options;
using MovieManager.Core.Interfaces;
using MovieManager.Core.Settings;

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



		public int GetPageCount(string pageUrl)
		{
			int lastPage = 0;
			HtmlDocument htmlDocument = TryGetHtmlDocument(pageUrl);
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

		private HtmlDocument TryGetHtmlDocument(string requestUrl)
		{
			int retry = 0;
			int maxRetry = 2;
			HtmlDocument html = null;

			var cookieContainer = new CookieContainer();
			var baseAddress = new Uri(_javlibSettings.BaseAddress);
			cookieContainer.Add(baseAddress, new Cookie("cf_clearance", _javlibSettings.Cloudflare));
			var headers = new Dictionary<string, string>() { { "Cookie", cookieContainer.GetCookieHeader(baseAddress) } };

			while(retry < maxRetry && html == null)
			{
				try
				{
					html = _htmlService.GetHtmlDocumentAsync(requestUrl, headers).Result;
				}
				catch(Exception ex)
				{
					_logger?.LogError($"Error when requesting page: {requestUrl}", ex);
					if(retry < maxRetry)
						_logger?.LogInformation($"Retrying {++retry}/{maxRetry} times");
				}
			}

			return html;
		}
	}
}
