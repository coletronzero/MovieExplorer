using System;
using MovieExplorer.Client.Models;
using MovieExplorer.Client.Services;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using MovieExplorer.Client.Messages;
using MovieExplorer.Client.NavigationParameters;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace MovieExplorer.Client.ViewModels
{
    public class DetailViewModel : MvxViewModel
    {
        private const string IMAGE_PREFIX = "http://image.tmdb.org/t/p/w500";

        private IMovieSharpService _movieClient;

        public DetailViewModel(IMovieSharpService movieClient)
        {
            _movieClient = movieClient;
        }

        public async void Init(SelectedMovie parameters)
        {
            var movieResponse = await _movieClient.GetMovieAsync(parameters.MovieId);
            if (movieResponse.IsOk)
            {
                SelectedMovie = movieResponse.Body;
            }
        }

        public Action MovieSelectedDelegate { get; set; }

        private MovieDto _selectedMovie;
        public MovieDto SelectedMovie
        {
            get { return _selectedMovie; }
            set
            {
                if(SetProperty(ref _selectedMovie,value))
                {
                    PosterUrl = IMAGE_PREFIX + _selectedMovie.PosterPath;
                    Title = _selectedMovie.Title;
                    ReleaseDate = "Release Date: " + _selectedMovie.ReleaseDate;
                    Rating = _selectedMovie.VoteAverage.ToString() + " Stars";
                    Votes = "(from " + _selectedMovie.VoteCount.ToString() + " votes)";
                    Overview = _selectedMovie.Overview;

                    MovieSelectedDelegate?.Invoke();
                }
            }
        }

        private string _posterUrl;
        public string PosterUrl
        {
            get { return _posterUrl; }
            set { SetProperty(ref _posterUrl, value); }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private string _releaseDate;
        public string ReleaseDate
        {
            get { return _releaseDate; }
            set { SetProperty(ref _releaseDate, value); }
        }

        private string _rating;
        public string Rating
        {
            get { return _rating; }
            set { SetProperty(ref _rating, value); }
        }

        private string _votes;
        public string Votes
        {
            get { return _votes; }
            set { SetProperty(ref _votes, value); }
        }

        private string _overview;
        public string Overview
        {
            get { return _overview; }
            set { SetProperty(ref _overview, value); }
        }
        
        private List<MovieDto> _similarMovies;
        public async Task<List<MovieDto>> GetSimilarMoviesAsync()
        {
            _similarMovies = new List<MovieDto>();
            if(SelectedMovie != null && SelectedMovie.Id > 0)
            {
                var similarMoviesResponse = await _movieClient.GetSimilarMoviesAsync(SelectedMovie.Id);
                if (similarMoviesResponse.IsOk)
                {
                    _similarMovies = similarMoviesResponse.Body.Results;
                }
            }
            return _similarMovies;
        }

        public void SelectSimilarMovie(int id)
        {
            var movie = _similarMovies.Where(x => x.Id == id).FirstOrDefault();
            if (movie != null)
            {
                SelectedMovie = movie;
            }
        }
    }
}