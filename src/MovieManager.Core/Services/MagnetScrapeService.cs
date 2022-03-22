using Microsoft.Extensions.Options;
using MovieManager.Core.Interfaces;
using MovieManager.Core.Settings;
using MovieManager.Core.Enumerations;
using MovieManager.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using MovieManager.Core.Extensions;
using System.Globalization;

namespace MovieManager.Core.Services
{
	public class MagnetScrapeService : IMagnetScrapeService
	{
		private readonly IAppLogger<MagnetScrapeService> _logger;
		private readonly IMovieService _movieService;
		private readonly IMovieMagnetService _movieMagnetService;
		private readonly MagnetSettings _magnetSettings;
		private readonly IHtmlService _htmlService;

		public MagnetScrapeService(IAppLogger<MagnetScrapeService> logger, IOptionsSnapshot<MagnetSettings> magnetSettings,
			IMovieService movieService, IMovieMagnetService movieMagnetService, IHtmlService htmlService)
		{
			_logger = logger;
			_magnetSettings = magnetSettings.Value;
			_movieService = movieService;
			_movieMagnetService = movieMagnetService;
			_htmlService = htmlService;
		}

		public void DailyDownloadMovieMagnet()
		{
			_logger.LogInformation("************** Start Daily Movie Magnet Download Job - {Date} **************", DateTime.Now.ToString("u", DateTimeFormatInfo.InvariantInfo));
			List<Movie> movies = _movieService.LoadMoviesToScrapeMagnet();
			movies.ForEach(m => ScrapeMovieMagnet(m));
			_logger.LogInformation("************** Daily Movie Magnet Download Job End - {Date} **************", DateTime.Now.ToString("u", DateTimeFormatInfo.InvariantInfo));
		}

		
		public void ScrapeMovieMagnet(Movie movie)
		{
			try
			{
				List<MovieMagnet> magnets = new List<MovieMagnet>();
				foreach(MagnetSource magnetSource in _magnetSettings.GetSearchSources())
				{
					switch(magnetSource)
					{
						case MagnetSource.Javbus:
							magnets.AddRange(SearchMagnetFromJavbus(movie));
							break;
						case MagnetSource.Sukebei:
							magnets.AddRange(SearchMagnetFromSukebei(movie));
							break;
					}
				}

				magnets.RemoveAll(m => m.Size == 0 || _movieMagnetService.FindMovieMagnetByHash(m.Hash) != null);
				magnets = magnets.GroupBy(x => x.Hash.ToLower()).Select(x => x.First()).OrderByDescending(x => x.HasSub).ThenByDescending(x => x.IsHD).ThenByDescending(x => x.DtMagnet).ThenByDescending(x => x.Size).ToList();

				if(magnets.Count > 0)
				{
					_movieMagnetService.SaveMovieMagnetList(magnets);
					_movieService.UpdateStatus(movie, MovieStatus.HasTorrent);
					_movieService.SaveMovie(movie);
					_logger?.LogInformation("Scrape magnet for movie {movieNumber}: {magCount} new torrents inserted", movie.Number, magnets.Count);
				}
				else
					_logger?.LogWarning("Scrape magnet for movie {movieNumber}: no new torrent found.", movie.Number);
			}
			catch(Exception ex)
			{
				_logger?.LogError(ex, "Error occurred when scraping magnet for movie: {movieNumber}", movie.Number);
			}
		}

