namespace WeatherApp.Converters;
using Microsoft.Maui.Controls;
using System.Globalization;


public class PlacemarkToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Placemark placemark)
        {
            // This is the same as FormatSuggestion in GeocodingService
            return $"{placemark.Locality}, {placemark.AdminArea}, {placemark.CountryName}";
        }
        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
