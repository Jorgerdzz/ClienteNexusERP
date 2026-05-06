using Microsoft.AspNetCore.Authentication.Cookies;
using NexusERP.Enums;
using NexusERP.Helpers;
using NexusERP.Services;

QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
// Añade esta línea para registrar tu Helper
builder.Services.AddTransient<HelperSessionContextAccessor>();

// ACTIVAMOS LA SESIÓN (Vital para guardar el Token JWT más adelante)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60); // Caducidad de la sesión
});

builder.Services.AddControllersWithViews().AddSessionStateTempDataProvider();

// MANTENEMOS LAS COOKIES: El frontend necesita su propio login para proteger las vistas MVC
builder.Services.AddAuthentication(options =>
{
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, config =>
{
    config.LoginPath = "/Account/LogIn";
    config.AccessDeniedPath = "/Account/AccessDenied";
});

builder.Services.AddAuthorization(options =>
{
    // Políticas para ocultar/mostrar elementos en las Vistas y proteger Controladores MVC
    options.AddPolicy("ADMIN", policy => policy.RequireRole(RolesUsuario.Admin.ToString()));
    options.AddPolicy("EMPLEADO", policy => policy.RequireRole(RolesUsuario.Empleado.ToString()));
    options.AddPolicy("DESCARGARPDF", policy => policy.RequireRole(
        RolesUsuario.Empleado.ToString(),
        RolesUsuario.Admin.ToString()
    ));
});

// =======================================================
// 🚀 NUEVO MOTOR DE COMUNICACIÓN CON LA API
// =======================================================
string apiBaseUrl = builder.Configuration.GetValue<string>("ApiUrls:ApiNexusERP");

builder.Services.AddHttpClient<ServiceApi>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});

builder.Services.AddTransient<ServiceAuth>();
builder.Services.AddTransient<ServiceBusqueda>();
builder.Services.AddTransient<ServiceClientes>();
builder.Services.AddTransient<ServiceContabilidad>();
builder.Services.AddTransient<ServiceDepartamentos>();
builder.Services.AddTransient<ServiceEmpresas>();
builder.Services.AddTransient<ServiceEstadisticas>();
builder.Services.AddTransient<ServiceFacturacion>();
builder.Services.AddTransient<ServiceNominas>();
builder.Services.AddTransient<ServiceUsuarios>();
builder.Services.AddTransient<ServiceEmpleados>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    // COMENTADO TEMPORALMENTE PARA LECTURA DE ERRORES EN PRODUCCIÓN
    //app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// FORZAR LA PANTALLA DE ERROR DE DESARROLLADOR
app.UseDeveloperExceptionPage();

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();