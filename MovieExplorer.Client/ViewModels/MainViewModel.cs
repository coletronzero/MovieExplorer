using MovieExplorer.Client.Models;
using MovieExplorer.Client.NavigationParameters;
using MovieExplorer.Client.Services;
using MvvmCross.Core.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using MvvmCross.Platform.Platform;
using MvvmCross.Platform;

namespace MovieExplorer.Client.ViewModels
{
    public class MainViewModel : MvxViewModel
    {
        private IMovieSharpService _movieClient;
        public MainViewModel(IMovieSharpService movieClient)
        {
            _movieClient = movieClient;
        }

        public void GoToSearch(string query)
        {
            ShowViewModel<SearchViewModel>(new SearchQuery { Query = query });
        }

        public Action RefreshTopRatedMovies { get; set; }
        public Action RefreshPopularMovies { get; set; }
        public Action RefreshNowPlayingMovies { get; set; }

        public async Task RefreshAllContent()
        {
            try
            {
                var topRatedMoviesResponse = await _movieClient.GetTopRatedMoviesAsync();
                if (topRatedMoviesResponse.IsOk)
                {
                    TopRatedMovies = topRatedMoviesResponse.Body.Results;
                    RefreshTopRatedMovies?.Invoke();
                }
                var popularMoviesResponse = await _movieClient.GetPopularMoviesAsync();
                if (popularMoviesResponse.IsOk)
                {
                    PopularMovies = popularMoviesResponse.Body.Results;
                    RefreshPopularMovies?.Invoke();
                }
                var nowPlayingMoviesResponse = await _movieClient.GetNowPlayingMoviesAsync();
                if (nowPlayingMoviesResponse.IsOk)
                {
                    NowPlayingMovies = nowPlayingMoviesResponse.Body.Results;
                    RefreshNowPlayingMovies?.Invoke();
                }
            }
            catch (Exception ex)
            {
                Mvx.Trace(MvxTraceLevel.Error, "Exception was thrown during MainViewModel RefreshAllContent", ex);
            }
        }

        private List<MovieDto> _topRatedMovies;
        public List<MovieDto> TopRatedMovies
        {
            get { return _topRatedMovies; }
            set { SetProperty(ref _topRatedMovies, value); }
        }

        private List<MovieDto> _popularMovies;
        public List<MovieDto> PopularMovies
        {
            get { return _popularMovies; }
            set { SetProperty(ref _popularMovies, value); }
        }

        private List<MovieDto> _nowPlayingMovies;
        public List<MovieDto> NowPlayingMovies
        {
            get { return _nowPlayingMovies; }
            set { SetProperty(ref _nowPlayingMovies, value); }
        }

        public void SelectMovie(int movieId)
        {
            ShowViewModel<DetailViewModel>(new SelectedMovie { MovieId = movieId });
        }

        public bool ShowFavorites()
        {
            return ShowViewModel<FavoritesViewModel>();
        }
    }
}