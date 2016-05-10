using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using MovieExplorer.Droid.Controls;
using MovieExplorer.Client.ViewModels;
using MvvmCross.Droid.Views;

namespace MovieExplorer.Droid.Views
{
    [Activity(Label = "Movie Explorer")]
    public class SearchActivity : MvxActivity
    {
        protected SearchViewModel _viewModel
        {
            get { return ViewModel as SearchViewModel; }
        }

        private RecyclerView _searchResultsView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.SearchView);
            }
            catch (Exception ex)
            {
                var test = ex;
            }
            
            _searchResultsView = FindViewById<RecyclerView>(Resource.Id.searchResults_recyclerView);
            _searchResultsView.SetLayoutManager(new LinearLayoutManager(this, LinearLayoutManager.Vertical, false));

            _viewModel.ShowSearchResultsDelegate = ShowSearchResults;
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