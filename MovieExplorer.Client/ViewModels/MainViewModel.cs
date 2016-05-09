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

namespace MovieExplorer.Client.ViewModels
{
    public class MainViewModel : MvxViewModel
    {
        private IMovieSharpService _movieClient;
        private IMvxMessenger _messenger;
        public MainViewModel(IMovieSharpService movieClient, IMvxMessenger messenger)
        {
            _movieClient = movieClient;
            _messenger = messenger;
        }

        private List<Movie> _topRatedMovies;
        public async Task<List<Movie>> GetTopRatedMoviesAsync()
        {
            _topRatedMovies = new List<Movie>();
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

                var message = new SelectedMovieMessage(this, movie);
                _messenger.Publish(message);
            }
        }

        private List<Movie> _popularMovies;
        public async Task<List<Movie>> GetPopularMoviesAsync()
        {
            _popularMovies = new List<Movie>();
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

                var message = new SelectedMovieMessage(this, movie);
                _messenger.Publish(message);
            }
        }

        private List<Movie> _nowPlayingMovies;
        public async Task<List<Movie>> GetNowPlayingMoviesAsync()
        {
            _nowPlayingMovies = new List<Movie>();
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

                var message = new SelectedMovieMessage(this, movie);
                _messenger.Publish(message);
            }
        }
    }
}