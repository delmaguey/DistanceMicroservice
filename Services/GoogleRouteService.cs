
public class GoogleRouteService
{
    public async Task<DiscoveredRoutes> GetRouteInfo(Addresses addresses, string apiUrl, string apiKey)
    {
        HttpClient httpClient = new();
        httpClient.DefaultRequestHeaders.Add("X-Goog-Api-Key", apiKey);
        httpClient.DefaultRequestHeaders.Add("ContentType", "application/json");
        httpClient.DefaultRequestHeaders.Add("X-Goog-FieldMask", "routes.duration,routes.distanceMeters");
            
        var routeRequest = new RouteRequest(new Origin(addresses.OriginAddress), 
                                                new Destination(addresses.DestinationAddress),
                                                travelMode: "DRIVE", 
                                                routingPreference: "TRAFFIC_AWARE", 
                                                computeAlternativeRoutes: true, 
                                                new Routemodifiers(avoidTolls: false, avoidHighways: false, avoidFerries: false), 
                                                languageCode: "en-US", 
                                                units: "UNITS_UNSPECIFIED");
        
        HttpResponseMessage response = await httpClient.PostAsJsonAsync(apiUrl, routeRequest);
        var discoveredRoutes = new DiscoveredRoutes(addresses.OriginAddress, addresses.DestinationAddress, string.Empty, Array.Empty<Route>());
        
        if (!response.IsSuccessStatusCode) 
        {
            discoveredRoutes.Message = $"Error: {response.StatusCode} - {response.ReasonPhrase}";
            return discoveredRoutes;
        }
        
        var responseBody = await response.Content.ReadAsStringAsync();
        var routes = System.Text.Json.JsonSerializer.Deserialize<Routes>(responseBody);
        
        if (routes is null)
        {
            discoveredRoutes.Message = "Failed to deserialize response to Routes.";
        }
        
        if (routes?.routes is null)
        {
            discoveredRoutes.Message = "Failed to deserialize response to Routes or routes array is null.";
        }
        
        discoveredRoutes.Routes = routes?.routes ?? Array.Empty<Route>();
        discoveredRoutes.Message = $"Number of routes found:{discoveredRoutes.Routes.Length}";
        
        return discoveredRoutes;
    }
}