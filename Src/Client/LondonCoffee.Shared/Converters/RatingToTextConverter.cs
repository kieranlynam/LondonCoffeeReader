using System;
using Windows.UI.Xaml.Data;

namespace CoffeeClientPrototype.Converters
{
    public sealed class RatingToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var rating = (double) value;
            return string.Format("{0}/5", rating);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
