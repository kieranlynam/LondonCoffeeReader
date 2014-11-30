using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using CoffeeClientPrototype.ViewModel;

namespace CoffeeClientPrototype.View
{
    public static class PageExtensions
    {
        public static Task NotifyNavigatedTo(this Page page, NavigationEventArgs args)
        {
            var listener = page.DataContext as INavigationListener;

            if (listener == null)
            {
                throw new InvalidOperationException(string.Format("DataContext must be bound to a {0}", typeof(INavigationListener).Name));
            }

            return listener.OnNavigatedTo(args.Parameter as IDictionary<string, object>);
        }

        public static void NotifyNavigatedFrom(this Page page)
        {
            var listener = page.DataContext as INavigationListener;

            if (listener == null)
            {
                throw new InvalidOperationException(string.Format("DataContext must be bound to a {0}", typeof(INavigationListener).Name));
            }

            listener.OnNavigatedFrom();
        }
    }
}