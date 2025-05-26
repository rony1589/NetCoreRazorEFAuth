using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MusicRadio.Application.Mapping;
using MusicRadio.Infrastructure;
using MusicRadio.Infrastructure.Data;
using MusicRadio.Web.Mapping;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Globalization;


var cultureInfo = new CultureInfo("es-ES");
cultureInfo.NumberFormat.CurrencyDecimalSeparator = "."; // Fuerza el punto como separador decimal
cultureInfo.NumberFormat.NumberDecimalSeparator = ".";   // También afecta los números en general

CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

var builder = WebApplication.CreateBuilder(args);

// Configuración de la cadena de conexión
builder.Services.AddDbContext<MusicRadioDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MusicRadioDbConnection"),
        sqlOptions =>
        {
            sqlOptions.MigrationsAssembly("MusicRadio.Infrastructure");
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);
        });
});

// Inyección de dependencias de infraestructura
builder.Services.AddInfrastructure();

// Configuración de AutoMapper
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<MappingProfile>();      // Perfil de Application
    config.AddProfile<WebMappingProfile>();   // Perfil de Web
});



// Configuración de antiforgery
builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN";
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

// Configuración de autenticación combinada (Cookies + JWT)
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme; // Para Razor Pages
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;  // Para APIs
})
.AddCookie(options => // Configuración para Razor Pages
{
    options.Cookie.Name = "MusicRadio.Auth";
    options.LoginPath = "/Auth/Login";
    options.LogoutPath = "/Auth/Logout";
    options.AccessDeniedPath = "/Error";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.SlidingExpiration = true;
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
})
.AddJwtBearer(options => // Configuración para APIs
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

// Configuración de autorización
builder.Services.AddAuthorization();

// Configuración de Razor Pages
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/"); // Todas las páginas requieren auth
    options.Conventions.AllowAnonymousToPage("/Index");
    options.Conventions.AllowAnonymousToPage("/Auth/Login");
    options.Conventions.AllowAnonymousToPage("/Auth/Register");
    options.Conventions.AllowAnonymousToPage("/Error");
    options.Conventions.AllowAnonymousToPage("/Auth/Logout");
})
.AddViewOptions(options =>
{
    options.HtmlHelperOptions.ClientValidationEnabled = true;
});

// Configuración de sesión
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

// Configuración de memoria caché
builder.Services.AddMemoryCache();

// Configuración del HttpContext
builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton(new JsonSerializerOptions
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
});


var app = builder.Build();

// Configuración del middleware
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture(cultureInfo),
    SupportedCultures = new List<CultureInfo> { cultureInfo },
    SupportedUICultures = new List<CultureInfo> { cultureInfo }
});

// Configuración del pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Middleware para headers de seguridad
app.Use(async (context, next) =>
{
    context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Append("X-Frame-Options", "DENY");
    await next();
});

app.UseRouting();

// Configuración de cookies
app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always
});

app.UseSession();
app.UseAuthentication(); // Debe ir antes de UseAuthorization
app.UseAuthorization();

// Configurar ruta por defecto
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.MapRazorPages();

app.Run();