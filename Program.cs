using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PokemonTcgApp;
using PokemonTcgApp.Services;

// Create the default WebAssembly host builder
var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Register the root components
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Register services for dependency injection
// Add HttpClient with base address from the current environment
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Register the Pokemon TCG API service
builder.Services.AddScoped<PokemonTcgService>();

// Register the Favorites service for managing favorite cards
builder.Services.AddScoped<FavoritesService>();

// Build and run the application
await builder.Build().RunAsync();

