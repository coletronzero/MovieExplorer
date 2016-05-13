using Android.App;
using Android.OS;
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
    [Activity(Label = "Favorites")]
    public class FavoritesActivity : MvxAppCompatActivity
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
                Mvx.Trace(MvxTraceLevel.Error, "Exception was thrown Inflating FavoritesView", ex);
            }

            var toolbar = FindViewById<Toolbar>(Resource.Id.favorites_toolbar);
            //Toolbar will now take on default actionbar characteristics
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "Favorites";
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);

            _favoritesView = FindViewById<RecyclerView>(Resource.Id.favorites_recyclerView);
            _favoritesView.SetLayoutManager(new LinearLayoutManager(this, LinearLayoutManager.Vertical, false));

            _viewModel.ShowFavoritesDelegate = ShowFavorites;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
                Finish();

            return base.OnOptionsItemSelected(item);
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