using IndexedDB.Blazor;
using Magic.IndexedDb;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using MudBlazor.Services;
using SoftwareProject.Client.Clients;
using SoftwareProject.Client.Interfaces;
using SoftwareProject.Client.Providers;
using SoftwareProject.Client.Services;
using SoftwareProject.Data;
using SoftwareProject.Interfaces;
using SoftwareProject.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(sp => new HttpClient 
{ 
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) 
});


builder.Services.AddDbContextFactory<ChatbotDbContext>((DbContextOptionsBuilder options) =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ChatbotDbConnection")));

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IIndexedDbFactory, IndexedDbFactory>();
builder.Services.AddMagicBlazorDB(BlazorInteropMode.WASM, builder.HostEnvironment.IsDevelopment());
builder.Services.AddTransient<AccountService>();
builder.Services.AddTransient<ApiService>();
builder.Services.AddScoped<IMessageService, MessageClient>();
builder.Services.AddScoped<ITopicService, TopicClient>();
builder.Services.AddMudMarkdownServices();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CookieAuthStateProvider>();

builder.Services.AddMudServices();

await builder.Build().RunAsync();