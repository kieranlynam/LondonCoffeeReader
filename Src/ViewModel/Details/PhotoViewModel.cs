using System.ComponentModel;
using System.Runtime.CompilerServices;
using CoffeeClientPrototype.Model;
using CoffeeClientPrototype.ViewModel.Annotations;

namespace CoffeeClientPrototype.ViewModel.Details
{
    public class PhotoViewModel : INotifyPropertyChanged
    {
        private string submittedBy;

        public string SubmittedBy
        {
            get { return this.submittedBy; }
            set
            {
                if (value == this.submittedBy) return;
                this.submittedBy = value;
                this.OnPropertyChanged();
            }
        }

        public PhotoViewModel(Photo model)
        {
            this.SubmittedBy = model.SubmittedBy;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
