using HtmlAgilityPack;
using MovieManager.Core.Entities;
using QBittorrent.Client;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace MovieManager.Core.Interfaces
{
	public interface IQbittorrentService
	{
		Task AddTorrentAsync(MovieMagnet magnet);
		Task DeleteTorrentAsync(string hash, bool deleteDownloaded = true);
		Task EnsureLoggedInAsync();
		int GetTorrentCanAddCount();
		TorrentInfo GetTorrentInfo(string hash);
	}
}