using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MovieManager.Core.Interfaces;
using MovieManager.Core.Entities;

namespace MovieManager.Core.Services
{
	public class DirectorService : IDirectorService
	{
		private readonly IDirectorRepo _directorRepo;
		private readonly IAppLogger<DirectorService> _logger;

		public DirectorService(IAppLogger<DirectorService> logger, IDirectorRepo directorRepo)
		{
			_directorRepo = directorRepo;
			_logger = logger;
		}

		public Director SaveDirector(Director director)
		{
			return _directorRepo.Save(director);
		}

		public Director FindDirectorById(int idDirector)
		{
			return _directorRepo.FindById(idDirector);
		}

		public Director FindDirectorByName(string name, string url = "")
		{
			return _directorRepo.FindByName(name, url);
		}
	}
}
