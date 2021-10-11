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
	public class ActorService : IActorService
	{
		private readonly IActorRepo _actorRepo;
		private readonly IAppLogger<ActorService> _logger;

		public ActorService(IAppLogger<ActorService> logger, IActorRepo actorRepo)
		{
			_actorRepo = actorRepo;
			_logger = logger;
		}

		public Actor SaveActor(Actor actor)
		{
			return _actorRepo.Save(actor);
		}

		public Actor FindActorById(int idActor)
		{
			return _actorRepo.FindById(idActor);
		}

		public Actor FindActorByName(string name, string url = "")
		{
			return _actorRepo.FindByName(name, url);
		}

	}
}
