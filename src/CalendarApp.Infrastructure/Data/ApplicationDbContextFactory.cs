namespace CalendarApp.Infrastructure.Data;

using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var basePath = Directory.GetCurrentDirectory();

        var candidatePaths = new[]
        {
            basePath,
            Path.Combine(basePath, "src", "CalendarApp.Web"),
            Path.Combine(basePath, "..", "CalendarApp.Web"),
            Path.Combine(basePath, "..", "..", "src", "CalendarApp.Web")
        };

        string? settingsDir = null;
        foreach (var path in candidatePaths)
        {
            if (File.Exists(Path.Combine(path, "appsettings.json")))
            {
                settingsDir = Path.GetFullPath(path);
                break;
            }
        }

        var configBuilder = new ConfigurationBuilder();
        if (settingsDir != null)
        {
            configBuilder.SetBasePath(settingsDir);
        }
        else
        {
            configBuilder.SetBasePath(basePath);
        }

        var configuration = configBuilder
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? "Server=(localdb)\\mssqllocaldb;Database=CalendarApp;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
