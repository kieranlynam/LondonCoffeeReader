using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using CoffeeClientPrototype.View;
using CoffeeClientPrototype.ViewModel.Services;

namespace CoffeeClientPrototype.Services
{
    public class NavigationService : INavigationService
    {
        private static Frame Frame
        {
            get { return ((Frame) Window.Current.Content); }
        }

        public void Navigate(string destination, IDictionary<string, object> parameters = null)
        {
            Frame.Navigate(typeof(CafeDetailsPage), parameters);
        }
    }
}
