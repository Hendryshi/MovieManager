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

namespace FunctionalTests
{
	public class ReportTests
	{
		private readonly ITestOutputHelper _output;
		private ReportService _reportService;
		
		public ReportTests(ITestOutputHelper output)
		{
			_output = output;
			_reportService = new ReportServiceBuilder().Build();
		}

		[Fact]
		public void Test_Insert_ScrapeReport()	
		{
			var report = new ScrapeReport() { NbDownload = 1, NbInterest = 2, NbReleased = 3 };
			var reportInserted = _reportService.SaveScrapeReport(report);
			Assert.True(reportInserted.IdReport > 0);
		}

		[Fact]
		public void Test_Insert_Doublon_DtReport_Should_Failed()
		{
			var report = new ScrapeReport() { NbDownload = 1, NbInterest = 2, NbReleased = 3 };
			Assert.Throws<System.Data.SqlClient.SqlException>(() => _reportService.SaveScrapeReport(report));
		}

		[Fact]
		public void Test_Find_ScrapeReport_ById()
		{
			int idReport = 1;
			var report = _reportService.FindScrapeReportById(idReport);
			Assert.Equal(1, report.IdReport);

			idReport = 0;
			report = _reportService.FindScrapeReportById(idReport);
			Assert.Null(report);
		}

		[Fact]
		public void Test_Find_ScrapeReport_ByDate()
		{
			DateTime dtReport = new DateTime(2022, 8, 23);
			var report = _reportService.FindScrapeReportByDate(dtReport);
			Assert.Equal(1, report.IdReport);

			dtReport = new DateTime(2022, 8, 20);
			report = _reportService.FindScrapeReportByDate(dtReport);
			Assert.Null(report);
		}


		[Fact]
		public void Test_Update_ScrapeReport()
		{
			int idReport = 1;
			var report = _reportService.FindScrapeReportById(idReport);

			report.NbInterest = 100;

			_reportService.SaveScrapeReport(report);

			var reportModified = _reportService.FindScrapeReportById(idReport);
			Assert.Equal(100, report.NbInterest);
		}

		[Fact]
		public void Test_Get_Report_To_Send()
		{
			var report = _reportService.GetScrapeReportToSend();
			Assert.NotNull(report);
			Assert.Equal(1, report.IdReport);
		}

		[Fact]
		public void Test_Send_Scrapt_Report()
		{
			DateTime dtReport = new DateTime(2022, 8, 24);
			_reportService.SendScrapeReport(dtReport);
		}
	}
}
