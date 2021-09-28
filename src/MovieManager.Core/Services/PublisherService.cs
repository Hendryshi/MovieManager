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
	public class PublisherService : IPublisherService
	{
		private readonly IPublisherRepo _publisherRepo;
		private readonly IAppLogger<PublisherService> _logger;

		public PublisherService(IAppLogger<PublisherService> logger, IPublisherRepo publisherRepo)
		{
			_publisherRepo = publisherRepo;
			_logger = logger;
		}

		public Publisher SavePublisher(Publisher publisher)
		{
			return _publisherRepo.Save(publisher);
		}

		public Publisher FindPublisherById(int idPublisher)
		{
			return _publisherRepo.FindById(idPublisher);
		}

		public Publisher FindPublisherByName(string name, string url = "")
		{
			return _publisherRepo.FindByName(name, url);
		}

	}
}
