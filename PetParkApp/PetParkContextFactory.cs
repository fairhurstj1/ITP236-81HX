using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
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
        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var connectionString = config.GetConnectionString("PetParkDb");

        var optionsBuilder = new DbContextOptionsBuilder<PetParkContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new PetParkContext(optionsBuilder.Options);
    }
}
