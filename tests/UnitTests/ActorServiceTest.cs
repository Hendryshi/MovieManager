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
	public class ActorServiceTest
	{
		private readonly ITestOutputHelper _output;
		private ActorService _actorService;
		
		public ActorServiceTest(ITestOutputHelper output)
		{
			_output = output;
			_actorService = new ActorServiceBuilder().Build();
		}

		[Fact]
		public void TestInsertActor()	
		{
			var actor = new Actor() { Name = "ActorTest", Description = "description", FavLevel = JavlibFavLevel.DlMovie, Url = "google.fr" };
			var actorInserted = _actorService.SaveActor(actor);
			Assert.True(actorInserted.IdActor > 0);
		}

		[Fact]
		public void TesFindActorById()
		{
			int idActor = 1;
			var actor = _actorService.FindActorById(idActor);
			Assert.Equal("ActorTest", actor.Name);

			idActor = 0;
			actor = _actorService.FindActorById(idActor);
			Assert.True(actor == null);
		}

		[Fact]
		public void TesFindActorByName()
		{
			string name = "ActorTest";
			var actor = _actorService.FinActorByName(name);
			Assert.Equal(name, actor.Name);

			string url = "urlfalse";
			actor = _actorService.FinActorByName(name, url);
			Assert.True(actor == null);
		}

		[Fact]
		public void TestUpdateActor()
		{
			int idActor = 1;
			var actor = _actorService.FindActorById(idActor);

			actor.Name = "name modified";

			_actorService.SaveActor(actor);

			var actorModified = _actorService.FindActorById(idActor);
			Assert.Equal("name modified", actor.Name);
		}

	}
}
