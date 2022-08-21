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
	public class PublisherTests
	{
		private readonly ITestOutputHelper _output;
		private PublisherService _publisherService;
		
		public PublisherTests(ITestOutputHelper output)
		{
			_output = output;
			_publisherService = new PublisherServiceBuilder().Build();
		}

		[Fact]
		public void TestInsertPublisher()	
		{
			var publisher = new Publisher() { Name = "PublisherTest", Description = "description", FavLevel = JavlibFavLevel.DlMovie, Url = "google.fr" };
			var publisherInserted = _publisherService.SavePublisher(publisher);
			Assert.True(publisherInserted.IdPublisher > 0);
		}

		[Fact]
		public void TesFindPublisherById()
		{
			int idPublisher = 1;
			var publisher = _publisherService.FindPublisherById(idPublisher);
			Assert.Equal("PublisherTest", publisher.Name);

			idPublisher = 0;
			publisher = _publisherService.FindPublisherById(idPublisher);
			Assert.True(publisher == null);
		}

		[Fact]
		public void TesFindPublisherByName()
		{
			string name = "PublisherTest";
			var publisher = _publisherService.FindPublisherByName(name);
			Assert.Equal(name, publisher.Name);

			string url = "urlfalse";
			publisher = _publisherService.FindPublisherByName(name, url);
			Assert.True(publisher == null);
		}

		[Fact]
		public void TestUpdatePublisher()
		{
			int idPublisher = 1;
			var publisher = _publisherService.FindPublisherById(idPublisher);
			string expectedValue = publisher.Url += "r";
			
			publisher.Url = expectedValue;
			_publisherService.SavePublisher(publisher);

			var publisherModified = _publisherService.FindPublisherById(idPublisher);
			Assert.Equal(expectedValue, publisher.Url);
		}

	}
}
