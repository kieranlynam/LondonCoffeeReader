/*
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using CoffeeClientPrototype.Services;
using CoffeeClientPrototype.ViewModel.Details;
using CoffeeClientPrototype.ViewModel.List;
using CoffeeClientPrototype.ViewModel.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using Microsoft.WindowsAzure.MobileServices;

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
                SimpleIoc.Default.Register<IIdentityService, DesignIdentityService>();
                SimpleIoc.Default.Register<IGeolocationProvider, DesignGeolocationProvider>();
            }
            else
            {
                // Create run time view services and models
                SimpleIoc.Default.Register(CreateDataService);
                SimpleIoc.Default.Register<IIdentityService, DesignIdentityService>();
                SimpleIoc.Default.Register<IGeolocationProvider, GeolocationProvider>(true);
            }

            SimpleIoc.Default.Register<INavigationService, NavigationService>();

            SimpleIoc.Default.Register<IMapLauncher, MapLauncher>();
            SimpleIoc.Default.Register<IShareSource, ShareSource>();

            SimpleIoc.Default.Register<ListViewModel>();
            SimpleIoc.Default.Register<DetailsViewModel>();
            SimpleIoc.Default.Register<MapViewModel>();
        }

        public ListViewModel List
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ListViewModel>();
            }
        }

        public DetailsViewModel Details
        {
            get
            {
                return ServiceLocator.Current.GetInstance<DetailsViewModel>();
            }
        }

        public MapViewModel Map
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MapViewModel>();
            }
        }

        private static IDataService CreateDataService()
        {
            return new AzureDataService(
                new MobileServiceClient(
                        "https://londoncoffee.azure-mobile.net/",
                        "lTnoAEeKSVqJevElSfjpysrtTaPTON54"));
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}