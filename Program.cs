using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddSingleton<GoogleRouteService>();
var app = builder.Build();
IConfiguration config = app.Configuration;
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseHttpsRedirection();
app.MapPost("/getdistanceinfo", (Addresses addresses, GoogleRouteService googleRouteService) =>
{
    var apiUrl = config["googleRoutesApi:apiUrl"] ?? throw new InvalidOperationException("URL key, googleRouteApiUrl, not found.");
    var apiKey = config["googleRoutesApi:apiKey"] ?? throw new InvalidOperationException("API key, googleRouteApiKey, not found in user secrets.");
    var response = googleRouteService.GetRouteInfo(addresses, apiUrl, apiKey);
    return response;
}).WithName("GetDistanceInfo");

app.Run();