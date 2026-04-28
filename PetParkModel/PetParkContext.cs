using Microsoft.EntityFrameworkCore;

namespace PetParkModel;

public class PetParkContext : DbContext
{
    public PetParkContext(DbContextOptions<PetParkContext> options)
        : base(options)
    {
    }

    public DbSet<Pet> Pets => Set<Pet>();
    public DbSet<Trick> Tricks => Set<Trick>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("PetPark");
        SeedData.Seed(modelBuilder);
    }
}
