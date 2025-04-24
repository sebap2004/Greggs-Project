using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Data.SqlClient;
using MudBlazor.Services;
using SoftwareProject.Data;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using SoftwareProject.Client.Providers;
using SoftwareProject.Services;
using SoftwareProject.Components;

var builder = WebApplication.CreateBuilder(args);

// Add MudBlazor services
builder.Services.AddMudServices();
builder.Services.AddMudMarkdownServices();

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

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.WithOrigins("https://localhost:3000") // Specify exact origins instead of AllowAnyOrigin
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials(); // Allow cookies
    });
});

// Update your cookie configuration
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.Cookie.Name = "auth_token";
        options.Cookie.MaxAge = TimeSpan.FromDays(1);
        options.ExpireTimeSpan = TimeSpan.FromDays(1);
        options.Cookie.Path = "/";
        options.LoginPath = "/login";
        
        options.Cookie.HttpOnly = true; // Security best practice
        
        options.SessionStore = null; // Don't use session store - store in cookie
        
        options.Events = new CookieAuthenticationEvents
        {
            OnRedirectToLogin = context =>
            {
                context.Response.StatusCode = 401;
                Console.WriteLine("OnRedirectToLogin triggered");
                return Task.CompletedTask;
            },
            // Other events...
        };
    });

// Add explicit CookiePolicy configuration
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => false; // Don't require consent
    options.MinimumSameSitePolicy = SameSiteMode.Lax;
    // In development, allow insecure cookies for localhost
    options.Secure = builder.Environment.IsDevelopment() 
        ? CookieSecurePolicy.SameAsRequest 
        : CookieSecurePolicy.Always;
});
builder.Services.AddAuthorization();
builder.Services.AddScoped<AuthenticationStateProvider, CookieAuthStateProvider>();
builder.Services.AddAntiforgery(options => {
    options.HeaderName = "X-CSRF-TOKEN";
    options.SuppressXFrameOptionsHeader = false;
});
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddHttpContextAccessor();

// Add database configurations.
builder.Services.AddDbContextFactory<ChatbotDbContext>((DbContextOptionsBuilder options) =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ChatbotDbConnection")));
builder.Services.AddTransient<AccountService>();
builder.Services.AddTransient<TopicService>();
builder.Services.AddTransient<ApiService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCookiePolicy();
app.UseRouting();
app.UseCors("AllowAll"); // Make sure this comes after UseRouting but before UseAuthentication
app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapControllers();
app.MapStaticAssets();

app.MapRazorComponents<App>()
       .AddInteractiveWebAssemblyRenderMode()
       .AddAdditionalAssemblies(typeof(SoftwareProject.Client._Imports).Assembly);



app.Run();