		public List<MovieMagnet> SearchMagnetFromJavbus(Movie movie)
		{
			string javbusUrl = string.Format(_movieMagnetService.LoadMagSourceUrl(MagnetSource.Javbus), movie.Number);
			HtmlDocument htmlDocument = _htmlService.GetHtmlDocumentAsync(javbusUrl, maxRetry:1).Result;
			List<MovieMagnet> lstMovieMagnets = new List<MovieMagnet>();

			if(htmlDocument != null)
			{
				string outerHtml = htmlDocument.DocumentNode.InnerHtml;
				var gidPattern = "var gid = (.*?);";
				var ucPattern = "var uc = (.*?);";
				var picPattern = "var img = '(.*?)';";

				var gidMatch = Regex.Match(outerHtml, gidPattern);
				var ucMatch = Regex.Match(outerHtml, ucPattern);
				var picMatch = Regex.Match(outerHtml, picPattern);

				var gid = gidMatch.Groups[1].Value;
				var uc = ucMatch.Groups[1].Value;
				var pic = picMatch.Groups[1].Value;

				HtmlDocument magDocument = null;
				var magUrl = $"https://www.javbus.com/ajax/uncledatoolsbyajax.php?gid={gid}&lang=zh&img={pic}&uc={uc}&floor=552";
				magDocument = _htmlService.GetHtmlDocumentAsync(magUrl, new Dictionary<string, string>() { { "referer", javbusUrl } }).Result;

				if(magDocument != null)
				{
					var magPattern = "//tr[@style=' border-top:#DDDDDD solid 1px']";
					HtmlNodeCollection nodes = magDocument.DocumentNode.SelectNodes(magPattern);
					if(nodes != null)
					{
						DateTime? lastDtReleaseInNode = null;
						foreach(var node in nodes)
						{
							MovieMagnet movieMagnet = new MovieMagnet() { IdMovie = movie.IdMovie, MovieNumber = movie.Number, IdMagSource = MagnetSource.Javbus };
							if(node.ChildNodes.Count >= 2)
							{
								if(node.ChildNodes[1].InnerText.Contains("高清"))
									movieMagnet.IsHD = true;

								if(node.ChildNodes[1].InnerText.Contains("字幕") && movieMagnet.IsHD)
									movieMagnet.HasSub = true;

								movieMagnet.MagName = node.ChildNodes[1].ChildNodes[1].InnerText.Trim();
								movieMagnet.MagnetUrl = node.ChildNodes[1].ChildNodes[1].Attributes["href"].Value;
								movieMagnet.GenerateHash();
							}

							if(node.ChildNodes.Count >= 4)
							{
								string sizePart = node.ChildNodes[3].InnerText.Trim();
								movieMagnet.Size = sizePart.GetByteSize();
							}

							if(node.ChildNodes.Count >= 5 && !string.Equals(node.ChildNodes[5].InnerText.Trim(), "0000-00-00"))
							{
								movieMagnet.DtMagnet = DateTime.Parse(node.ChildNodes[5].InnerText.Trim());
								if(!lastDtReleaseInNode.HasValue || movieMagnet.DtMagnet < lastDtReleaseInNode)
									lastDtReleaseInNode = movieMagnet.DtMagnet;
							}else
								movieMagnet.HasSub = false;

							lstMovieMagnets.Add(movieMagnet);
						}

						//JavBus Bug: The movie is not a SubVersion but has a Sub tag
						foreach(MovieMagnet mag in lstMovieMagnets)
						{
							if(mag.DtMagnet.HasValue && (mag.DtMagnet.Value - lastDtReleaseInNode.Value).Days < 3)
								mag.HasSub = false;
						}
					}
				}
			}
			return lstMovieMagnets;
		}

		public List<MovieMagnet> SearchMagnetFromSukebei(Movie movie)
		{

			string javbusUrl = string.Format(_movieMagnetService.LoadMagSourceUrl(MagnetSource.Sukebei), movie.Number);
			HtmlDocument htmlDocument = _htmlService.GetHtmlDocumentAsync(javbusUrl, maxRetry:1).Result;
			List<MovieMagnet> lstMovieMagnets = new List<MovieMagnet>();

			if(htmlDocument != null)
			{
				string xpath = "//tr";
				HtmlNodeCollection nodes = htmlDocument.DocumentNode.SelectNodes(xpath);

				if(nodes != null && nodes.Count > 1)
				{
					foreach(var node in nodes.Skip(1))
					{
						MovieMagnet movieMagnet = new MovieMagnet() { IdMovie = movie.IdMovie, MovieNumber = movie.Number, IdMagSource = MagnetSource.Sukebei };

						movieMagnet.MagName = node.ChildNodes[3].InnerText.Trim();

						var magHref = node.ChildNodes[5].OuterHtml;
						var size = node.ChildNodes[7].InnerText.Trim();
						movieMagnet.Size = size.GetByteSize();

						DateTime dtMag = DateTime.MinValue;
						if(!string.IsNullOrEmpty(node.ChildNodes[9].InnerText) && DateTime.TryParse(node.ChildNodes[9].InnerText.Trim(), out dtMag))
							movieMagnet.DtMagnet = dtMag;

						var url = magHref.Substring(magHref.IndexOf("<a href=\"magnet:?xt") + 9);
						movieMagnet.MagnetUrl = url.Substring(0, url.IndexOf("\""));
						movieMagnet.GenerateHash();

						if(movieMagnet.MagName.Contains("高清") || movieMagnet.MagName.ToLower().Contains("hd") || movieMagnet.Size > 2048)
							movieMagnet.IsHD = true;

						if((movieMagnet.MagName.Contains("字幕") || movieMagnet.MagName.Contains("中文")) && movieMagnet.IsHD)
							movieMagnet.HasSub = true;

						lstMovieMagnets.Add(movieMagnet);
					}
				}
			}

			return lstMovieMagnets;
		}

    }
}
