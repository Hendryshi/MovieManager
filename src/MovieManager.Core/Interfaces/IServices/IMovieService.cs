﻿using MovieManager.Core.Entities;
using MovieManager.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieManager.Core.Interfaces
{
	public interface IMovieService
	{
		Movie FindMovieById(int idMovie);
		Movie FindMovieByNumber(string movieNbr);
		List<Movie> FindMovieByCriteria(DateTime? dtReleaseMin = null, DateTime? dtReleaseMax = null);
		List<Movie> LoadMoviesToDownload();
		List<Movie> LoadMoviesToScrapeMagnet();
		Movie SaveMovie(Movie movie, bool updateRelation = false);
		Movie UpdateStatus(Movie movie, MovieStatus newStatus);
	}
}
