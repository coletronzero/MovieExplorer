using System;
using System.Collections.Generic;
using Android.Views;
using Android.Support.V7.Widget;
using Android.Content.Res;
using Android.Graphics;
using System.Threading.Tasks;
using System.Net;
using MovieExplorer.Droid.Helpers;
using MovieExplorer.Client.Models;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using MovieExplorer.Client;

namespace MovieExplorer.Droid.Controls
{
    public class MovieRecyclerViewAdapter : RecyclerView.Adapter
    {
        public static int ScreenWdith { get; set; }

        // Create an Event so that our our clients can act when a user clicks
        // on each individual item.
        public event EventHandler<int> ItemClick;

        private List<MovieDto> _movies;
        private bool _forSearchResults;

        public MovieRecyclerViewAdapter(List<MovieDto> movies, Resources resources, bool forSearchResults = false)
        {
            _movies = movies;
            _forSearchResults = forSearchResults;
        }

        // Must override, just like regular Adapters
        public override int ItemCount
        {
            get
            {
                return _movies.Count;
            }
        }

        // Must override, this inflates our Layout and instantiates and assigns
        // it to the ViewHolder.
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            RecyclerView.ViewHolder viewHolder;
            if (_forSearchResults)
            {
                // Inflate our Movie Layout
                View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.SearchResultCell, parent, false);

                // Create our ViewHolder to cache the layout view references and register
                // the OnClick event.
                viewHolder = new SearchResultViewHolder(itemView, OnClick);
            }
            else
            {
                // Inflate our Movie Layout
                View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.MovieCell, parent, false);

                // Create our ViewHolder to cache the layout view references and register
                // the OnClick event.
                viewHolder = new MovieItemViewHolder(itemView, OnClick);
            }
            
            return viewHolder;
        }

        // Must override, this is the important one.  This method is used to
        // bind our current data to your view holder.  Think of this as the equivalent
        // of GetView for regular Adapters.
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var currentMovie = _movies[position];

            // Bind our data from our data source to our View References
            if (_forSearchResults)
            {
                var viewHolder = holder as SearchResultViewHolder;

                viewHolder.MovieName.Text = currentMovie.Title;

                var task = new BitmapDownloaderTask(viewHolder.MoviePhoto);
                viewHolder.MoviePhoto.SetImageDrawable(new DownloadedDrawable(task, Color.Gray));
                viewHolder.MoviePhoto.SetMinimumHeight(300);
                task.Execute(App.IMAGE_PREFIX + currentMovie.PosterPath);
            }
            else
            {
                var viewHolder = holder as MovieItemViewHolder;

                int tempWidth = (int)(ScreenWdith / 3);

                var task = new BitmapDownloaderTask(viewHolder.MoviePhoto);
                viewHolder.MoviePhoto.SetImageDrawable(new DownloadedDrawable(task, Color.Gray));
                viewHolder.MoviePhoto.SetMinimumWidth(tempWidth);
                task.Execute(App.IMAGE_PREFIX + currentMovie.PosterPath);
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
                Mvx.Trace(MvxTraceLevel.Error, "Exception was thrown during MovieRecyclerViewAdapter GetImageBitmapFromUrl", ex);
            }

            return imageBitmap;
        }

        // This will fire any event handlers that are registered with our ItemClick
        // event.
        private void OnClick(int position)
        {
            ItemClick?.Invoke(this, _movies[position].Id);
        }

        // Since this example uses a lot of Bitmaps, we want to do some house cleaning
        // and make them available for garbage collecting as soon as possible.
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}