using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using MovieExplorer.Client.ViewModels;
using MovieExplorer.Droid.Controls;
using MvvmCross.Droid.Views;
using System;

namespace MovieExplorer.Droid.Views
{
    [Activity(Label = "FavoritesActivity")]
    public class FavoritesActivity : MvxActivity
    {
        protected FavoritesViewModel _viewModel
        {
            get { return ViewModel as FavoritesViewModel; }
        }

        private RecyclerView _favoritesView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.FavoritesView);
            }
            catch (Exception ex)
            {
                var test = ex;
            }

            _favoritesView = FindViewById<RecyclerView>(Resource.Id.favorites_recyclerView);
            _favoritesView.SetLayoutManager(new LinearLayoutManager(this, LinearLayoutManager.Vertical, false));

            _viewModel.ShowFavoritesDelegate = ShowFavorites;
        }

        private void ShowFavorites()
        {
            if (_viewModel.FavoriteMovies != null)
            {
                if (_favoritesView != null)
                {
                    MovieRecyclerViewAdapter similarAdapter = new MovieRecyclerViewAdapter(_viewModel.FavoriteMovies, this.Resources, true);
                    similarAdapter.ItemClick += OnResultsItemClick;
                    _favoritesView.SetAdapter(similarAdapter);
                }
            }
        }

        private void OnResultsItemClick(object sender, int movieId)
        {
            _viewModel.SelectMovie(movieId);
        }
    }
}