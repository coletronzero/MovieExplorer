using MvvmCross.Plugins.Messenger;

namespace MovieExplorer.Client.Messages
{
    public class MovieFavoritedMessage : MvxMessage
    {
        public MovieFavoritedMessage(object sender, int movieId) : base(sender)
        {
            MovieId = movieId;
        }

        public int MovieId { get; private set; }
    }
}
