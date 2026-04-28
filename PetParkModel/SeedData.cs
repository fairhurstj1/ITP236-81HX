using Microsoft.EntityFrameworkCore;

namespace PetParkModel;

/// <summary>
/// Provides model seed data for pets, tricks, and their many-to-many links.
/// </summary>
public static class SeedData
{
    /// <summary>
    /// Applies static seed data to the EF Core model.
    /// </summary>
    /// <param name="modelBuilder">The model builder used to configure seed entries.</param>
    public static void Seed(ModelBuilder modelBuilder)
    {
        // Parent data for the Owner-Pet one-to-many relationship.
        modelBuilder.Entity<Owner>().HasData(
            new Owner { OwnerId = 1, FullName = "Alex Carter" },
            new Owner { OwnerId = 2, FullName = "Jordan Blake" },
            new Owner { OwnerId = 3, FullName = "Taylor Morgan" }
        );

        // Core Pet rows reference seeded owners.
        modelBuilder.Entity<Pet>().HasData(
            new Pet { PetId =  1, Name = "Buddy",   Species = "Dog",    OwnerId = 1 },
            new Pet { PetId =  2, Name = "Luna",    Species = "Cat",    OwnerId = 2 },
            new Pet { PetId = 10, Name = "Mango",   Species = "Parrot", OwnerId = 1 },
            new Pet { PetId = 11, Name = "Shadow",  Species = "Dog",    OwnerId = 3 },
            new Pet { PetId = 12, Name = "Cleo",    Species = "Rabbit", OwnerId = 2 }
        );

        modelBuilder.Entity<Trick>().HasData(
            new Trick { TrickId =  1, Name = "Sit",        DifficultyLevel = "Easy"   },
            new Trick { TrickId =  2, Name = "Roll Over",  DifficultyLevel = "Medium" },
            new Trick { TrickId = 10, Name = "Shake",      DifficultyLevel = "Easy"   },
            new Trick { TrickId = 11, Name = "Play Dead",  DifficultyLevel = "Hard"   },
            new Trick { TrickId = 12, Name = "Fetch",      DifficultyLevel = "Medium" }
        );

        // Lookup rows for Treat.
        modelBuilder.Entity<Treat>().HasData(
            new Treat { TreatId = 1, Name = "Peanut Biscuit", Calories = 40 },
            new Treat { TreatId = 2, Name = "Salmon Bite", Calories = 30 },
            new Treat { TreatId = 3, Name = "Carrot Chip", Calories = 10 }
        );

        // Bridge rows include extra properties required by the assignment.
        modelBuilder.Entity<PetTreat>().HasData(
            new PetTreat { PetId = 1, TreatId = 1, DateGiven = new DateTime(2026, 4, 1), Quantity = 2 },
            new PetTreat { PetId = 2, TreatId = 2, DateGiven = new DateTime(2026, 4, 2), Quantity = 1 },
            new PetTreat { PetId = 10, TreatId = 3, DateGiven = new DateTime(2026, 4, 3), Quantity = 3 },
            new PetTreat { PetId = 11, TreatId = 1, DateGiven = new DateTime(2026, 4, 4), Quantity = 1 },
            new PetTreat { PetId = 12, TreatId = 3, DateGiven = new DateTime(2026, 4, 5), Quantity = 2 }
        );

        // Existing implicit Pet-Trick many-to-many seed data.
        modelBuilder.Entity<Pet>()
            .HasMany(p => p.Tricks)
            .WithMany(t => t.Pets)
            .UsingEntity(j => j.HasData(
                new { PetsPetId =  1, TricksTrickId =  1 },
                new { PetsPetId =  1, TricksTrickId =  2 },
                new { PetsPetId =  1, TricksTrickId = 12 },
                new { PetsPetId =  2, TricksTrickId =  1 },
                new { PetsPetId =  2, TricksTrickId = 10 },
                new { PetsPetId = 10, TricksTrickId = 10 },
                new { PetsPetId = 11, TricksTrickId =  1 },
                new { PetsPetId = 11, TricksTrickId = 11 },
                new { PetsPetId = 11, TricksTrickId = 12 },
                new { PetsPetId = 12, TricksTrickId =  2 }
            ));
    }
}
