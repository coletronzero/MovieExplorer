using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace MovieExplorer.Droid
{
    [Activity(Label = "MovieExplorer", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        int count = 1;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.MyButton);

            button.Click += delegate { button.Text = string.Format("{0} clicks!", count++); };

            LinearLayout topRatedLayout = FindViewById<LinearLayout>(Resource.Id.top_rated_movies);
            LinearLayout popularLayout = FindViewById<LinearLayout>(Resource.Id.popular_movies);
            LinearLayout nowPlayingLayout = FindViewById<LinearLayout>(Resource.Id.now_playing_movies);

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

                topRatedLayout.AddView(imageView);
            }
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

                popularLayout.AddView(imageView);
            }
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

                nowPlayingLayout.AddView(imageView);
            }

            //var gridview = FindViewById<GridView>(Resource.Id.gridview);
            //gridview.Adapter = new ImageAdapter(this);

            //gridview.ItemClick += delegate(object sender, AdapterView.ItemClickEventArgs args)
            //{
            //    Toast.MakeText(this, args.Position.ToString(), ToastLength.Short).Show();
            //};
        }

        void imageView_Click(object sender, EventArgs e)
        {
            MovieImageView view = (MovieImageView)sender;
            if(view != null)
            {
                var intent = new Intent(this, typeof(DetailActivity));
                intent.PutExtra("movie_id", view.MovieId);
                StartActivity(intent);
            }
        }
    }
}