using MovieManager.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieManager.Core.Interfaces
{
	public interface ICompanyRepo
	{
		Company FindById(int idCompany);
		Company FindByName(string name, string url = "");
		Company Save(Company company);
	}
}
