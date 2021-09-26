using MovieManager.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieManager.Core.Interfaces
{
	public interface IActorRepo
	{
		Actor FindById(int idActor);
		Actor FindByName(string name, string url = "");
		Actor Save(Actor actor);
	}
}
