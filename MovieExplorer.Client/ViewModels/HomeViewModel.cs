using MovieExplorer.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieExplorer.Client.ViewModels
{
    public class HomeViewModel
    {
        public HomeViewModel()
        {
        }

        public Action<List<Movie>> AddTopRatedMoviesToView { get; set; }
        public Action<List<Movie>> AddPopularMoviesToView { get; set; }
        public Action<List<Movie>> AddNowPlayingMoviesToView { get; set; }

        public async Task InitializeMovieListsAsync()
        {
            var topRatedMoviesResponse = await ApplicationMaster.MovieClient.GetTopRatedMoviesAsync();
            if(topRatedMoviesResponse.IsOk)
            {
                TopRatedMovies = topRatedMoviesResponse.Body.Results;
                if(AddTopRatedMoviesToView != null)
                {
                    AddTopRatedMoviesToView(TopRatedMovies);
                }
            }
            var popularMoviesResponse = await ApplicationMaster.MovieClient.GetPopularMoviesAsync();
            if (popularMoviesResponse.IsOk)
            {
                PopularMovies = popularMoviesResponse.Body.Results;
                if (AddPopularMoviesToView != null)
                {
                    AddPopularMoviesToView(PopularMovies);
                }
            }
            var nowPlayingMoviesResponse = await ApplicationMaster.MovieClient.GetNowPlayingMoviesAsync();
            if (nowPlayingMoviesResponse.IsOk)
            {
                NowPlayingMovies = nowPlayingMoviesResponse.Body.Results;
                if (AddNowPlayingMoviesToView != null)
                {
                    AddNowPlayingMoviesToView(NowPlayingMovies);
                }
            }
        }

        public List<Movie> TopRatedMovies { get; private set; }

        public List<Movie> PopularMovies { get; private set; }

        public List<Movie> NowPlayingMovies { get; private set; }
    }
}