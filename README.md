# WeatherApp

A .NET MAUI mobile application demonstrating dependency injection, local storage, and weather API integration.

---

## Features

- **Dependency Injection** for services, repositories, and view models.
- **Local Storage** with [`SQLite-net-pcl`](https://www.nuget.org/packages/sqlite-net-pcl/) for locations and weather records.
- **Weather Data** from [OpenWeatherMap API](https://api.openweathermap.org) (`/data/2.5/weather`).
- **Geocoding API** for basic autocomplete in the search bar when adding locations.
- **FontAwesome Icons** via CDN for UI elements.
- **Pinned Locations** to keep favorite locations visible.
- **MVVM Architecture** with clean separation of concerns.
- **Secure API Key Storage** using `SecureStorage`.

---

## Requirements

- .NET MAUI (latest stable)
- OpenWeatherMap API Key
    - Sign up at [OpenWeatherMap.org](https://openweathermap.org/) and generate a free API key.
    - Enter the key in **Settings** before adding locations to fetch temperature data.

---

## Usage

1. Enter your API key in the **Settings** page.
2. Add new locations via the **Add Location** page (autocomplete provided).
3. View current weather, humidity, and condition for each location on the **Home** page.
4. Pin locations if desired.

---

## Notes

- Only **city-level locations** are supported.
- Weather data is **cached per day** to reduce API calls.
- Ensure network access for API requests.
- Screenshots can be added to `screenshots/` for presentation.

---

## License

MIT License. See [LICENSE](LICENSE) for details.

---

Created by **Mike Mac** to demonstrate modern .NET MAUI app patterns, dependency injection, and API integration.
