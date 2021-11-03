using HtmlAgilityPack;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace MovieManager.Core.Interfaces
{
	public interface IHtmlService
	{
		Task<HtmlDocument> GetHtmlDocumentAsync(string requestUrl, Dictionary<string, string> headers = null, int maxRetry = 2);
	}
}