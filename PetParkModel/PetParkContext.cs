using Microsoft.EntityFrameworkCore;

namespace PetParkModel;

/// <summary>
/// EF Core database context for the PetPark domain model.
/// </summary>
public class PetParkContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PetParkContext"/> class.
    /// </summary>
    /// <param name="options">Context configuration options.</param>
    public PetParkContext(DbContextOptions<PetParkContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Gets the pets table.
    /// </summary>
    public DbSet<Pet> Pets => Set<Pet>();

    /// <summary>
    /// Gets the tricks table.
    /// </summary>
    public DbSet<Trick> Tricks => Set<Trick>();

    /// <summary>
    /// Configures model metadata including schema and seed data.
    /// </summary>
    /// <param name="modelBuilder">The model builder used to configure entities.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("PetPark");
        SeedData.Seed(modelBuilder);
    }
}
