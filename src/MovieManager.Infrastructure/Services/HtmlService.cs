﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.Extensions.Options;
using MovieManager.Core.Interfaces;
using MovieManager.Core.Settings;

namespace MovieManager.Infrastructure.Services
{
	public class HtmlService : IHtmlService
	{
		private readonly CommonSettings _commonSettings;
		private readonly IAppLogger<HtmlService> _logger;

		public HtmlService(IAppLogger<HtmlService> logger, IOptionsSnapshot<CommonSettings> commonSettings)
		{
			_logger = logger;
			_commonSettings = commonSettings.Value;
		}

		public async Task<HtmlDocument> GetHtmlDocumentAsync(string requestUrl, Dictionary<string, string> headers = null, int maxRetry = 2)
		{
			int retry = 0;
			HtmlDocument doc = null;

			while(retry <= maxRetry && doc == null)
			{
				try
				{
					using(HttpClient client = new())
					{
						client.DefaultRequestHeaders.Add("User-Agent", _commonSettings.DefaultUserAgent);
						if(headers != null)
						{
							foreach(var item in headers)
								client.DefaultRequestHeaders.Add(item.Key, item.Value);
						}
						var html = await client.GetStringAsync(requestUrl);

						if(!string.IsNullOrWhiteSpace(html))
						{
							doc = new HtmlDocument();
							doc.LoadHtml(html);
						}
					}
				}
				catch(Exception ex)
				{
					_logger?.LogWarning(ex, $"Error when requesting page: {requestUrl}");
					retry++;
					if(retry <= maxRetry)
					{
						Task.Delay(1000).Wait();
						_logger?.LogInformation($"Retrying {retry}/{maxRetry} times");
					}
				}
			}

			return doc;
		}
	}
}
