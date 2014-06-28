using System;
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
            this.navigationService.NavigateToCoffeeDetails(cafe);
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
    }
}