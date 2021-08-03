using MovieManager.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieManager.Core.Job
{
	public class TestJob : ITestJob
	{
		
		private MovieManager.Infrastructure.Repositories.BaseRepository _baseRepository;
		public TestJob(Infrastructure.Repositories.BaseRepository baseRepository)
		{
			_baseRepository = baseRepository;
		}

		public void Run()
		{
			Console.WriteLine("Job Enter");
		}
	}
}
