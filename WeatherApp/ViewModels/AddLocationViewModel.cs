using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Logging;
using WeatherApp.Extensions;
using WeatherApp.Interfaces;
using WeatherApp.Models;

namespace WeatherApp.ViewModels;

public partial class AddLocationViewModel : ObservableObject
{
    private const int MaxSuggestions = 4;

    // For checking if placemark exists in collection
    private const double Tolerance = 0.000001;

    // For cancelling the Geocoding search to prevent flooding the API
    private CancellationTokenSource cts;

    private IGeocodingService geocodingService;
    private ILogger logger;
    private IModalService modalService;
    private IRepository<LocationRecord> repository;

    public ObservableCollection<Placemark> Suggestions { get; } = new();

    public AddLocationViewModel(ILogger<AddLocationViewModel> logger,
                                IGeocodingService geocodingService,
                                IModalService modalService,
                                IDatabaseProvider databaseProvider,
                                IRepository<LocationRecord> repository)
    {
        this.geocodingService = geocodingService;
        this.logger = logger;
        this.modalService = modalService;
        this.repository = repository;
    }

    [RelayCommand]
    private void PopModal() => modalService.PopModalAsync();

    [ObservableProperty] private string searchText;

    [ObservableProperty] private string suggestionText;
    [ObservableProperty] private Placemark? selectedSuggestion;

    partial void OnSelectedSuggestionChanged(Placemark? value)
    {
        if (value == null) return;
        _ = HandleSelectionAsync(value);
    }

    private async Task HandleSelectionAsync(Placemark value)
    {
        var formattedSelection = geocodingService.FormatSuggestion(value);

        var trimmedSelection = formattedSelection.Length <= 12
            ? formattedSelection
            : formattedSelection.Substring(0, 12) + "...";

        var selectedAction = await modalService.ShowActionSheetAsync(
            trimmedSelection,
            "Cancel",
            null,
            new[] { "Select Location", "Select & Pin" }
        );

        if (selectedAction == "Cancel" || string.IsNullOrEmpty(selectedAction)) return;

        var locationRecord = value.ToLocationRecord();

        if (selectedAction == "Select & Pin")
        {
            locationRecord.Pinned = true;
        }
        else
        {
            // This would unset pinned on an existing record
            locationRecord.Pinned = false;
        }


        // This allows a user to set an existing location to Pinned
        await repository.UpsertAsync(locationRecord);

        WeakReferenceMessenger.Default.Send(new LocationAddedMessage());

        SelectedSuggestion = null;
    }


    partial void OnSearchTextChanged(string value)
    {
        cts?.Cancel();
        cts = new CancellationTokenSource();
        var token = cts.Token;

        _ = Task.Run(async () =>
        {
            await Task.Delay(300, token);
            if (!token.IsCancellationRequested)
                await UpdateSuggestionsAsync(value, token);
        }, token);
    }

    private async Task UpdateSuggestionsAsync(string query, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(query))
            return;

        try
        {
            var locations = await geocodingService.GetLocationsAsync(query);
            var placemarks = new List<Placemark>();

            // Collect all placemarks from locations
            foreach (var loc in locations)
            {
                token.ThrowIfCancellationRequested();

                var cityPlacemark = GetCityPlacemark(await geocodingService.GetPlacemarksAsync(loc));
                if (cityPlacemark == null ||
                    IsDuplicatePlacemark(cityPlacemark)) continue;

                logger.LogInformation(
                    $"{cityPlacemark.Locality}, {cityPlacemark.AdminArea}, {cityPlacemark.CountryName}, {cityPlacemark.PostalCode}");

                // Insert at top so the CollectionView renders new items first and is LIFO
                Suggestions.Insert(0, cityPlacemark);

                while (Suggestions.Count > MaxSuggestions)
                    Suggestions.RemoveAt(Suggestions.Count - 1);
            }
        }
        catch (Exception ex)
        {
            logger.LogError($"Geocoding error: {ex.Message}");
        }
    }

    private static Placemark? GetCityPlacemark(IEnumerable<Placemark> placemarks)
        => placemarks.FirstOrDefault(p => !string.IsNullOrWhiteSpace(p.Locality));

    /// <summary>
    /// Finds duplicate placemarks to filter out by matching the lat/lon and matching the formatted string.
    /// </summary>
    /// <remarks>
    /// This is not the best way to handle this, but the number of items is kept reasonably
    /// low enough for it to not be a problem.
    /// </remarks>
    private bool IsDuplicatePlacemark(Placemark p)
    {
        return Suggestions.Any(existing =>
            // Either the coordinates are close enough
            Math.Abs(existing.Location.Latitude - p.Location.Latitude) < Tolerance &&
            Math.Abs(existing.Location.Longitude - p.Location.Longitude) < Tolerance
            // Or the formatted strings are a match
            || geocodingService.FormatSuggestion(existing) == geocodingService.FormatSuggestion(p)
        );
    }
}