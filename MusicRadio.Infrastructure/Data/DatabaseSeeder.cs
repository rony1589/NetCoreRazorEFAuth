using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;



namespace MusicRadio.Infrastructure.Data
{

    #pragma warning disable CA2254
    public static class DatabaseSeeder
    {
        public static async Task SeedProceduresAsync(MusicRadioDbContext context, ILogger logger)
        {
            try
            {

                // Opción 1 (alternativa): Usar archivos físicos
                await SeedFromPhysicalFiles(context, logger);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error en DatabaseSeeder");
                throw;
            }
        }

        private static async Task SeedFromPhysicalFiles(MusicRadioDbContext context, ILogger logger)
        {
            var scriptsPath = Path.Combine(AppContext.BaseDirectory, "StoredProcedure");
            logger.LogInformation($"Buscando scripts en: {scriptsPath}");

            if (!Directory.Exists(scriptsPath))
            {
                logger.LogError($"Directorio no encontrado: {scriptsPath}");
                return;
            }

            var scriptFiles = Directory.GetFiles(scriptsPath, "*.sql", SearchOption.AllDirectories)
                                     .OrderBy(f => f);

            foreach (var filePath in scriptFiles)
            {
                logger.LogInformation($"\nProcesando: {Path.GetFileName(filePath)}");

                try
                {
                    var sql = await File.ReadAllTextAsync(filePath);
                    await context.Database.ExecuteSqlRawAsync(sql);
                    logger.LogInformation("✅ Script ejecutado correctamente");
                }
                catch (Exception ex)
                {
                    logger.LogError($"❌ Error al ejecutar: {ex.Message}");
                    throw;
                }
            }
        }
    }
}
