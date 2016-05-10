using MovieExplorer.Client.Models;
using MvvmCross.Plugins.Messenger;

namespace MovieExplorer.Client.Messages
{
    public class SelectedMovieMessage : MvxMessage
    {
        public SelectedMovieMessage(object sender, MovieDto movie) : base(sender)
        {
            SelectedMovie = movie;
        }

        public MovieDto SelectedMovie { get; private set; }
    }
}