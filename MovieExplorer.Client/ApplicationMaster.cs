using MovieExplorer.Client.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovieExplorer.Client
{
    public static class ApplicationMaster
    {
        private const string API_KEY = "d228489ec1663d555aced8667d465766";

        public static MovieSharpClient MovieClient { get; private set; }

        public static void Init()
        {
            MovieClient = new MovieSharpClient(API_KEY);
        }
    }
}