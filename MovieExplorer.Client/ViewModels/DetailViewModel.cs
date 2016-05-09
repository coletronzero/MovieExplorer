using System;
using MovieExplorer.Client.Models;
using MovieExplorer.Client.Services;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using MovieExplorer.Client.Messages;
using MovieExplorer.Client.NavigationParameters;

namespace MovieExplorer.Client.ViewModels
{
    public class DetailViewModel : MvxViewModel
    {
        private IMovieSharpService _movieClient;
        private readonly MvxSubscriptionToken _token;
        public DetailViewModel(IMovieSharpService movieClient, IMvxMessenger messenger)
        {
            _movieClient = movieClient;
            _token = messenger.Subscribe<SelectedMovieMessage>(OnSelectedMovieMessage);
        }

        private void OnSelectedMovieMessage(SelectedMovieMessage obj)
        {
            SelectedMovie = obj.SelectedMovie;
        }

        public async void Init(SelectedMovie parameters)
        {
            var movieResponse = await _movieClient.GetMovieAsync(parameters.MovieId);
            if (movieResponse.IsOk)
            {
                SelectedMovie = movieResponse.Body;
            }
        }

        private string _hello = "Hello MvvmCross";
        public string Hello
        { 
            get { return _hello; }
            set { SetProperty (ref _hello, value); }
        }

        private Movie _selectedMovie;
        public Movie SelectedMovie
        {
            get { return _selectedMovie; }
            set
            {
                if(SetProperty(ref _selectedMovie,value))
                {
                    Title = _selectedMovie.Title;
                    ReleaseDate = _selectedMovie.ReleaseDate;
                    Votes = "(from " + _selectedMovie.VoteCount.ToString() + " votes)";
                    Overview = _selectedMovie.Overview;
                }
            }
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
    }
}
