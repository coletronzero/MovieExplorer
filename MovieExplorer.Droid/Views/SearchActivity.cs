using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Support.V7.Widget;
using Android.Views;
using MovieExplorer.Client.ViewModels;
using MovieExplorer.Droid.Controls;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using System;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace MovieExplorer.Droid.Views
{
    [Activity(ScreenOrientation = ScreenOrientation.Portrait)]
    public class SearchActivity : MvxAppCompatActivity
    {
        protected SearchViewModel _viewModel
        {
            get { return ViewModel as SearchViewModel; }
        }

        private RecyclerView _searchResultsView;
        private SearchView _searchView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.SearchView);
            }
            catch (Exception ex)
            {
                Mvx.Trace(MvxTraceLevel.Error, "Exception was thrown Inflating SearchView", ex);
            }

            var toolbar = FindViewById<Toolbar>(Resource.Id.search_toolbar);
            //Toolbar will now take on default actionbar characteristics
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "Search";
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);

            _searchResultsView = FindViewById<RecyclerView>(Resource.Id.searchResults_recyclerView);
            _searchResultsView.SetLayoutManager(new LinearLayoutManager(this, LinearLayoutManager.Vertical, false));

            _viewModel.ShowSearchResultsDelegate = ShowSearchResults;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.search, menu);

            var item = menu.FindItem(Resource.Id.search_menu_action_search);

            var searchView = MenuItemCompat.GetActionView(item);
            _searchView = searchView.JavaCast<SearchView>();

            _searchView.QueryTextSubmit += _searchView_QueryTextSubmit;

            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
                Finish();

            return base.OnOptionsItemSelected(item);
        }

        private void _searchView_QueryTextSubmit(object sender, SearchView.QueryTextSubmitEventArgs e)
        {
            _searchView.ClearFocus();
            _viewModel.SearchQuery = e.Query;
            _viewModel.SearchForMovies();
        }

        private void ShowSearchResults()
        {
            if (_viewModel.SearchResults != null)
            {
                if (_searchResultsView != null)
                {
                    MovieRecyclerViewAdapter similarAdapter = new MovieRecyclerViewAdapter(_viewModel.SearchResults, this.Resources, true);
                    similarAdapter.ItemClick += OnResultsItemClick;
                    _searchResultsView.SetAdapter(similarAdapter);
                }
            }
        }

        private void OnResultsItemClick(object sender, int movieId)
        {
            _viewModel.SelectMovie(movieId);
        }
    }
}