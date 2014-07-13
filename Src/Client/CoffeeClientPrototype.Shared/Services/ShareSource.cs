using Windows.ApplicationModel.DataTransfer;
using CoffeeClientPrototype.ViewModel.Services;

namespace CoffeeClientPrototype.Services
{
    public class ShareSource : IShareSource
    {
        private bool isEnabled;
        private DataTransferManager dataTransferManager;
        private SharePackage sharePackage;

        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set
            {
                if (value == this.isEnabled)
                {
                    return;
                }

                if (value)
                {
                    this.dataTransferManager = DataTransferManager.GetForCurrentView();
                    this.dataTransferManager.DataRequested += this.OnDataRequested;
                }
                else
                {
                    this.dataTransferManager.DataRequested -= this.OnDataRequested;
                    this.dataTransferManager = null;
                }

                this.isEnabled = value;
            }
        }

        public void Share(SharePackage package)
        {
            this.sharePackage = package;
            DataTransferManager.ShowShareUI();
        }

        private void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            args.Request.Data.Properties.Title = this.sharePackage.Title;
            args.Request.Data.SetText(this.sharePackage.Text);

            if (this.sharePackage.Description != null)
            {
                args.Request.Data.Properties.Description = this.sharePackage.Description;
            }
        }
    }
}
