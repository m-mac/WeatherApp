using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Logging;
using WeatherApp.Extensions;
using WeatherApp.Interfaces;
using WeatherApp.Models;
using WeatherApp.Pages;

namespace WeatherApp.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    // When this changes (an item is added, removed or updated) LocationGroups needs to be reloaded.
    public ObservableCollection<LocationRecord> Locations { get; } = new();
    public ObservableCollection<LocationGroup> LocationGroups { get; } = new();


    private IRepository<WeatherRecord> weatherRecordRepository;
    private IRepository<LocationRecord> locationRepository;
    private IModalService modalService;
    private ILogger<HomeViewModel> logger;
    private IWeatherApiService weatherApiService;

    public HomeViewModel(IRepository<WeatherRecord> weatherRecordRepository,
                         IRepository<LocationRecord> locationRepository,
                         IModalService modalService,
                         ILogger<HomeViewModel> logger,
                         IWeatherApiService weatherApiService)
    {
        this.weatherRecordRepository = weatherRecordRepository;
        this.locationRepository = locationRepository;
        this.modalService = modalService;
        this.logger = logger;
        this.weatherApiService = weatherApiService;

        WeakReferenceMessenger.Default.Register<LocationAddedMessage>(this,
            async (r, m) => { await LoadLocationsAsync(); });

        WeakReferenceMessenger.Default.Register<DataResetMessage>(this, (r, m) =>
        {
            Locations.Clear();
            LocationGroups.Clear();
        });

        // Initial load
        _ = LoadLocationsAsync();
    }


    [RelayCommand]
    private async Task AddLocation()
    {
        var page = App.Services.GetRequiredService<AddLocationPage>();
        await modalService.PushModalAsync(page);
    }

    private async Task LoadLocationsAsync()
    {
        try
        {
            var locations = await locationRepository.GetAllAsync();
            if (!locations.Any())
            {
                logger.LogInformation("No locations found");
                return;
            }

            Locations.Clear();
            LocationGroups.Clear();

            foreach (var location in locations)
            {
                location.CurrentWeatherRecord = await GetCurrentWeatherForLocationAsync(location);

                // Check to see if we've got a weather record and that it's current
                if (location.CurrentWeatherRecord == null ||
                    location.CurrentWeatherRecord.RecordTime.Date != DateTime.UtcNow.Date)
                {
                    await GetCurrentWeatherForLocationAsync(location);
                }

                Locations.Add(location);
            }

            LoadLocationGroups();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to load locations");
        }
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
    }

    private async Task<WeatherRecord> GetCurrentWeatherForLocationAsync(LocationRecord location)
    {
        if (location == null) throw new ArgumentNullException(nameof(location));

        var currentDate = DateTime.UtcNow.Date;

        // Query the most recent WeatherRecord for this location
        var recentRecord = await weatherRecordRepository.FirstOrDefaultAsync<WeatherRecord>(q =>
            q.Where(w => w.LocationId == location.Id)
                .OrderByDescending(w => w.RecordTime)
        );

        if (recentRecord != null && recentRecord.RecordTime.Date == currentDate)
        {
            location.CurrentWeatherRecord = recentRecord;
            return recentRecord;
        }

        // Fetch from API if missing or outdated
        var weatherResponse = await weatherApiService.GetWeatherResponseAsync(
            location.Latitude,
            location.Longitude,
            currentDate
        );

        if (weatherResponse == null)
        {
            location.CurrentWeatherRecord = null;
            return null;
        }

        var newRecord = weatherResponse.ToWeatherRecord(location.Id);

        // Save to database
        int newId = await weatherRecordRepository.InsertAsync(newRecord);
        newRecord.Id = newId; // assign returned ID
        location.CurrentWeatherRecord = newRecord;

        return newRecord;
    }
}