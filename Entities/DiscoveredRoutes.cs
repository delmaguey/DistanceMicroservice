public record Addresses(string OriginAddress, string DestinationAddress);
public class DiscoveredRoutes(string OriginAddress, string DestinationAddress, string Message, Route[] Routes)
{
    public string OriginAddress { get; } = OriginAddress;
    public string DestinationAddress { get; } = DestinationAddress;
    public string Message { get; set; } = Message;
    public Route[] Routes { get; set; } = Routes;
}
public record Routes(Route[] routes);

/// <param name="distanceMeters"> Distance in meters. </param>
/// <param name="duration"> Duration in seconds. </param>
public record Route(int distanceMeters, string duration)
{
    public required int DistanceMeters = distanceMeters;
    public required string Duration = duration;
    // Convert meters to miles
    public double DistanceInMiles =>
    Math.Round(distanceMeters / 1609.34, 2);
}
public record RouteRequest(Origin origin, 
                           Destination destination, 
                           string travelMode, 
                           string routingPreference, 
                           bool computeAlternativeRoutes, 
                           Routemodifiers routeModifiers, 
                           string languageCode, 
                           string units);
public record Origin(string Address);
public record Destination(string Address);
public record Routemodifiers(bool avoidTolls, bool avoidHighways, bool avoidFerries);