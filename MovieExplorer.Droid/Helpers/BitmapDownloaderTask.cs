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
using Android.Graphics;
using System.Net;
using Android.Media;
using MovieExplorer.Droid.Controls;

namespace MovieExplorer.Droid.Helpers
{
    /// <summary>
    /// An AsyncTask that will asynchronously download an image and assign it to an ImageView.
    /// </summary>
    public class BitmapDownloaderTask : AsyncTask<String, Java.Lang.Void, Bitmap>
    {
        private readonly WeakReference<ImageView> _imageViewReference;

        public BitmapDownloaderTask(ImageView imageView)
        {
            if (imageView == null)
                throw new ArgumentNullException("imageView");

            _imageViewReference = new WeakReference<ImageView>(imageView);
        }

        public string Url { get; private set; }

        /// <summary>
        /// Called on a background thread when the task is executed.
        /// </summary>
        protected override Bitmap RunInBackground(params string[] @params)
        {
            Url = @params[0];
            return DownloadRemoteImage(Url);
        }

        /// <summary>
        /// Once the image is downloaded, associates it to the imageView
        /// </summary>
        protected override void OnPostExecute(Bitmap bitmap)
        {
            if (IsCancelled)
                bitmap = null;

            if (_imageViewReference != null && bitmap != null)
            {
                ImageView imageView;
                if (!_imageViewReference.TryGetTarget(out imageView))
                    return;

                DownloadedDrawable drawable = (DownloadedDrawable)imageView.Drawable;
                if(drawable != null)
                {
                    var bitmapDownloaderTask = drawable.GetBitmapDownloaderTask();

                    // Change bitmap only if this process is still associated with it.
                    // This is necessary as views can be reused by Android, and a newer BitmapDownloader instance may have been attached to it.
                    if (this == bitmapDownloaderTask)
                        imageView.SetImageBitmap(bitmap);
                }
            }
        }

        private Bitmap DownloadRemoteImage(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("url must not be null, empty, or whitespace");

            Uri imageUri;
            if (!Uri.TryCreate(url, UriKind.Absolute, out imageUri))
                throw new ArgumentException("Invalid url");

            try
            {
                WebRequest request = HttpWebRequest.Create(imageUri);
                request.Timeout = 10000;

                WebResponse response = request.GetResponse();
                System.IO.Stream inputStream = response.GetResponseStream();

                return BitmapFactory.DecodeStream(inputStream);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}