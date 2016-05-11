using MovieExplorer.Client.Models;
using MovieExplorer.Client.NavigationParameters;
using MovieExplorer.Client.Services;
using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace MovieExplorer.Client.ViewModels
{
    public class SearchViewModel : MvxViewModel
    {
        private IMovieSharpService _movieClient;

        public SearchViewModel(IMovieSharpService movieClient)
        {
            _movieClient = movieClient;
            SearchResults = new List<MovieDto>();
        }

        public async void Init(SearchQuery parameters)
        {
            var searchResponse = await _movieClient.SearchMoviesAsync(parameters.Query);
            if (searchResponse.IsOk)
            {
                SearchResults = searchResponse.Body.Results;
                ShowSearchResultsDelegate?.Invoke();
            }
        }

        private string _searchQuery;
        public string SearchQuery
        {
            get { return _searchQuery; }
            set { SetProperty(ref _searchQuery, value); }
        }

        private List<MovieDto> _searchResults;
        public List<MovieDto> SearchResults
        {
            get { return _searchResults; }
            set { SetProperty(ref _searchResults, value); }
        }

        public Action ShowSearchResultsDelegate { get; set; }

        public ICommand Search
        {
            get
            {
                return new MvxCommand(SearchForMovies);
            }
        }

        public async void SearchForMovies()
        {
            SearchResults.Clear();
            var searchResponse = await _movieClient.SearchMoviesAsync(SearchQuery);
            if (searchResponse.IsOk)
            {
                SearchResults = searchResponse.Body.Results;
                ShowSearchResultsDelegate?.Invoke();
            }
        }

        public void SelectMovie(int id)
        {
            var movie = SearchResults.Where(x => x.Id == id).FirstOrDefault();
            if (movie != null)
            {
                ShowViewModel<DetailViewModel>(new SelectedMovie { MovieId = movie.Id });
            }
        }
    }
}