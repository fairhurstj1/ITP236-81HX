using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace PetParkModel;

/// <summary>
/// EF Core database context for the PetPark domain model.
/// </summary>
public class PetParkContext : DbContext
{
    private readonly ILoggerFactory? _loggerFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="PetParkContext"/> class.
    /// </summary>
    /// <param name="options">Context configuration options.</param>
    public PetParkContext(DbContextOptions<PetParkContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PetParkContext"/> class with optional logging.
    /// </summary>
    /// <param name="options">Context configuration options.</param>
    /// <param name="loggerFactory">Optional logger factory used for SQL logging.</param>
    public PetParkContext(DbContextOptions<PetParkContext> options, ILoggerFactory loggerFactory)
        : base(options)
    {
        _loggerFactory = loggerFactory;
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
    /// Gets the owners table.
    /// </summary>
    public DbSet<Owner> Owners => Set<Owner>();

    /// <summary>
    /// Gets the treats table.
    /// </summary>
    public DbSet<Treat> Treats => Set<Treat>();

    /// <summary>
    /// Gets the pet-treat bridge table.
    /// </summary>
    public DbSet<PetTreat> PetTreats => Set<PetTreat>();

    /// <summary>
    /// Configures optional provider logging when a logger factory is supplied.
    /// </summary>
    /// <param name="optionsBuilder">The options builder used to configure the context.</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (_loggerFactory is not null)
        {
            optionsBuilder.UseLoggerFactory(_loggerFactory);
            optionsBuilder.EnableSensitiveDataLogging();
        }
    }

    /// <summary>
    /// Configures model metadata including schema and seed data.
    /// </summary>
    /// <param name="modelBuilder">The model builder used to configure entities.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("PetPark");

        // One owner can have many pets.
        modelBuilder.Entity<Pet>()
            .HasOne(p => p.Owner)
            .WithMany(o => o.Pets)
            .HasForeignKey(p => p.OwnerId)
            .OnDelete(DeleteBehavior.Cascade);

        // Composite key for the explicit Pet-Treat bridge entity.
        modelBuilder.Entity<PetTreat>()
            .HasKey(pt => new { pt.PetId, pt.TreatId });

        // PetTreat depends on Pet.
        modelBuilder.Entity<PetTreat>()
            .HasOne(pt => pt.Pet)
            .WithMany(p => p.PetTreats)
            .HasForeignKey(pt => pt.PetId)
            .OnDelete(DeleteBehavior.Cascade);

        // PetTreat depends on Treat.
        modelBuilder.Entity<PetTreat>()
            .HasOne(pt => pt.Treat)
            .WithMany(t => t.PetTreats)
            .HasForeignKey(pt => pt.TreatId)
            .OnDelete(DeleteBehavior.Cascade);

        SeedData.Seed(modelBuilder);
    }
}
