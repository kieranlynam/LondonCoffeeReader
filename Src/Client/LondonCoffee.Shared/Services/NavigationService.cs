using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using CoffeeClientPrototype.View;
using CoffeeClientPrototype.ViewModel.Services;

namespace CoffeeClientPrototype.Services
{
    public class NavigationService : INavigationService
    {
        private static readonly IDictionary<string, object> EmptyParametes = new Dictionary<string, object>();

        private static Frame Frame
        {
            get { return ((Frame) Window.Current.Content); }
        }

        public void Navigate(string destination, IDictionary<string, object> parameters = null)
        {
            parameters = parameters ?? EmptyParametes;
            
            switch (destination)
            {
                case "CafeDetails":
                    Frame.Navigate(typeof(CafeDetailsPage), parameters);
                    break;

#if WINDOWS_PHONE_APP
                case "Map":
                    Frame.Navigate(typeof(MapPage), parameters);
                    break;
#endif

                default:
                    throw new NotSupportedException(destination + " navigation destination not supported");
            }
        }
    }
}
