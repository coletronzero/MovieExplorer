using System;
using Android.App;
using Android.OS;
using MovieExplorer.Client.ViewModels;
using Android.Support.V7.Widget;
using MovieExplorer.Droid.Controls;
using Android.Views;
using MvvmCross.Droid.Support.V7.AppCompat;
using Android.Support.V4.View;
using Java.Interop;
using Android.Support.V4.Widget;
using System.Threading.Tasks;

namespace MovieExplorer.Droid.Views
{
    [Activity(Label = "Movie Explorer")]
    public class MainActivity : MvxAppCompatActivity
    {
        protected MainViewModel _viewModel
        {
            get { return ViewModel as MainViewModel; }
        }

        private RecyclerView _topRatedRecyclerView;
        private RecyclerView _popularRecyclerView;
        private RecyclerView _nowPlayingRecyclerView;

        private SearchView _searchView;
        private IMenuItem _searchMenuItem;

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

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            //Toolbar will now take on default actionbar characteristics
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "Movie Explorer";

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

            
            var refresher = FindViewById<SwipeRefreshLayout>(Resource.Id.refresher);
            refresher.Refresh += async delegate {
                // TODO: Refresh content
                await Task.Delay(2000);
                refresher.Refreshing = false;
            };
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.main, menu);

            _searchMenuItem = menu.FindItem(Resource.Id.main_menu_action_search);

            var searchView = MenuItemCompat.GetActionView(_searchMenuItem);
            _searchView = searchView.JavaCast<SearchView>();

            _searchView.QueryTextSubmit += _searchView_QueryTextSubmit;

            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch(item.ItemId)
            {
                // Show Favorites
                case Resource.Id.action_favorite:
                    return _viewModel.ShowFavorites();
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        private void _searchView_QueryTextSubmit(object sender, SearchView.QueryTextSubmitEventArgs e)
        {
            _searchView.SetQuery(string.Empty, false);
            _searchView.ClearFocus();
            _searchMenuItem.CollapseActionView();
            _viewModel.GoToSearch(e.Query);
        }

        private void OnTopRatedItemClick(object sender, int movieId)
        {
            _viewModel.SelectMovie(movieId);
        }

        private void OnPopularItemClick(object sender, int movieId)
        {
            _viewModel.SelectMovie(movieId);
        }

        private void OnNowPlayingItemClick(object sender, int movieId)
        {
            _viewModel.SelectMovie(movieId);
        }
    }
}