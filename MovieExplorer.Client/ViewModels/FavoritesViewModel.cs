using MovieExplorer.Client.Models;
using MovieExplorer.Client.NavigationParameters;
using MovieExplorer.Client.Services;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Platform;
using MvvmCross.Plugins.File;
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

        private List<int> FavoriteMovieIdList;

        public FavoritesViewModel(IMovieSharpService movieClient, IMvxFileStore fileStore, IMvxJsonConverter jsonConverter)
        {
            _movieClient = movieClient;
            _fileStore = fileStore;
            _jsonConverter = jsonConverter;
            _filePath = _fileStore.PathCombine("SubDir", "FavoriteMovies.txt");
            _fileStore.EnsureFolderExists("SubDir");

            FavoriteMovies = new List<MovieDto>();
            LoadFavoriteMovies();
        }

        private async Task LoadFavoriteMovies()
        {
            try
            {
                // Attempt to Load List of Favorite Movies from Storage
                FavoriteMovieIdList = new List<int>();
                string txt;
                if (_fileStore.TryReadTextFile(_filePath, out txt))
                {
                    FavoriteMovieIdList = _jsonConverter.DeserializeObject<List<int>>(txt);

                    foreach(var movieId in FavoriteMovieIdList)
                    {
                        var movieResponse = await _movieClient.GetMovieAsync(movieId);
                        if (movieResponse.IsOk)
                        {
                            FavoriteMovies.Add(movieResponse.Body);
                        }
                    }

                    ShowFavoritesDelegate?.Invoke();
                }
            }
            catch (Exception ex)
            {
                //TODO: Log Exception
            }
        }

        public List<MovieDto> FavoriteMovies { get; set; }

        public Action ShowFavoritesDelegate { get; set; }

        public void SelectMovie(int movieId)
        {
            ShowViewModel<DetailViewModel>(new SelectedMovie { MovieId = movieId });
        }
    }
}