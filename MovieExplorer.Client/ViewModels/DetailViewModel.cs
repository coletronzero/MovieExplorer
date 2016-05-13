using System;
using MovieExplorer.Client.Models;
using MovieExplorer.Client.Services;
using MvvmCross.Core.ViewModels;
using MovieExplorer.Client.NavigationParameters;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using MvvmCross.Platform.Platform;
using MvvmCross.Plugins.File;
using System.Windows.Input;
using MvvmCross.Plugins.Messenger;
using MovieExplorer.Client.Messages;
using MvvmCross.Platform;

namespace MovieExplorer.Client.ViewModels
{
    public class DetailViewModel : MvxViewModel
    {
        private IMovieSharpService _movieClient;
        private readonly IMvxFileStore _fileStore;
        private readonly IMvxJsonConverter _jsonConverter;
        private readonly string _filePath;
        private IMvxMessenger _messenger;

        private List<int> FavoriteMovieIdList;

        public DetailViewModel(IMovieSharpService movieClient, IMvxFileStore fileStore, IMvxJsonConverter jsonConverter, IMvxMessenger messenger)
        {
            _movieClient = movieClient;
            _fileStore = fileStore;
            _jsonConverter = jsonConverter;
            _filePath = _fileStore.PathCombine(App.FILE_SUBDIRECTORY, App.FILE_NAME);
            _fileStore.EnsureFolderExists(App.FILE_SUBDIRECTORY);
            _messenger = messenger;

            LoadFavoriteMovies();
        }

        public async void Init(SelectedMovie parameters)
        {
            try
            {
                var movieResponse = await _movieClient.GetMovieAsync(parameters.MovieId);
                if (movieResponse.IsOk)
                {
                    SelectedMovie = movieResponse.Body;

                    SetFavoritesButtonText();
                }
            }
            catch (Exception ex)
            {
                Mvx.Trace(MvxTraceLevel.Error, "Exception was thrown during DetailViewModel Init", ex);
            }
        }

        private void LoadFavoriteMovies()
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
                Mvx.Trace(MvxTraceLevel.Error, "Exception was thrown during DetailViewModel LoadFavoriteMovies", ex);
            }
        }

        private string _favoriteButtonText;
        public string FavoriteButtonText
        {
            get { return _favoriteButtonText; }
            set { SetProperty(ref _favoriteButtonText, value); }
        }

        private void SetFavoritesButtonText()
        {
            // Set Favorite Button Text
            if (FavoriteMovieIdList != null)
            {
                if (FavoriteMovieIdList.Contains(SelectedMovie.Id))
                {
                    FavoriteButtonText = "Unfavorite";
                }
                else
                {
                    FavoriteButtonText = "Favorite";
                }
            }
            else
            {
                // Shouldn't happen, but just default button text to Favorite
                FavoriteButtonText = "Favorite";
            }
        }

        public Action MovieSelectedDelegate { get; set; }

        private MovieDto _selectedMovie;
        public MovieDto SelectedMovie
        {
            get { return _selectedMovie; }
            set
            {
                if (SetProperty(ref _selectedMovie, value))
                {
                    PosterUrl = App.IMAGE_PREFIX + _selectedMovie.PosterPath;
                    Title = _selectedMovie.Title;
                    ReleaseDate = "Release Date: " + _selectedMovie.ReleaseDate;
                    Rating = _selectedMovie.VoteAverage.ToString() + " Stars";
                    Votes = "(from " + _selectedMovie.VoteCount.ToString() + " votes)";
                    Overview = _selectedMovie.Overview;
                    SetFavoritesButtonText();

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
            try
            {
                if (SelectedMovie != null && SelectedMovie.Id > 0)
                {
                    var similarMoviesResponse = await _movieClient.GetSimilarMoviesAsync(SelectedMovie.Id);
                    if (similarMoviesResponse.IsOk)
                    {
                        _similarMovies = similarMoviesResponse.Body.Results;
                    }
                }
            }
            catch (Exception ex)
            {
                Mvx.Trace(MvxTraceLevel.Error, "Exception was thrown during DetailViewModel GetSimilarMoviesAsync", ex);
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

        public ICommand FavoriteButtonClick
        {
            get
            {
                return new MvxCommand(() =>
                {
                    try
                    {
                        if (FavoriteMovieIdList.Contains(SelectedMovie.Id))
                        {
                            // Remove From List
                            FavoriteMovieIdList.Remove(SelectedMovie.Id);

                            var json = _jsonConverter.SerializeObject(FavoriteMovieIdList);
                            _fileStore.WriteFile(_filePath, json);
                            FavoriteButtonText = "Favorite";

                            var message = new MovieUnfavoritedMessage(this, SelectedMovie.Id);
                            _messenger.Publish(message);
                        }
                        else
                        {
                            // Add To List
                            FavoriteMovieIdList.Add(SelectedMovie.Id);

                            var json = _jsonConverter.SerializeObject(FavoriteMovieIdList);
                            _fileStore.WriteFile(_filePath, json);
                            FavoriteButtonText = "Unfavorite";

                            var message = new MovieFavoritedMessage(this, SelectedMovie.Id);
                            _messenger.Publish(message);
                        }
                    }
                    catch (Exception ex)
                    {
                        Mvx.Trace(MvxTraceLevel.Error, "Exception was thrown during DetailViewModel FavoriteButtonClick", ex);
                    }
                });
            }
        }
    }
}