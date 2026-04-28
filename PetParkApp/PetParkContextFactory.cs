using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using PetParkModel;

public class PetParkContextFactory : IDesignTimeDbContextFactory<PetParkContext>
{
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
