using PetParkModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

var connectionString = config.GetConnectionString("PetParkDb");

var options = new DbContextOptionsBuilder<PetParkContext>()
    .UseSqlServer(connectionString)
    .Options;

using var db = new PetParkContext(options);

// Startup query – display all pets and the tricks they know
var allPets = db.Pets
    .Include(p => p.Tricks)
    .OrderBy(p => p.Name)
    .ToList();

Console.WriteLine("=== PetPark – Pets and Their Tricks ===");
foreach (var pet in allPets)
{
    Console.WriteLine($"{pet.Name} ({pet.Species}) knows:");
    if (pet.Tricks.Count == 0)
        Console.WriteLine("   (no tricks yet)");
    else
        foreach (var trick in pet.Tricks)
            Console.WriteLine($"   - {trick.Name} [{trick.DifficultyLevel}]");
}

Console.WriteLine();
Console.WriteLine($"Total Pets: {db.Pets.Count()}  |  Total Tricks: {db.Tricks.Count()}");
Console.WriteLine("----------------------------------------");
Console.WriteLine("Commands: pets | tricks | links | inserts | deletes | exit");

while (true)
{
    Console.Write("Command> ");
    var input = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(input) || input.Equals("exit", StringComparison.OrdinalIgnoreCase))
        break;

    try
    {
        switch (input.ToLower())
        {
            case "pets":
                var pets = db.Pets
                    .AsNoTracking()
                    .Include(p => p.Tricks)
                    .ToList();
                foreach (var p in pets)
                {
                    Console.WriteLine($"{p.PetId}: {p.Name} ({p.Species})");
                    foreach (var t in p.Tricks)
                        Console.WriteLine($"\t- {t.Name} [{t.DifficultyLevel}]");
                }
                break;

            case "tricks":
                var tricks = db.Tricks
                    .AsNoTracking()
                    .Include(t => t.Pets)
                    .ToList();
                foreach (var t in tricks)
                {
                    Console.WriteLine($"{t.TrickId}: {t.Name} [{t.DifficultyLevel}]");
                    foreach (var p in t.Pets)
                        Console.WriteLine($"\t- {p.Name} ({p.Species})");
                }
                break;

            case "links":
                var linked = db.Pets
                    .AsNoTracking()
                    .Include(p => p.Tricks)
                    .ToList();
                foreach (var p in linked)
                    foreach (var t in p.Tricks)
                        Console.WriteLine($"{p.Name} knows {t.Name}");
                break;

            case "inserts":
                var newPet = new Pet { Name = "Max", Species = "Rabbit" };
                var newTrick = new Trick { Name = "Spin", DifficultyLevel = "Easy" };
                db.Pets.Add(newPet);
                db.Tricks.Add(newTrick);
                db.SaveChanges();
                newPet.Tricks.Add(newTrick);
                db.SaveChanges();
                Console.WriteLine($"Inserted {newPet.Name} and {newTrick.Name}.");
                break;

            case "deletes":
                var lastPet = db.Pets
                    .Include(p => p.Tricks)
                    .OrderBy(p => p.PetId).Last();
                db.Pets.Remove(lastPet);
                db.SaveChanges();
                Console.WriteLine($"Deleted {lastPet.Name}.");
                break;

            default:
                Console.WriteLine("Unknown command. Try: pets | tricks | links | inserts | deletes | exit");
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}