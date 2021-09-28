using MovieManager.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieManager.Core.Interfaces
{
	public interface IPublisherService
	{
		Publisher FindPublisherByName(string name, string url = "");
		Publisher FindPublisherById(int idPublisher);
		Publisher SavePublisher(Publisher publisher);
	}
}
