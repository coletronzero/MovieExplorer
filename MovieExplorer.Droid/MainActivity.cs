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

namespace MovieExplorer.Droid
{
    [Activity(Label = "MovieExplorer", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            ApplicationMaster.Init();

            Button btnSearch = FindViewById<Button>(Resource.Id.SearchButton);

            LinearLayout topRatedLayout = FindViewById<LinearLayout>(Resource.Id.top_rated_movies);

            LoadTopRatedMovies(topRatedLayout);

            LinearLayout popularLayout = FindViewById<LinearLayout>(Resource.Id.popular_movies);
            LinearLayout nowPlayingLayout = FindViewById<LinearLayout>(Resource.Id.now_playing_movies);

            //for (int i = 0; i < 10; i++)
            //{
            //    MovieImageView imageView = new MovieImageView(this);
            //    imageView.MovieId = i;
            //    imageView.SetImageResource(Resource.Drawable.movie);
            //    LinearLayout.LayoutParams imgViewParams = new LinearLayout.LayoutParams(300, 400);
            //    imgViewParams.SetMargins(0, 0, 20, 0);
            //    imageView.LayoutParameters = imgViewParams;
            //    imageView.SetScaleType(Android.Widget.ImageView.ScaleType.CenterCrop);

            //    imageView.Click += imageView_Click;

            //    nowPlayingLayout.AddView(imageView);
            //}
        }

        private void LoadTopRatedMovies(LinearLayout topRatedLayout)
        {
            // Iterate through the records returned.
            //var movies = _viewModel.TopRatedMovies;

            //foreach (var movie in movies)
            //{
            //    MovieImageView imageView = new MovieImageView(this);
            //    imageView.MovieId = movie.Id;
            //    imageView.SetImageResource(Resource.Drawable.movie);

            //    imageView.SetImageBitmap(GetImageBitmapFromUrl("http://image.tmdb.org/t/p/w500" + movie.PosterPath));

            //    LinearLayout.LayoutParams imgViewParams = new LinearLayout.LayoutParams(300, 400);
            //    imgViewParams.SetMargins(0, 0, 20, 0);
            //    imageView.LayoutParameters = imgViewParams;
            //    imageView.SetScaleType(Android.Widget.ImageView.ScaleType.CenterCrop);

            //    imageView.Click += imageView_Click;

            //    topRatedLayout.AddView(imageView);
            //}
        }

        private Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }

            return imageBitmap;
        }

        void imageView_Click(object sender, EventArgs e)
        {
            MovieImageView view = (MovieImageView)sender;
            if (view != null)
            {
                var intent = new Intent(this, typeof(DetailActivity));
                intent.PutExtra("movie_id", view.MovieId);
                StartActivity(intent);
            }
        }
    }
}