using System;
using Android.App;
using Android.OS;
using Android.Widget;
using MvvmCross.Droid.Views;
using MovieExplorer.Droid.Helpers;
using MovieExplorer.Droid.Controls;
using Android.Graphics;
using MovieExplorer.Client.ViewModels;
using Android.Support.V7.Widget;
using Android.Views;

namespace MovieExplorer.Droid.Views
{
    [Activity(Label = "Movie Explorer")]
    public class DetailActivity : MvxActivity
    {
        protected DetailViewModel _viewModel
        {
            get { return ViewModel as DetailViewModel; }
        }

        private ImageView _movieImage;
        private RecyclerView _similarRecycler;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            ActionBar.SetHomeButtonEnabled(true);
            try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.Detail);
            }
            catch (Exception ex)
            {
                var test = ex;
            }

            _movieImage = FindViewById<ImageView>(Resource.Id.detailMoviePhoto);
            _similarRecycler = FindViewById<RecyclerView>(Resource.Id.similar_recyclerView);
            _similarRecycler.SetLayoutManager(new LinearLayoutManager(this, LinearLayoutManager.Horizontal, false));

            _viewModel.MovieSelectedDelegate = OnMovieSelected;
        }

        private async void OnMovieSelected()
        {
            if (_viewModel.SelectedMovie != null)
            {
                if (_movieImage != null)
                {
                    // Load Movie Image
                    var task = new BitmapDownloaderTask(_movieImage);
                    _movieImage.SetImageDrawable(new DownloadedDrawable(task, Color.Gray));
                    _movieImage.SetMinimumHeight(300);
                    task.Execute("http://image.tmdb.org/t/p/w500" + _viewModel.SelectedMovie.PosterPath);
                }

                if (_similarRecycler != null)
                {
                    // Load Similar Movies
                    var similarMovies = await _viewModel.GetSimilarMoviesAsync();

                    MovieRecyclerViewAdapter similarAdapter = new MovieRecyclerViewAdapter(similarMovies, this.Resources);
                    similarAdapter.ItemClick += OnSimilarItemClick;
                    _similarRecycler.SetAdapter(similarAdapter);
                }
            }
        }

        private void OnSimilarItemClick(object sender, int movieId)
        {
            _viewModel.SelectSimilarMovie(movieId);
        }
    }
}