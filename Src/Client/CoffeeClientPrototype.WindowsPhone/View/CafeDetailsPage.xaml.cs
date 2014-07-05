using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556
using CoffeeClientPrototype.AppBar;
using CoffeeClientPrototype.View;

namespace CoffeeClientPrototype
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CafeDetailsPage : Page
    {
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
    }
}
