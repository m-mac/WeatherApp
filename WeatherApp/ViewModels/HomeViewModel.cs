using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using WeatherApp.Models;

namespace WeatherApp.ViewModels;

public class HomeViewModel : ObservableObject
{
    // When this changes (an item is added, removed or updated) LocationGroups needs to be reloaded.
    public ObservableCollection<LocationRecord> Locations { get; } = new();
    public ObservableCollection<LocationGroup> LocationGroups { get; } = new();

    /*
     * Going to need to filter Locations into a LocationGroups based on "Pinned" property
     *  
     */
    
    
    public HomeViewModel()
    {
        AddDummyLocations();
    }


    private void LoadLocations()
    {
        // Wire to SQLite backend
        throw new NotImplementedException();
    }


    private void LoadLocationGroups()
    {
        // Divide into groups based on pinned
        // Could add a "Warnings" group later and track sever weather warnings in background 
        var pinned = Locations.Where(l => l.Pinned).ToList();
        var notPinned = Locations.Where(l => !l.Pinned).ToList();

        // Initial clear so we can reuse this method on sync
        LocationGroups.Clear();
        
        
        if (pinned.Any())
            LocationGroups.Add(new LocationGroup("Pinned", pinned));
        
        if (notPinned.Any())
            LocationGroups.Add(new LocationGroup("NotPinned", notPinned));
        
        
        // If LocationGroups is empty we want to put a little view with some instructions or something in it's place

    }
    
    

    private void AddDummyLocations()
    {
        Locations.Add(new LocationRecord
        {
            Id = 1,
            Name = "Central Park",
            Address = "New York, NY, USA",
            Latitude = 40.785091,
            Longitude = -73.968285,
            SavedAt = DateTime.Now.AddDays(-2),
            Pinned = true
        });

        Locations.Add(new LocationRecord
        {
            Id = 2,
            Name = "Eiffel Tower",
            Address = "Paris, France",
            Latitude = 48.8584,
            Longitude = 2.2945,
            SavedAt = DateTime.Now.AddDays(-1),
            Pinned = false
        });

        Locations.Add(new LocationRecord
        {
            Id = 3,
            Name = "Sydney Opera House",
            Address = "Sydney NSW, Australia",
            Latitude = -33.8568,
            Longitude = 151.2153,
            SavedAt = DateTime.Now.AddHours(-5),
            Pinned = false
        });

        Locations.Add(new LocationRecord
        {
            Id = 4,
            Name = "Tokyo Tower",
            Address = "Tokyo, Japan",
            Latitude = 35.6586,
            Longitude = 139.7454,
            SavedAt = DateTime.Now.AddHours(-10),
            Pinned = true
        });

        Locations.Add(new LocationRecord
        {
            Id = 5,
            Name = "Christ the Redeemer",
            Address = "Rio de Janeiro, Brazil",
            Latitude = -22.9519,
            Longitude = -43.2105,
            SavedAt = DateTime.Now,
            Pinned = false
        });

    }
}

