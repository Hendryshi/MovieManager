using MovieManager.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieManager.Core.Interfaces
{
	public interface ICompanyService
	{
		Company FindCompanyByName(string name, string url = "");
		Company FindCompanyById(int idCompany);
		Company SaveCompany(Company company);
	}
}
