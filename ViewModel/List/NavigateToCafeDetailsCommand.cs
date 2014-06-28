using System;
using System.Collections.Generic;
using System.Windows.Input;
using CoffeeClientPrototype.Model;
using CoffeeClientPrototype.ViewModel.Services;

namespace CoffeeClientPrototype.ViewModel.List
{
    public class NavigateToCafeDetailsCommand : ICommand
    {
        private readonly Cafe cafe;
        private readonly INavigationService navigationService;

        public NavigateToCafeDetailsCommand(Cafe cafe, INavigationService navigationService)
        {
            this.cafe = cafe;
            this.navigationService = navigationService;
        }

        public void Execute(object parameter)
        {
            this.navigationService.Navigate(
                "CafeDetails",
                new Dictionary<string, object>
                {
                    { "Id", this.cafe.Id }
                });
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
    }
}