using Magic.IndexedDb;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Data.SqlClient;
using MudBlazor.Services;
using SoftwareProject.Data;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using SoftwareProject.Client.Interfaces;
using SoftwareProject.Client.Providers;
using SoftwareProject.Client.Services;
using SoftwareProject.Services;
using SoftwareProject.Components;
using SoftwareProject.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add MudBlazor services
builder.Services.AddMudServices();
builder.Services.AddMudMarkdownServices();
builder.Services.AddMagicBlazorDB(BlazorInteropMode.WASM, true);

// Added HTTP client handler
builder.Services.AddScoped(sp => 
{
    var httpClientHandler = new HttpClientHandler
    {
        UseCookies = true,
        CookieContainer = new System.Net.CookieContainer(),
        UseDefaultCredentials = false,
    };
    
    var httpClient = new HttpClient(httpClientHandler)
    {
        BaseAddress = new Uri(builder.Configuration["BaseUrl:ApiUrl"] ?? string.Empty)
    };
    
    return httpClient;
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

// Adds API controllers to services.
builder.Services.AddControllers();

// Adds cookie authentication.
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.Cookie.Name = "auth_token";
        options.Cookie.MaxAge = TimeSpan.FromDays(1);
        options.ExpireTimeSpan = TimeSpan.FromDays(1);
        options.Cookie.Path = "/";
        options.LoginPath = "/login";
        options.Cookie.HttpOnly = true; 
        options.SessionStore = null; 
        options.Events = new CookieAuthenticationEvents
        {
            OnRedirectToLogin = context =>
            {
                context.Response.StatusCode = 401;
                Console.WriteLine("OnRedirectToLogin triggered");
                return Task.CompletedTask;
            },
        };
    });

// Adds authentication and relevant services.
builder.Services.AddAuthorization();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<AuthenticationStateProvider, CookieAuthStateProvider>();
builder.Services.AddAntiforgery(options => {
    options.HeaderName = "X-CSRF-TOKEN";
    options.SuppressXFrameOptionsHeader = false;
});
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<ITopicService, TopicService>();
builder.Services.AddScoped<ISettingsService, SettingsService>();
builder.Services.AddDbContextFactory<ChatbotDbContext>((DbContextOptionsBuilder options) =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ChatbotDbConnection")));
builder.Services.AddTransient<ApiService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCookiePolicy();
app.UseRouting();
app.UseCors("AllowAll"); 
app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapControllers();
app.MapStaticAssets();

app.MapRazorComponents<App>()
       .AddInteractiveWebAssemblyRenderMode()
       .AddAdditionalAssemblies(typeof(SoftwareProject.Client._Imports).Assembly);



app.Run();
public partial class Program { }