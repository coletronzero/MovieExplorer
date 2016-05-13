using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using MovieExplorer.Client;
using MovieExplorer.Client.ViewModels;
using MovieExplorer.Droid.Controls;
using MovieExplorer.Droid.Helpers;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using System;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace MovieExplorer.Droid.Views
{
    [Activity(ScreenOrientation = ScreenOrientation.Portrait)]
    public class DetailActivity : MvxAppCompatActivity
    {
        protected DetailViewModel _viewModel
        {
            get { return ViewModel as DetailViewModel; }
        }

        private ImageView _movieImage;
        private RecyclerView _similarRecycler;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.DetailView);
            }
            catch (Exception ex)
            {
                Mvx.Trace(MvxTraceLevel.Error, "Exception was thrown Inflating DetailView", ex);
            }

            var toolbar = FindViewById<Toolbar>(Resource.Id.detail_toolbar);
            //Toolbar will now take on default actionbar characteristics
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "Detail";
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);

            _movieImage = FindViewById<ImageView>(Resource.Id.detailMoviePhoto);
            _similarRecycler = FindViewById<RecyclerView>(Resource.Id.similar_recyclerView);
            _similarRecycler.SetLayoutManager(new LinearLayoutManager(this, LinearLayoutManager.Horizontal, false));

            _viewModel.MovieSelectedDelegate = OnMovieSelected;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
                Finish();

            return base.OnOptionsItemSelected(item);
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
                    task.Execute(App.IMAGE_PREFIX + _viewModel.SelectedMovie.PosterPath);
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