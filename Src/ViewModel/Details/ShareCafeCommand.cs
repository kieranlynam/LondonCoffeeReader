using System;
using System.Windows.Input;
using CoffeeClientPrototype.Model;
using CoffeeClientPrototype.ViewModel.Services;

namespace CoffeeClientPrototype.ViewModel.Details
{
    public class ShareCafeCommand : ICommand
    {
        private Cafe cafe;
        private readonly IShareSource shareSource;

        public ShareCafeCommand(IShareSource shareSource)
        {
            this.shareSource = shareSource;
        }

        public Cafe Cafe
        {
            get { return cafe; }
            set
            {
                this.cafe = value;
                if (this.CanExecuteChanged != null)
                {
                    this.CanExecuteChanged(this, EventArgs.Empty);
                }
            }
        }

        public bool CanExecute(object parameter)
        {
            return this.cafe != null;
        }

        public void Execute(object parameter)
        {
            if (this.cafe != null)
            {
                this.shareSource.Share(this.CreateSharePackage());
            }
        }

        private SharePackage CreateSharePackage()
        {
            return new SharePackage
                {
                    Title = this.Cafe.Name,
                    Text = string.Concat(this.Cafe.Address, ", ", this.Cafe.PostCode),
                };
        }

        public event EventHandler CanExecuteChanged;
    }
}
