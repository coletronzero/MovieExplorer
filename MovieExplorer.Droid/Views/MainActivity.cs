using System;
using Android.App;
using Android.OS;
using MovieExplorer.Client.ViewModels;
using Android.Support.V7.Widget;
using MovieExplorer.Droid.Controls;
using MvvmCross.Droid.Views;
using Android.Views;

namespace MovieExplorer.Droid.Views
{
    [Activity(Label = "Movie Explorer")]
    public class MainActivity : MvxActivity
    {
        protected MainViewModel _viewModel
        {
            get { return ViewModel as MainViewModel; }
        }

        private RecyclerView _topRatedRecyclerView;
        private RecyclerView _popularRecyclerView;
        private RecyclerView _nowPlayingRecyclerView;

        protected override async void OnCreate(Bundle bundle)
        {
            try
            {
                base.OnCreate(bundle);
                SetContentView(Resource.Layout.MainView);
            }
            catch (Exception ex)
            {
                var test = ex;
            }

            //Button btnSearch = FindViewById<Button>(Resource.Id.SearchButton);

            // Create a reference to our RecyclerView and set the layout manager;
            _topRatedRecyclerView = FindViewById<RecyclerView>(Resource.Id.topRated_recyclerView);
            _topRatedRecyclerView.SetLayoutManager(new LinearLayoutManager(this, LinearLayoutManager.Horizontal, false));

            // Get our movie data.
            var movies = await _viewModel.GetTopRatedMoviesAsync();

            // Create the adapter for the RecyclerView with movie data, and set
            // the adapter. Also, wire an event handler for when the user taps on each
            // individual item.
            MovieRecyclerViewAdapter topRatedAdapter = new MovieRecyclerViewAdapter(movies, this.Resources);
            topRatedAdapter.ItemClick += OnTopRatedItemClick;
            _topRatedRecyclerView.SetAdapter(topRatedAdapter);

            // Popular List
            _popularRecyclerView = FindViewById<RecyclerView>(Resource.Id.popular_recyclerView);
            _popularRecyclerView.SetLayoutManager(new LinearLayoutManager(this, LinearLayoutManager.Horizontal, false));

            var popularMovies = await _viewModel.GetPopularMoviesAsync();

            MovieRecyclerViewAdapter popularAdapter = new MovieRecyclerViewAdapter(popularMovies, this.Resources);
            popularAdapter.ItemClick += OnPopularItemClick;
            _popularRecyclerView.SetAdapter(popularAdapter);

            // Now Playing List
            _nowPlayingRecyclerView = FindViewById<RecyclerView>(Resource.Id.nowPlaying_recyclerView);
            _nowPlayingRecyclerView.SetLayoutManager(new LinearLayoutManager(this, LinearLayoutManager.Horizontal, false));

            var nowPlayingMovies = await _viewModel.GetNowPlayingMoviesAsync();

            MovieRecyclerViewAdapter nowPlayingAdapter = new MovieRecyclerViewAdapter(nowPlayingMovies, this.Resources);
            nowPlayingAdapter.ItemClick += OnNowPlayingItemClick;
            _nowPlayingRecyclerView.SetAdapter(nowPlayingAdapter);
        }

        private void OnTopRatedItemClick(object sender, int movieId)
        {
            _viewModel.SelectTopRatedMovie(movieId);
        }

        private void OnPopularItemClick(object sender, int movieId)
        {
            _viewModel.SelectPopularMovie(movieId);
        }

        private void OnNowPlayingItemClick(object sender, int movieId)
        {
            _viewModel.SelectNowPlayingMovie(movieId);
        }
    }
}