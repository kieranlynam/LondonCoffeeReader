using System;
using System.Collections.Generic;
using System.Text;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Data;
using CoffeeClientPrototype.ViewModel.List;

namespace CoffeeClientPrototype.Converters
{
    public class GeopointConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var coord = value as ObservableCoordinate;
            if (coord != null)
            {
                var position = new BasicGeoposition
                    {
                        Latitude = coord.Latitude,
                        Longitude = coord.Longitude
                    };
                return new Geopoint(position);
            }

            var viewmodel = value as CafeSummaryViewModel;
            if (viewmodel != null)
            {
                var position = new BasicGeoposition
                    {
                        Latitude = viewmodel.Latitude,
                        Longitude = viewmodel.Longitude
                    };
                return new Geopoint(position);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
