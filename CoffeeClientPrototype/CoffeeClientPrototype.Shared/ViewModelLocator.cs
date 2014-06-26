/*
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using CoffeeClientPrototype.ViewModel.List;
using CoffeeClientPrototype.ViewModel.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace CoffeeClientPrototype
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                // Create design time view services and models
                SimpleIoc.Default.Register<IDataService, DesignDataService>();
            }
            else
            {
                // Create run time view services and models
                SimpleIoc.Default.Register<IDataService, DesignDataService>();
            }

            SimpleIoc.Default.Register<ListViewModel>();
        }

        public ListViewModel List
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ListViewModel>();
            }
        }
        
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}