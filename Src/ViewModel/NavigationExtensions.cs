using System.Collections.Generic;
using CoffeeClientPrototype.Model;
using CoffeeClientPrototype.ViewModel.Services;

namespace CoffeeClientPrototype.ViewModel
{
    internal static class NavigationExtensions
    {
        public static void NavigateToCoffeeDetails(this INavigationService navigationService, Cafe cafe)
        {
            navigationService.Navigate(
                "CafeDetails",
                new Dictionary<string, object>
                {
                    { "Id", cafe.Id }
                });
        }

        public static void NavigateToMap(this INavigationService navigationService, string cafeId = null)
        {
            if (cafeId == null)
            {
                navigationService.Navigate("Map");
            }
            else
            {
                navigationService.Navigate(
                "Map",
                new Dictionary<string, object>
                {
                    { "Id", cafeId }
                });
            }
        }
    }
}
