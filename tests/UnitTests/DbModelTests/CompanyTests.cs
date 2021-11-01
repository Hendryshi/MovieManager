using System;
using Xunit;
using Moq;
using UnitTests.Builders;
using MovieManager.Core.Interfaces;
using MovieManager.Core.Services;
using MovieManager.Core.Entities;
using MovieManager.Infrastructure.DbContext;
using MovieManager.Infrastructure.Repositories;
using Xunit.Abstractions;
using System.Collections.Generic;
using System.Linq;
using MovieManager.Core.Enumerations;

namespace UnitTests
{
	public class CompanyTests
	{
		private readonly ITestOutputHelper _output;
		private CompanyService _companyService;
		
		public CompanyTests(ITestOutputHelper output)
		{
			_output = output;
			_companyService = new CompanyServiceBuilder().Build();
		}

		[Fact]
		public void TestInsertCompany()	
		{
			var company = new Company() { Name = "CompanyTest", Description = "description", FavLevel = JavlibFavLevel.DlMovie, Url = "google.fr" };
			var companyInserted = _companyService.SaveCompany(company);
			Assert.True(companyInserted.IdCompany > 0);
		}

		[Fact]
		public void TesFindCompanyById()
		{
			int idCompany = 1;
			var company = _companyService.FindCompanyById(idCompany);
			Assert.Equal("CompanyTest", company.Name);

			idCompany = 0;
			company = _companyService.FindCompanyById(idCompany);
			Assert.True(company == null);
		}

		[Fact]
		public void TesFindCompanyByName()
		{
			string name = "CompanyTest";
			var company = _companyService.FinCompanyByName(name);
			Assert.Equal(name, company.Name);

			string url = "urlfalse";
			company = _companyService.FinCompanyByName(name, url);
			Assert.True(company == null);
		}

		[Fact]
		public void TestUpdateCompany()
		{
			int idCompany = 1;
			var company = _companyService.FindCompanyById(idCompany);
			string expectedValue = company.Url += "r";
			
			company.Url = expectedValue;
			_companyService.SaveCompany(company);

			var companyModified = _companyService.FindCompanyById(idCompany);
			Assert.Equal(expectedValue, company.Url);
		}

	}
}
