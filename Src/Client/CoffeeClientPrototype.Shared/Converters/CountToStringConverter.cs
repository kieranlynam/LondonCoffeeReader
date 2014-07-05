using System;
using System.Collections;
using Windows.UI.Xaml.Data;

namespace CoffeeClientPrototype.Converters
{
    public sealed class StringFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var format = parameter as string;

            if (string.IsNullOrEmpty(format))
            {
                throw new NotSupportedException("Must specify a format string (as a converter parameter)");
            }

            return string.Format(format, value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
