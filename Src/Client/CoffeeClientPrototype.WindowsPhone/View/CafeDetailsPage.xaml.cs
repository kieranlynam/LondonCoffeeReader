// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using CoffeeClientPrototype.AppBar;
using CoffeeClientPrototype.ViewModel.Details;

namespace CoffeeClientPrototype.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CafeDetailsPage : Page
    {
        public DetailsViewModel ViewModel
        {
            get { return ((ViewModelLocator) Application.Current.Resources["Locator"]).Details; } 
        }

        public CafeDetailsPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await this.NotifyNavigatedTo(e);
            this.ViewModel.CurrentIdentityReview.ReviewSubmitted += this.OnReviewSubmitted;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            this.ViewModel.CurrentIdentityReview.ReviewSubmitted -= this.OnReviewSubmitted;
            this.NotifyNavigatedFrom();
        }

        private void OnReviewSubmitted(object sender, ReviewSubmittedEventArgs args)
        {
            if (this.Pivot.SelectedItem == this.Rate)
            {
                this.Pivot.SelectedItem = this.Details;
            }
        }

        private void OnPivotItemLoading(Pivot sender, PivotItemEventArgs args)
        {
            var commandBar = ((CommandBar) this.BottomAppBar);
            var buttons = commandBar.PrimaryCommands.OfType<PivotItemAppBarButton>();
            foreach (var button in buttons)
            {
                button.Visibility = button.PivotItem == args.Item.Name ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void OnRatingsGridTapped(object sender, TappedRoutedEventArgs e)
        {
            this.Pivot.SelectedItem = this.Rate;
        }
    }
}
