using MovieManager.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieManager.Core.Interfaces
{
	public interface ILocalFileService
	{
		bool CheckMovieDownloaded(string savePath, string movieNumber);
		Task DeleteFolder(string path);
		Task GenerateMovieFile(MovieMagnet magnet);
	}
}
