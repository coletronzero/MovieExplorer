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

namespace MovieExplorer.Droid
{
    [Activity(Label = "DetailActivity")]
    public class DetailActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Detail);

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