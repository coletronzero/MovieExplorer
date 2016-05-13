using MvvmCross.Plugins.Messenger;

namespace MovieExplorer.Client.Messages
{
    public class MovieUnfavoritedMessage : MvxMessage
    {
        public MovieUnfavoritedMessage(object sender, int movieId) : base(sender)
        {
            MovieId = movieId;
        }

        public int MovieId { get; private set; }
    }
}
