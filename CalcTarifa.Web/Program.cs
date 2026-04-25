using System.Globalization;
using CalcTarifa.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Cliente HTTP tipado hacia la API REST
builder.Services.AddHttpClient<IApiClientService, ApiClientService>(client =>
{
    var baseUrl = builder.Configuration["ApiSettings:BaseUrl"] ?? "https://localhost:61168/";
    client.BaseAddress = new Uri(baseUrl);
    client.Timeout     = TimeSpan.FromSeconds(30);
})
.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    // Solo ignorar errores SSL si estamos en desarrollo
    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => builder.Environment.IsDevelopment()
});

builder.Services.AddAuthentication(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(opts =>
    {
        opts.LoginPath    = "/Auth/Login";
        opts.LogoutPath   = "/Auth/Logout";
        opts.AccessDeniedPath = "/Auth/AccessDenied";
        opts.Cookie.Name  = "CalcTarifa.Auth";
        opts.ExpireTimeSpan = TimeSpan.FromHours(1);
    });

var app = builder.Build();

// Forzar cultura invariante para evitar problemas con separadores decimales (. vs ,)
CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

var supportedCultures = new[] { "en-US" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture("en-US")
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);
app.UseRequestLocalization(localizationOptions);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
app.Run();
