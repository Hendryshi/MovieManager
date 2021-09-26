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
	public class CompanyService : ICompanyService
	{
		private readonly ICompanyRepo _companyRepo;
		private readonly IAppLogger<CompanyService> _logger;

		public CompanyService(IAppLogger<CompanyService> logger, ICompanyRepo companyRepo)
		{
			_companyRepo = companyRepo;
			_logger = logger;
		}

		public Company SaveCompany(Company company)
		{
			return _companyRepo.Save(company);
		}

		public Company FindCompanyById(int idCompany)
		{
			return _companyRepo.FindById(idCompany);
		}

		public Company FinCompanyByName(string name, string url = "")
		{
			return _companyRepo.FindByName(name, url);
		}

	}
}
