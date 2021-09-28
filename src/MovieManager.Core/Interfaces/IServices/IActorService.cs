using MovieManager.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieManager.Core.Interfaces
{
	public interface IActorService
	{
		Actor FindActorByName(string name, string url = "");
		Actor FindActorById(int idActor);
		Actor SaveActor(Actor actor);
	}
}
