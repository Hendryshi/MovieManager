﻿using MovieManager.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieManager.Core.Interfaces
{
	public interface IMovieRelationRepo
	{
		List<MovieRelation> LoadAllRelations(int idMovie);
		void SaveAllRelations(int idMovie, List<MovieRelation> movieRelations);
	}
}
