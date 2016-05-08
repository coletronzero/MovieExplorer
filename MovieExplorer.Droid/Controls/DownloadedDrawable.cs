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
using Android.Graphics.Drawables;
using MovieExplorer.Droid.Helpers;
using Android.Graphics;

namespace MovieExplorer.Droid.Controls
{
    /// <summary>
    /// A fake Drawable that will be attached to the ImageView while the download is in progress.
    /// Contains a reference to the actual download task, so that a download task can be stopped
    ///  if a new binding is required, and makes sure that only the last started download process can
    ///  bind its result, independently of the download finish order.
    /// </summary>
    public class DownloadedDrawable : ColorDrawable
    {
        private readonly WeakReference<BitmapDownloaderTask> _bitmapDownloaderTaskReference;

        public DownloadedDrawable(BitmapDownloaderTask bitmapDownloaderTask, Color loadingBackgroundColor)
            : base(loadingBackgroundColor)
        {
            _bitmapDownloaderTaskReference = new WeakReference<BitmapDownloaderTask>(bitmapDownloaderTask);
        }

        public BitmapDownloaderTask GetBitmapDownloaderTask()
        {
            BitmapDownloaderTask task;
            _bitmapDownloaderTaskReference.TryGetTarget(out task);
            return task;
        }

        ///// <summary>
        ///// Retrieve the BitmapDownloaderTask from the ImageView's drawable.
        ///// This will return null if the drawable is not a DownloadedDrawable, or there is no BitmapDownloaderTask attached to it.
        ///// </summary>
        //public static BitmapDownloaderTask GetBitmapDownloaderTask(this ImageView imageView)
        //{
        //    if (imageView != null)
        //    {
        //        var drawable = imageView.Drawable as DownloadedDrawable;
        //        if (drawable != null)
        //            return drawable.GetBitmapDownloaderTask();
        //    }

        //    return null;
        //}
    }
}