using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Moq;

namespace UnitTests.Builders
{
	public class ConfigBuilder
	{
		private Mock<IConfiguration> _mockConfiguration;
		private Mock<IConfigurationSection> _mockConfSection;

		public ConfigBuilder()
		{
			_mockConfSection = new Mock<IConfigurationSection>();
			_mockConfSection.SetupGet(m => m[It.Is<string>(s => s == "MovieManagerDB")]).Returns("Data Source=Hendryshi\\YSISQLSERVER;Initial Catalog=MovieManager;Persist Security Info=True;User ID=sa;Password=pwd");
			_mockConfiguration = new Mock<IConfiguration>();
			_mockConfiguration.Setup(a => a.GetSection(It.Is<string>(s => s == "ConnectionStrings"))).Returns(_mockConfSection.Object);
	}

		public IConfiguration Build()
		{
			return _mockConfiguration.Object;
		}
	}
}
