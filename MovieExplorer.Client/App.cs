using MvvmCross.Platform.IoC;

namespace MovieExplorer.Client
{
    public class App : MvvmCross.Core.ViewModels.MvxApplication
    {
        public const string IMAGE_PREFIX = "http://image.tmdb.org/t/p/w500";
        public const string FILE_SUBDIRECTORY = "MovieExplorer";
        public const string FILE_NAME = "FavoriteMovies.txt";

        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            RegisterAppStart<ViewModels.MainViewModel>();
        }
    }
}