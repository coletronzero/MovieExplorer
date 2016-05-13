using System;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;

namespace MovieExplorer.Droid.Controls
{
    public class MovieItemViewHolder : RecyclerView.ViewHolder
    {
        public ImageView MoviePhoto { get; set; }
        //public TextView MovieName { get; set; }

        public MovieItemViewHolder(View itemView, Action<int> listener) : base(itemView)
        {
            MoviePhoto = itemView.FindViewById<ImageView>(Resource.Id.movie_Photo);
            //MovieName = itemView.FindViewById<TextView>(Resource.Id.movie_Name);

            itemView.Click += (sender, e) => listener(Position);
        }
    }
}