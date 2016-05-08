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

        public async Task<List<Movie>> GetTopRatedMoviesAsync()
        {
            List<Movie> movies = new List<Movie>();
            var topRatedMoviesResponse = await ApplicationMaster.MovieClient.GetTopRatedMoviesAsync();
            if (topRatedMoviesResponse.IsOk)
            {
                movies = topRatedMoviesResponse.Body.Results;
            }
            return movies;
        }

        public async Task<List<Movie>> GetPopularMoviesAsync()
        {
            List<Movie> movies = new List<Movie>();
            var topRatedMoviesResponse = await ApplicationMaster.MovieClient.GetPopularMoviesAsync();
            if (topRatedMoviesResponse.IsOk)
            {
                movies = topRatedMoviesResponse.Body.Results;
            }
            return movies;
        }

        public async Task<List<Movie>> GetNowPlayingMoviesAsync()
        {
            List<Movie> movies = new List<Movie>();
            var topRatedMoviesResponse = await ApplicationMaster.MovieClient.GetNowPlayingMoviesAsync();
            if (topRatedMoviesResponse.IsOk)
            {
                movies = topRatedMoviesResponse.Body.Results;
            }
            return movies;
        }
    }
}