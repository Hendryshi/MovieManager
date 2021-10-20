using HtmlAgilityPack;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace MovieManager.Core.Interfaces
{
	public interface IQbittorrentService
	{
		Task EnsureLoggedIn();
	}
}