using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovieExplorer.Client.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {

        }

        private static HomeViewModel _home;
        public HomeViewModel Home
        {
            get
            {
                if(_home == null)
                {
                    _home = new HomeViewModel();
                }
                return _home;
            }
        }
    }
}