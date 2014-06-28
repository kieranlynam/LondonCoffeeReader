using System;
using System.Windows.Input;
using CoffeeClientPrototype.Model;
using CoffeeClientPrototype.ViewModel.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace CoffeeClientPrototype.ViewModel.Details
{
    public class NewComment : ViewModelBase
    {
        private readonly IDataService dataService;

        private string text;
        private Cafe associatedCafe;

        public string Text
        {
            get { return this.text; }
            set
            {
                var before = this.CanExecuteSubmit();
                if (this.Set(ref this.text, value))
                {
                    var after = this.CanExecuteSubmit();
                    if (before != after)
                    {
                        this.Submit.RaiseCanExecuteChanged();
                    }
                }
            }
        }

        public Cafe AssociatedCafe
        {
            get
            {
                return this.associatedCafe;
            }
            set
            {
                if (this.Set(ref this.associatedCafe, value))
                {
                    this.Submit.RaiseCanExecuteChanged();
                }
            }
        }

        public RelayCommand Submit { get; private set; }

        public NewComment(IDataService dataService)
        {
            this.dataService = dataService;
            this.Submit = new RelayCommand(this.OnSubmitExecuted, this.CanExecuteSubmit);
        }

        private bool CanExecuteSubmit()
        {
            if (this.AssociatedCafe == null)
            {
                return false;
            }

            if (string.IsNullOrEmpty(this.text))
            {
                return false;
            }
            
            return true;
        }

        private void OnSubmitExecuted()
        {
            var comment = new Comment
                {
                    Text = this.text
                };
            this.dataService.SubmitComment(
                this.AssociatedCafe.Id,
                comment);
        }
    }
}
