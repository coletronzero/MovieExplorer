using MovieExplorer.Client.Messages;
using MovieExplorer.Client.Models;
using MovieExplorer.Client.NavigationParameters;
using MovieExplorer.Client.Services;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using MvvmCross.Plugins.File;
using MvvmCross.Plugins.Messenger;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieExplorer.Client.ViewModels
{
    public class FavoritesViewModel : MvxViewModel
    {
        private IMovieSharpService _movieClient;
        private readonly IMvxFileStore _fileStore;
        private readonly IMvxJsonConverter _jsonConverter;
        private readonly string _filePath;
        private readonly MvxSubscriptionToken _favoriteToken;
        private readonly MvxSubscriptionToken _unfavoriteToken;

        private List<int> FavoriteMovieIdList;

        public FavoritesViewModel(IMovieSharpService movieClient, IMvxFileStore fileStore, IMvxJsonConverter jsonConverter, IMvxMessenger messenger)
        {
            _movieClient = movieClient;
            _fileStore = fileStore;
            _jsonConverter = jsonConverter;
            _filePath = _fileStore.PathCombine(App.FILE_SUBDIRECTORY, App.FILE_NAME);
            _fileStore.EnsureFolderExists(App.FILE_SUBDIRECTORY);

            FavoriteMovies = new List<MovieDto>();
            LoadFavoriteMoviesList();
            RetrieveFavoriteMoviesAsync();

            // Subscribe to Favorite/Unfavorite Events
            _favoriteToken = messenger.Subscribe<MovieFavoritedMessage>(OnMovieFavorited);
            _unfavoriteToken = messenger.Subscribe<MovieUnfavoritedMessage>(OnMovieUnfavorited);
        }

        private void LoadFavoriteMoviesList()
        {
            try
            {
                // Attempt to Load List of Favorite Movies from Storage
                FavoriteMovieIdList = new List<int>();
                string txt;
                if (_fileStore.TryReadTextFile(_filePath, out txt))
                {
                    FavoriteMovieIdList = _jsonConverter.DeserializeObject<List<int>>(txt);
                }
            }
            catch (Exception ex)
            {
                Mvx.Trace(MvxTraceLevel.Error, "Exception was thrown during FavoritesViewModel LoadFavoriteMoviesList", ex);
            }
        }

        private async Task RetrieveFavoriteMoviesAsync()
        {
            try
            {
                FavoriteMovies.Clear();
                foreach (var movieId in FavoriteMovieIdList)
                {
                    var movieResponse = await _movieClient.GetMovieAsync(movieId);
                    if (movieResponse.IsOk)
                    {
                        FavoriteMovies.Add(movieResponse.Body);
                    }
                }

                ShowFavoritesDelegate?.Invoke();
            }
            catch (Exception ex)
            {
                Mvx.Trace(MvxTraceLevel.Error, "Exception was thrown during FavoritesViewModel RetrieveFavoriteMoviesAsync", ex);
            }
        }

        private async void OnMovieFavorited(MovieFavoritedMessage movie)
        {
            if (!FavoriteMovieIdList.Contains(movie.MovieId))
            {
                FavoriteMovieIdList.Add(movie.MovieId);
                await RetrieveFavoriteMoviesAsync();
            }
        }

        private async void OnMovieUnfavorited(MovieUnfavoritedMessage movie)
        {
            if (FavoriteMovieIdList.Contains(movie.MovieId))
            {
                FavoriteMovieIdList.Remove(movie.MovieId);
                await RetrieveFavoriteMoviesAsync();
            }
        }

        private List<MovieDto> _favoriteMovies;
        public List<MovieDto> FavoriteMovies
        {
            get { return _favoriteMovies; }
            set { SetProperty(ref _favoriteMovies, value); }
        }

        public Action ShowFavoritesDelegate { get; set; }

        public void SelectMovie(int movieId)
        {
            ShowViewModel<DetailViewModel>(new SelectedMovie { MovieId = movieId });
        }
    }
}