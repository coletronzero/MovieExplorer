using System;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;

namespace MovieExplorer.Droid.Controls
{
    public class SearchResultViewHolder : RecyclerView.ViewHolder
    {
        public ImageView MoviePhoto { get; set; }
        public TextView MovieName { get; set; }

        public SearchResultViewHolder(View itemView, Action<int> listener) : base(itemView)
        {
            MoviePhoto = itemView.FindViewById<ImageView>(Resource.Id.searchResult_MovieImage);
            MovieName = itemView.FindViewById<TextView>(Resource.Id.searchResult_MovieTitle);

            itemView.Click += (sender, e) => listener(Position);
        }
    }
}