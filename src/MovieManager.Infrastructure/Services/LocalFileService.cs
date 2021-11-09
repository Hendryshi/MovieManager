using Microsoft.Extensions.Options;
using MovieManager.Core.Entities;
using MovieManager.Core.Enumerations;
using MovieManager.Core.Interfaces;
using MovieManager.Core.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieManager.Infrastructure.Services
{
	public class LocalFileService : ILocalFileService
	{
		private readonly LocalFileSettings _localFileSetting;
		private readonly IAppLogger<LocalFileService> _logger;

		public LocalFileService(IAppLogger<LocalFileService> logger, IOptionsSnapshot<LocalFileSettings> localFileSetting)
		{
			_logger = logger;
			_localFileSetting = localFileSetting.Value;
		}

		public bool CheckMovieDownloaded(string savePath, string movieNumber)
		{
			return FindMovieFile(savePath, movieNumber) != null;
		}

		//This function is leaved as async as it can be a copy function in future => This will take some time
		public async Task GenerateMovieFile(MovieMagnet magnet)
		{
			_logger?.LogInformation("GenerateMovieFile Start - {movieNumber}", magnet.MovieNumber);
			FileInfo movieFile = FindMovieFile(magnet.SavePath, magnet.MovieNumber);

			if(movieFile != null)
			{
				string movieNameWithoutExt = magnet.MovieNumber.ToUpper() + (magnet.HasSub ? "-C" : "");
				string destMovieName = movieNameWithoutExt + Path.GetExtension(movieFile.FullName);

				string destMovieFolder = Path.Combine(_localFileSetting.DestSaveRootPath, movieNameWithoutExt);
				string destMoviePath = Path.Combine(destMovieFolder, destMovieName);

				if(!Directory.Exists(destMovieFolder))
				{
					_logger.LogInformation("Create destination movie folder: {destMovieFolder}", destMovieFolder);
					Directory.CreateDirectory(destMovieFolder);
				}

				_logger.LogInformation("Moving movie {movieNumber} to {destMoviePath}", magnet.MovieNumber, destMoviePath);
				await Task.Run(() => movieFile.MoveTo(destMoviePath, true));

				_logger.LogInformation("Movie {movieNumber} has been moved to dest location. Now Archiving the temp download folder", magnet.MovieNumber);
				MoveDirectory(magnet.SavePath, _localFileSetting.ArchivedDownloadPath);

				magnet.SavePath = destMovieFolder;
				magnet.IdStatus = MagnetStatus.Finished;
				magnet.DtFinish = DateTime.Now;
			}
			else
				_logger.LogWarning("Cannot find movie {movieNumber} in path: {savePath}", magnet.MovieNumber, magnet.SavePath);
		}
		
		public async Task DeleteFolder(string path)
		{
			_logger.LogInformation("Deleting folder {path}", path);
			if(Directory.Exists(path))
			{
				await Task.Run(() => Directory.Delete(path, true));
				_logger.LogInformation("Delete folder finished: {path}", path);
			}
			else
				_logger.LogWarning("Delete folder aborted. Cannot find path: {path}", path);
		}

		public void MoveDirectory(string sourcePath, string targetParentPath, bool deleteIfExist = true)
		{
			if(!Directory.Exists(sourcePath))
				throw new Exception(string.Format("Directory move cancelled. Cannot find the sourcePath: {0}", sourcePath));

			if(!Directory.Exists(targetParentPath))
				Directory.CreateDirectory(targetParentPath);

			DirectoryInfo dirInfo = new DirectoryInfo(sourcePath);
			string targetPath = Path.Combine(targetParentPath, dirInfo.Name);

			if(Directory.Exists(targetPath))
			{
				if(deleteIfExist)
				{
					_logger.LogInformation("Found an exist folder when moving folder from {sourcePath} to {targetParentPath}. Delete it", sourcePath, targetParentPath);
					Directory.Delete(targetPath, true);		
				}
				else
					throw new Exception(string.Format("Directory move cancelled. There already have the exist folder [{0}] in targetPath [{1}]", dirInfo.Name, targetParentPath));
			}

			dirInfo.MoveTo(targetPath);
		}

		public FileInfo FindMovieFile(string rootPath, string movieNumber)
		{
			string numericInMovie = new String(movieNumber.Where(Char.IsDigit).ToArray());

			if(!Directory.Exists(rootPath))
				return null;

			List<string> lstFiles = Directory.GetFiles(rootPath, "*.*", SearchOption.AllDirectories).ToList();
			List<FileInfo> lstMediaFiles = lstFiles.Where(x => x.EndsWith("mp4") || x.EndsWith("mkv")).Select(f => new FileInfo(f)).ToList();

			return lstMediaFiles.Where(x => x.Name.Contains(numericInMovie)).OrderByDescending(f => f.Length).FirstOrDefault();
		}
	}
}
