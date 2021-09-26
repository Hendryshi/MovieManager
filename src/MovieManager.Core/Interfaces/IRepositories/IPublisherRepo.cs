using MovieManager.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieManager.Core.Interfaces
{
	public interface IPublisherRepo
	{
		Publisher FindById(int idPublisher);
		Publisher FindByName(string name, string url = "");
		Publisher Save(Publisher publisher);
	}
}
