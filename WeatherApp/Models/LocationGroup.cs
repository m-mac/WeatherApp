using System.Collections.ObjectModel;
using Microsoft.Maui.Graphics;

namespace WeatherApp.Models;

public class LocationGroup : ObservableCollection<LocationRecord>
{
    /// <summary>
    /// The title of the group (e.g. "pinned", "locations", "warning"
    /// </summary>
    public string Title { get; private set; }
    public Color TextColor {get; private set;}
    
    // public string IconSource {get ; private set;}

    public LocationGroup(string title, IEnumerable<LocationRecord> locations, Color? textColor = null) : base(locations)
    {
        TextColor = textColor ?? Colors.Black;
        Title = title;
    }
}