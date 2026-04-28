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
Console.WriteLine("Commands: pets | tricks | links | inserts | deletes | cleanup | exit");

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
                var pet = db.Pets
                    .Include(p => p.Tricks)
                    .FirstOrDefault(p => p.Name == "Max" && p.Species == "Rabbit");

                if (pet is null)
                {
                    pet = new Pet { Name = "Max", Species = "Rabbit" };
                    db.Pets.Add(pet);
                }

                var trick = db.Tricks
                    .FirstOrDefault(t => t.Name == "Spin" && t.DifficultyLevel == "Easy");

                if (trick is null)
                {
                    trick = new Trick { Name = "Spin", DifficultyLevel = "Easy" };
                    db.Tricks.Add(trick);
                }

                var hasLink = pet.Tricks.Any(t => t.Name == trick.Name && t.DifficultyLevel == trick.DifficultyLevel);
                if (!hasLink)
                {
                    pet.Tricks.Add(trick);
                }

                db.SaveChanges();
                Console.WriteLine($"Insert complete: {pet.Name} and {trick.Name} are in the database, and their link is set.");
                break;

            case "deletes":
                var lastPet = db.Pets
                    .Include(p => p.Tricks)
                    .OrderBy(p => p.PetId)
                    .LastOrDefault();
                if (lastPet is null)
                {
                    Console.WriteLine("No pets available to delete.");
                    break;
                }
                db.Pets.Remove(lastPet);
                db.SaveChanges();
                Console.WriteLine($"Deleted {lastPet.Name}.");
                break;

            case "cleanup":
                var removedPets = 0;
                var removedTricks = 0;

                // Keep lowest PetId for duplicate Max (Rabbit) records and move links before delete.
                var duplicateMaxPets = db.Pets
                    .Include(p => p.Tricks)
                    .Where(p => p.Name == "Max" && p.Species == "Rabbit")
                    .OrderBy(p => p.PetId)
                    .ToList();

                if (duplicateMaxPets.Count > 1)
                {
                    var keepPet = duplicateMaxPets.First();
                    foreach (var duplicatePet in duplicateMaxPets.Skip(1))
                    {
                        foreach (var dtrick in duplicatePet.Tricks.ToList())
                        {
                            if (!keepPet.Tricks.Any(t => t.TrickId == dtrick.TrickId))
                            {
                                keepPet.Tricks.Add(dtrick);
                            }
                        }

                        db.Pets.Remove(duplicatePet);
                        removedPets++;
                    }
                }

                // Keep lowest TrickId for duplicate Spin (Easy) records and move links before delete.
                var duplicateSpinTricks = db.Tricks
                    .Include(t => t.Pets)
                    .Where(t => t.Name == "Spin" && t.DifficultyLevel == "Easy")
                    .OrderBy(t => t.TrickId)
                    .ToList();

                if (duplicateSpinTricks.Count > 1)
                {
                    var keepTrick = duplicateSpinTricks.First();
                    foreach (var duplicateTrick in duplicateSpinTricks.Skip(1))
                    {
                        foreach (var petRef in duplicateTrick.Pets.ToList())
                        {
                            if (!petRef.Tricks.Any(t => t.TrickId == keepTrick.TrickId))
                            {
                                petRef.Tricks.Add(keepTrick);
                            }
                        }

                        db.Tricks.Remove(duplicateTrick);
                        removedTricks++;
                    }
                }

                db.SaveChanges();
                Console.WriteLine($"Cleanup complete. Removed duplicate pets: {removedPets}; removed duplicate tricks: {removedTricks}.");
                break;

            default:
                Console.WriteLine("Unknown command. Try: pets | tricks | links | inserts | deletes | cleanup | exit");
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}