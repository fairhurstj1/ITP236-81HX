//#define Logging
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetParkModel;

/// <summary>
/// Creates <see cref="PetParkContext"/> instances at design time for EF Core tooling.
/// </summary>
public class PetParkContextFactory : IDesignTimeDbContextFactory<PetParkContext>
{
    /// <summary>
    /// Creates a configured <see cref="PetParkContext"/> using appsettings.json.
    /// </summary>
    /// <param name="args">Design-time arguments supplied by EF Core tools.</param>
    /// <returns>A configured <see cref="PetParkContext"/> instance.</returns>
    public PetParkContext CreateDbContext(string[] args)
    {
        // EF tools call this at design time, so load configuration from appsettings files.
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        var connectionString = config.GetConnectionString("PetParkDb");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            connectionString = Environment.GetEnvironmentVariable("PETPARKDB_CONNECTION");
        }

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "No PetPark connection string configured. Set ConnectionStrings:PetParkDb or PETPARKDB_CONNECTION.");
        }

        var optionsBuilder = new DbContextOptionsBuilder<PetParkContext>();
        optionsBuilder.UseSqlServer(connectionString);

#if Logging
        // Optional SQL command logging for debugging and grading demos.
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder
                .AddFilter((category, level) =>
                    category == DbLoggerCategory.Database.Command.Name &&
                    level == LogLevel.Information)
                .AddConsole();
        });

        return new PetParkContext(optionsBuilder.Options, loggerFactory);
#else
        return new PetParkContext(optionsBuilder.Options);
#endif
    }
}
