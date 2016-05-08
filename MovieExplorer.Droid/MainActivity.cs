using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Graphics;
using Java.IO;
using System.Net;
using System.Threading.Tasks;
using MovieExplorer.Client;
using MovieExplorer.Client.ViewModels;
using System.Collections.Generic;
using Android.Support.V7.Widget;
using MovieExplorer.Droid.Controls;

namespace MovieExplorer.Droid
{
    [Activity(Label = "MovieExplorer", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private HomeViewModel _viewModel;

        private RecyclerView _topRatedRecyclerView;
        private RecyclerView _popularRecyclerView;
        private RecyclerView _nowPlayingRecyclerView;

        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            //Button btnSearch = FindViewById<Button>(Resource.Id.SearchButton);

            ApplicationMaster.Init();

            _viewModel = ApplicationMaster.ViewModels.Home;

            // Create a reference to our RecyclerView and set the layout manager;
            _topRatedRecyclerView = FindViewById<RecyclerView>(Resource.Id.topRated_recyclerView);
            _topRatedRecyclerView.SetLayoutManager(new LinearLayoutManager(this, LinearLayoutManager.Horizontal, false));

            // Get our movie data.
            var movies = await _viewModel.GetTopRatedMoviesAsync();

            // Create the adapter for the RecyclerView with movie data, and set
            // the adapter. Also, wire an event handler for when the user taps on each
            // individual item.
            MovieRecyclerViewAdapter topRatedAdapter = new MovieRecyclerViewAdapter(movies, this.Resources);
            topRatedAdapter.ItemClick += OnItemClick;
            _topRatedRecyclerView.SetAdapter(topRatedAdapter);

            // Popular List
            _popularRecyclerView = FindViewById<RecyclerView>(Resource.Id.popular_recyclerView);
            _popularRecyclerView.SetLayoutManager(new LinearLayoutManager(this, LinearLayoutManager.Horizontal, false));

            var popularMovies = await _viewModel.GetPopularMoviesAsync();

            MovieRecyclerViewAdapter popularAdapter = new MovieRecyclerViewAdapter(popularMovies, this.Resources);
            popularAdapter.ItemClick += OnItemClick;
            _popularRecyclerView.SetAdapter(popularAdapter);

            // Now Playing List
            _nowPlayingRecyclerView = FindViewById<RecyclerView>(Resource.Id.nowPlaying_recyclerView);
            _nowPlayingRecyclerView.SetLayoutManager(new LinearLayoutManager(this, LinearLayoutManager.Horizontal, false));

            var nowPlayingMovies = await _viewModel.GetNowPlayingMoviesAsync();

            MovieRecyclerViewAdapter nowPlayingAdapter = new MovieRecyclerViewAdapter(nowPlayingMovies, this.Resources);
            nowPlayingAdapter.ItemClick += OnItemClick;
            _nowPlayingRecyclerView.SetAdapter(nowPlayingAdapter);
        }

        private void OnItemClick(object sender, int e)
        {
            try
            {
                MovieRecyclerViewAdapter view = (MovieRecyclerViewAdapter)sender;
                if (view != null)
                {
                    // TODO: Select Movie in View Model that Details View Binds to

                    var intent = new Intent(this, typeof(DetailActivity));
                    StartActivity(intent);
                }
            }
            catch (Exception ex)
            {
                // TODO: Log Exception
            }
        }
    }
}