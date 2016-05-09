using MovieExplorer.Client.Models;
using MvvmCross.Plugins.Messenger;

namespace MovieExplorer.Client.Messages
{
    public class SelectedMovieMessage : MvxMessage
    {
        public SelectedMovieMessage(object sender, Movie movie) : base(sender)
        {
            SelectedMovie = movie;
        }

        public Movie SelectedMovie { get; private set; }
    }
}