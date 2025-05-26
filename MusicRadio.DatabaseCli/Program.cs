using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MusicRadio.Infrastructure.Data;
using System.Reflection;

#pragma warning disable CA2254
// 1. Configurar el builder con la ruta correcta a appsettings.json
var builder = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        var env = hostingContext.HostingEnvironment;
        var assemblyLocation = Assembly.GetExecutingAssembly().Location;
        var assemblyDirectory = Path.GetDirectoryName(assemblyLocation);

        // Buscar appsettings.json en varias ubicaciones posibles
        var appSettingsPaths = new[]
        {
            Path.Combine(assemblyDirectory??String.Empty, "appsettings.json"), // Para publicación
            Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"), // Para desarrollo
            Path.Combine(AppContext.BaseDirectory, "appsettings.json") // Ubicación alternativa
        };

        foreach (var path in appSettingsPaths)
        {
            if (File.Exists(path))
            {
                config.AddJsonFile(path, optional: false, reloadOnChange: true);
                Console.WriteLine($"Configuración cargada desde: {path}");
                break;
            }
        }
    })
    .ConfigureServices((context, services) =>
    {
        // Configuración del DbContext
        services.AddDbContext<MusicRadioDbContext>(options =>
        {
            var connectionString = context.Configuration.GetConnectionString("MusicRadioDbConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("Cadena de conexión no encontrada en la configuración");
            }

            Console.WriteLine($"Cadena de conexión: {connectionString}");
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);
            });
        });

        services.AddLogging(configure => configure.AddConsole());
    });

var host = builder.Build();

try
{
    Console.WriteLine("Iniciando actualización de procedimientos almacenados...");

    using var scope = host.Services.CreateScope();
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    var context = services.GetRequiredService<MusicRadioDbContext>();

    // Verificar conexión
    logger.LogInformation("Probando conexión a la base de datos...");
    try
    {
        var canConnect = await context.Database.CanConnectAsync();
        logger.LogInformation($"Conexión {(canConnect ? "exitosa" : "fallida")}");

        if (!canConnect)
        {
            throw new Exception("No se pudo conectar a la base de datos");
        }
    }
    catch (Exception ex)
    {
        logger.LogError($"Error de conexión: {ex.Message}");
        var conn = context.Database.GetDbConnection();
        logger.LogError($"Detalles: {conn.DataSource}/{conn.Database}, Estado: {conn.State}");
        throw;
    }

    // Ejecutar seeders
    await DatabaseSeeder.SeedProceduresAsync(context, logger);

    logger.LogInformation("✅ Procedimientos almacenados actualizados correctamente");
    return 0;
}
catch (Exception ex)
{
    var logger = host.Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "❌ Error durante la ejecución");
    return 1;
}