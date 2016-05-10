using Android.OS;
using MovieExplorer.Client.Messages;
using MovieExplorer.Client.Models;
using MovieExplorer.Client.NavigationParameters;
using MovieExplorer.Client.Services;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MovieExplorer.Client.ViewModels
{
    public class MainViewModel : MvxViewModel
    {
        private IMovieSharpService _movieClient;
        public MainViewModel(IMovieSharpService movieClient)
        {
            _movieClient = movieClient;
        }

        public ICommand GoToSearch
        {
            get
            {
                return new MvxCommand(() =>
                {
                    ShowViewModel<SearchViewModel>();
                });
            }
        }

        private List<MovieDto> _topRatedMovies;
        public async Task<List<MovieDto>> GetTopRatedMoviesAsync()
        {
            _topRatedMovies = new List<MovieDto>();
            var topRatedMoviesResponse = await _movieClient.GetTopRatedMoviesAsync();
            if (topRatedMoviesResponse.IsOk)
            {
                _topRatedMovies = topRatedMoviesResponse.Body.Results;
            }
            return _topRatedMovies;
        }

        public void SelectTopRatedMovie(int id)
        {
            var movie = _topRatedMovies.Where(x => x.Id == id).FirstOrDefault();
            if (movie != null)
            {
                ShowViewModel<DetailViewModel>(new SelectedMovie{ MovieId = movie.Id });
            }
        }

        private List<MovieDto> _popularMovies;
        public async Task<List<MovieDto>> GetPopularMoviesAsync()
        {
            _popularMovies = new List<MovieDto>();
            var popularMoviesResponse = await _movieClient.GetPopularMoviesAsync();
            if (popularMoviesResponse.IsOk)
            {
                _popularMovies = popularMoviesResponse.Body.Results;
            }
            return _popularMovies;
        }

        public void SelectPopularMovie(int id)
        {
            var movie = _popularMovies.Where(x => x.Id == id).FirstOrDefault();
            if (movie != null)
            {
                ShowViewModel<DetailViewModel>(new SelectedMovie { MovieId = movie.Id });
            }
        }

        private List<MovieDto> _nowPlayingMovies;
        public async Task<List<MovieDto>> GetNowPlayingMoviesAsync()
        {
            _nowPlayingMovies = new List<MovieDto>();
            var nowPlayingMoviesResponse = await _movieClient.GetNowPlayingMoviesAsync();
            if (nowPlayingMoviesResponse.IsOk)
            {
                _nowPlayingMovies = nowPlayingMoviesResponse.Body.Results;
            }
            return _nowPlayingMovies;
        }

        public void SelectNowPlayingMovie(int id)
        {
            var movie = _nowPlayingMovies.Where(x => x.Id == id).FirstOrDefault();
            if (movie != null)
            {
                ShowViewModel<DetailViewModel>(new SelectedMovie { MovieId = movie.Id });
            }
        }
    }
}