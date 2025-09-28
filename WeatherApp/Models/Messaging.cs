namespace WeatherApp.Models;

using CommunityToolkit.Mvvm.Messaging.Messages;

public class LocationAddedMessage : ValueChangedMessage<bool>
{
    public LocationAddedMessage(bool value = true) : base(value)
    {
    }
}

public class DataResetMessage : ValueChangedMessage<bool>
{
    public DataResetMessage(bool value = true) : base(value)
    {
    }
}