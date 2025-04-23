using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using SoftwareProject.Data;
using SoftwareProject.Providers;
using SoftwareProject.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(sp => new HttpClient 
{ 
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) 
});

builder.Services.AddDbContextFactory<ChatbotDbContext>((DbContextOptionsBuilder options) =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ChatbotDbConnection")));
builder.Services.AddScoped<AuthenticationStateProvider, CookieAuthStateProvider>();
builder.Services.AddTransient<AccountService>();
builder.Services.AddTransient<ApiService>();
builder.Services.AddAuthorizationCore();

builder.Services.AddMudServices();

await builder.Build().RunAsync();