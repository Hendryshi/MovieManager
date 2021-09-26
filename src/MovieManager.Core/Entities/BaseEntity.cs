using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieManager.Core.Entities
{
	public abstract class BaseEntity
	{
		public override string ToString() => JsonConvert.SerializeObject(this);
	}
}
