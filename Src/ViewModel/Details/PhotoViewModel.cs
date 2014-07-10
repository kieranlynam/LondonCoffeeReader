using System;
using CoffeeClientPrototype.Model;
using GalaSoft.MvvmLight;

namespace CoffeeClientPrototype.ViewModel.Details
{
    public class PhotoViewModel : ObservableObject
    {
        private string submittedBy;
        private Uri imageUri;

        public Uri ImageUri
        {
            get { return this.imageUri; }
            set { this.Set(ref this.imageUri, value); }
        }

        public string SubmittedBy
        {
            get { return this.submittedBy; }
            set { this.Set(ref this.submittedBy, value); }
        }

        public PhotoViewModel(Photo model)
        {
            this.ImageUri = model.ImageUri;
            this.SubmittedBy = model.SubmittedBy;
        }
    }
}
