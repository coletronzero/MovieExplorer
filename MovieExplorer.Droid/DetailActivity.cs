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
using MvvmCross.Droid.Views;
using MovieExplorer.Droid.Helpers;
using MovieExplorer.Droid.Controls;
using MovieExplorer.Client;
using Android.Graphics;
using MovieExplorer.Client.ViewModels;

namespace MovieExplorer.Droid
{
    [Activity(Label = "DetailActivity")]
    public class DetailActivity : MvxActivity
    {
        protected DetailViewModel _viewModel
        {
            get { return ViewModel as DetailViewModel; }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.Detail);
            }
            catch(Exception ex)
            {
                var test = ex;
            }

            if(_viewModel.SelectedMovie != null)
            {
                ImageView movieImageView = FindViewById<ImageView>(Resource.Id.movieImage);
                var task = new BitmapDownloaderTask(movieImageView);
                movieImageView.SetImageDrawable(new DownloadedDrawable(task, Color.Gray));
                movieImageView.SetMinimumHeight(300);
                task.Execute("http://image.tmdb.org/t/p/w500" + _viewModel.SelectedMovie.PosterPath);
            }
            
            LinearLayout similarLayout = FindViewById<LinearLayout>(Resource.Id.similar_movies);

            for (int i = 0; i < 10; i++)
            {
                MovieImageView imageView = new MovieImageView(this);
                imageView.MovieId = i;
                imageView.SetImageResource(Resource.Drawable.movie);
                LinearLayout.LayoutParams imgViewParams = new LinearLayout.LayoutParams(300, 400);
                imgViewParams.SetMargins(0, 0, 20, 0);
                imageView.LayoutParameters = imgViewParams;
                imageView.SetScaleType(Android.Widget.ImageView.ScaleType.CenterCrop);

                imageView.Click += imageView_Click;

                similarLayout.AddView(imageView);
            }
        }

        void imageView_Click(object sender, EventArgs e)
        {
            MovieImageView view = (MovieImageView)sender;
            if (view != null)
            {

            }
        }
    }
}