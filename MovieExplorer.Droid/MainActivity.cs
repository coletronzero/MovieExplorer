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

namespace MovieExplorer.Droid
{
    [Activity(Label = "MovieExplorer", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private HomeViewModel _viewModel;

        private LinearLayout _topRatedMoviesLayout;
        private LinearLayout _popularMoviesLayout;
        private LinearLayout _nowPlayingMoviesLayout;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            Button btnSearch = FindViewById<Button>(Resource.Id.SearchButton);
            _topRatedMoviesLayout = FindViewById<LinearLayout>(Resource.Id.top_rated_movies);
            _popularMoviesLayout = FindViewById<LinearLayout>(Resource.Id.popular_movies);
            _nowPlayingMoviesLayout = FindViewById<LinearLayout>(Resource.Id.now_playing_movies);

            ApplicationMaster.Init();

            _viewModel = ApplicationMaster.ViewModels.Home;

            _viewModel.AddTopRatedMoviesToView = AddTopRatedMovies;
            _viewModel.AddPopularMoviesToView = AddPopularMovies;
            _viewModel.AddNowPlayingMoviesToView = AddNowPlayingMovies;

            InitializeMoviesAsync();
        }

        private async Task InitializeMoviesAsync()
        {
            await _viewModel.InitializeMovieListsAsync();
        }

        private async void AddTopRatedMovies(List<Client.Models.Movie> movies)
        {
            if(movies != null && movies.Count > 0)
            {
                foreach (var movie in movies)
                {
                    if(movie != null)
                    {
                        MovieImageView imageView = new MovieImageView(this);
                        imageView.MovieId = movie.Id;
                        imageView.SetImageResource(Resource.Drawable.movie);

                        Bitmap moviePoster = await GetImageBitmapFromUrl("http://image.tmdb.org/t/p/w500" + movie.PosterPath);
                        imageView.SetImageBitmap(moviePoster);

                        LinearLayout.LayoutParams imgViewParams = new LinearLayout.LayoutParams(300, 400);
                        imgViewParams.SetMargins(0, 0, 20, 0);
                        imageView.LayoutParameters = imgViewParams;
                        imageView.SetScaleType(Android.Widget.ImageView.ScaleType.CenterCrop);

                        imageView.Click += imageView_Click;

                        _topRatedMoviesLayout.AddView(imageView);
                    }
                }
            }
        }

        private async void AddPopularMovies(List<Client.Models.Movie> movies)
        {
            if (movies != null && movies.Count > 0)
            {
                foreach (var movie in movies)
                {
                    if(movie != null)
                    {
                        MovieImageView imageView = new MovieImageView(this);
                        imageView.MovieId = movie.Id;
                        imageView.SetImageResource(Resource.Drawable.movie);

                        Bitmap moviePoster = await GetImageBitmapFromUrl("http://image.tmdb.org/t/p/w500" + movie.PosterPath);
                        imageView.SetImageBitmap(moviePoster);

                        LinearLayout.LayoutParams imgViewParams = new LinearLayout.LayoutParams(300, 400);
                        imgViewParams.SetMargins(0, 0, 20, 0);
                        imageView.LayoutParameters = imgViewParams;
                        imageView.SetScaleType(Android.Widget.ImageView.ScaleType.CenterCrop);

                        imageView.Click += imageView_Click;

                        _popularMoviesLayout.AddView(imageView);
                    }
                }
            }
        }

        private async void AddNowPlayingMovies(List<Client.Models.Movie> movies)
        {
            if (movies != null && movies.Count > 0)
            {
                foreach (var movie in movies)
                {
                    if(movie != null)
                    {
                        MovieImageView imageView = new MovieImageView(this);
                        imageView.MovieId = movie.Id;
                        imageView.SetImageResource(Resource.Drawable.movie);

                        Bitmap moviePoster = await GetImageBitmapFromUrl("http://image.tmdb.org/t/p/w500" + movie.PosterPath);
                        imageView.SetImageBitmap(moviePoster);

                        LinearLayout.LayoutParams imgViewParams = new LinearLayout.LayoutParams(300, 400);
                        imgViewParams.SetMargins(0, 0, 20, 0);
                        imageView.LayoutParameters = imgViewParams;
                        imageView.SetScaleType(Android.Widget.ImageView.ScaleType.CenterCrop);

                        imageView.Click += imageView_Click;

                        _nowPlayingMoviesLayout.AddView(imageView);
                    }
                }
            }
        }

        private async Task<Bitmap> GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;

            try
            {
                using (var webClient = new WebClient())
                {
                    var imageBytes = webClient.DownloadData(url);
                    if (imageBytes != null && imageBytes.Length > 0)
                    {
                        imageBitmap = await BitmapFactory.DecodeByteArrayAsync(imageBytes, 0, imageBytes.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                var test = ex;
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