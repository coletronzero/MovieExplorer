using Android.Content;
using Android.Widget;

namespace MovieExplorer.Droid.Controls
{
    internal class MovieImageView : ImageView
    {
        public MovieImageView (Context context) : base(context)
        {

        }

        public int MovieId { get; set; }
    }
}