using Microsoft.EntityFrameworkCore;

namespace PetParkModel;

public static class SeedData
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pet>().HasData(
            new Pet { PetId =  1, Name = "Buddy",   Species = "Dog"    },
            new Pet { PetId =  2, Name = "Luna",    Species = "Cat"    },
            new Pet { PetId = 10, Name = "Mango",   Species = "Parrot" },
            new Pet { PetId = 11, Name = "Shadow",  Species = "Dog"    },
            new Pet { PetId = 12, Name = "Cleo",    Species = "Rabbit" }
        );

        modelBuilder.Entity<Trick>().HasData(
            new Trick { TrickId =  1, Name = "Sit",        DifficultyLevel = "Easy"   },
            new Trick { TrickId =  2, Name = "Roll Over",  DifficultyLevel = "Medium" },
            new Trick { TrickId = 10, Name = "Shake",      DifficultyLevel = "Easy"   },
            new Trick { TrickId = 11, Name = "Play Dead",  DifficultyLevel = "Hard"   },
            new Trick { TrickId = 12, Name = "Fetch",      DifficultyLevel = "Medium" }
        );

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
