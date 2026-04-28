using PetParkModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

var connectionString = config.GetConnectionString("PetParkDb");
if (string.IsNullOrWhiteSpace(connectionString))
{
    connectionString = Environment.GetEnvironmentVariable("PETPARKDB_CONNECTION");
}

if (string.IsNullOrWhiteSpace(connectionString))
{
    Console.WriteLine("Missing connection string.");
    Console.WriteLine("Set ConnectionStrings:PetParkDb in appsettings.json or set PETPARKDB_CONNECTION.");
    return;
}

var options = new DbContextOptionsBuilder<PetParkContext>()
    .UseSqlServer(connectionString)
    .Options;

using var db = new PetParkContext(options);

// Restore any seed data that may have been left dirty by previous demo runs.
RestoreSeedData();

// Stable demo values keep inserts/deletes repeatable for assignment testing.
const string demoOwnerName = "Demo Owner";
const string demoPetName = "Max";
const string demoPetSpecies = "Rabbit";
const string demoTrickName = "Spin";
const string demoTrickDifficulty = "Easy";

// Startup query: display all pets and tricks.
var allPets = db.Pets
    .AsNoTracking()
    .Include(p => p.Tricks)
    .OrderBy(p => p.Name)
    .ToList();

Console.WriteLine("=== PetPark - Pets and Their Tricks ===");
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
Console.WriteLine("Section 4 - CRUD  : inserts | updates | deletes");
Console.WriteLine("Section 4 - Read  : pets | tricks | links");
Console.WriteLine("Section 5 - Track : tracking | notrackfail | notrackfix");
Console.WriteLine("Section 6 - Report: reports");
Console.WriteLine("              exit to quit");

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
            // --- Section 4: Read ---
            case "pets":
                ShowPets();
                break;

            case "tricks":
                ShowTricks();
                break;

            case "links":
                ShowLinks();
                break;

            // --- Section 4: CRUD ---
            case "inserts":
                Console.WriteLine("--- Section 4: Create ---");
                RunInsert();
                break;

            case "updates":
                Console.WriteLine("--- Section 4: Update ---");
                RunUpdate();
                break;

            case "deletes":
                Console.WriteLine("--- Section 4: Delete (cascade observed) ---");
                RunDelete();
                break;

            // --- Section 5: Tracking vs No-Tracking ---
            case "tracking":
                Console.WriteLine("--- Section 5: Tracking Update (succeeds) ---");
                RunTrackingDemo();
                break;

            case "notrackfail":
                Console.WriteLine("--- Section 5: No-Tracking Update (fails) ---");
                RunNoTrackingFailDemo();
                break;

            case "notrackfix":
                Console.WriteLine("--- Section 5: No-Tracking Fix via EntityState.Modified ---");
                RunNoTrackingFixDemo();
                break;

            // --- Section 6: Reporting Queries ---
            case "reports":
                Console.WriteLine("--- Section 6: Reporting Queries ---");
                RunReports();
                break;

            default:
                Console.WriteLine("Unknown command. Use: pets, tricks, links, inserts, updates, deletes, tracking, notrackfail, notrackfix, reports, exit.");
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Command '{input}' failed with {ex.GetType().Name}: {ex.Message}");
    }
}

void ShowPets()
{
    // Read-only query with related trick data.
    var pets = db.Pets
        .AsNoTracking()
        .Include(p => p.Tricks)
        .OrderBy(p => p.PetId)
        .ToList();

    foreach (var pet in pets)
    {
        Console.WriteLine($"{pet.PetId}: {pet.Name} ({pet.Species})");
        foreach (var trick in pet.Tricks)
        {
            Console.WriteLine($"\t- {trick.Name} [{trick.DifficultyLevel}]");
        }
    }
}

void ShowTricks()
{
    // Read-only query from the trick side of the relationship.
    var tricks = db.Tricks
        .AsNoTracking()
        .Include(t => t.Pets)
        .OrderBy(t => t.TrickId)
        .ToList();

    foreach (var trick in tricks)
    {
        Console.WriteLine($"{trick.TrickId}: {trick.Name} [{trick.DifficultyLevel}]");
        foreach (var pet in trick.Pets)
        {
            Console.WriteLine($"\t- {pet.Name} ({pet.Species})");
        }
    }
}

void ShowLinks()
{
    // Flat view of pet-to-trick links.
    var linked = db.Pets
        .AsNoTracking()
        .Include(p => p.Tricks)
        .OrderBy(p => p.Name)
        .ToList();

    foreach (var pet in linked)
    {
        foreach (var trick in pet.Tricks)
        {
            Console.WriteLine($"{pet.Name} knows {trick.Name}");
        }
    }
}

void RunInsert()
{
    // Reuse existing demo owner so repeated runs do not create duplicates.
    var owner = db.Owners
        .Where(o => o.FullName == demoOwnerName)
        .OrderBy(o => o.OwnerId)
        .FirstOrDefault();

    if (owner is null)
    {
        owner = new Owner { FullName = demoOwnerName };
        db.Owners.Add(owner);
    }

    // Reuse existing demo pet if present; otherwise create it.
    var pet = db.Pets
        .Include(p => p.Tricks)
        .FirstOrDefault(p => p.Name == demoPetName && p.Species == demoPetSpecies);

    if (pet is null)
    {
        pet = new Pet { Name = demoPetName, Species = demoPetSpecies, Owner = owner };
        db.Pets.Add(pet);
    }

    // Reuse existing trick if present; otherwise create it.
    var trick = db.Tricks
        .FirstOrDefault(t => t.Name == demoTrickName && t.DifficultyLevel == demoTrickDifficulty);

    if (trick is null)
    {
        trick = new Trick { Name = demoTrickName, DifficultyLevel = demoTrickDifficulty };
        db.Tricks.Add(trick);
    }

    // Create the link only once.
    if (!pet.Tricks.Any(t => t.Name == trick.Name && t.DifficultyLevel == trick.DifficultyLevel))
    {
        pet.Tricks.Add(trick);
    }

    db.SaveChanges();
    Console.WriteLine("Insert complete: owner, pet, trick, and relationship are now in place with no duplicates.");
}

void RunUpdate()
{
    // Update one existing owner in a deterministic way.
    var owner = db.Owners.OrderBy(o => o.OwnerId).FirstOrDefault();
    if (owner is null)
    {
        Console.WriteLine("No owner found to update.");
        return;
    }

    owner.FullName = owner.FullName.EndsWith(" (Updated)")
        ? owner.FullName.Replace(" (Updated)", string.Empty)
        : $"{owner.FullName} (Updated)";

    db.SaveChanges();
    Console.WriteLine($"Update complete: Owner {owner.OwnerId} is now '{owner.FullName}'.");
}

void RunDelete()
{
    // Delete all matching demo pets so the environment resets in one command.
    var demoPets = db.Pets
        .Include(p => p.Owner)
        .Include(p => p.Tricks)
        .Where(p => p.Name == demoPetName && p.Species == demoPetSpecies)
        .ToList();

    if (!demoPets.Any())
    {
        Console.WriteLine("No demo pet found to delete.");
        return;
    }

    foreach (var petToDelete in demoPets)
    {
        db.Pets.Remove(petToDelete);
    }

    // Save first so cascade cleanup for join rows is applied.
    db.SaveChanges();

    var demoTrick = db.Tricks
        .Include(t => t.Pets)
        .FirstOrDefault(t => t.Name == demoTrickName && t.DifficultyLevel == demoTrickDifficulty);

    if (demoTrick is not null && !demoTrick.Pets.Any())
    {
        db.Tricks.Remove(demoTrick);
    }

    var demoOwners = db.Owners
        .Include(o => o.Pets)
        .Where(o => o.FullName == demoOwnerName)
        .ToList();

    foreach (var demoOwner in demoOwners)
    {
        // Remove only owners that are no longer referenced by pets.
        if (!demoOwner.Pets.Any())
        {
            db.Owners.Remove(demoOwner);
        }
    }

    db.SaveChanges();
    Console.WriteLine("Delete complete: demo records removed safely.");
}

void RunTrackingDemo()
{
    // Tracking query: EF detects the change automatically; SaveChanges persists it.
    db.ChangeTracker.Clear();

    var pet = db.Pets.OrderBy(p => p.PetId).FirstOrDefault();
    if (pet is null)
    {
        Console.WriteLine("No pet found for tracking demo.");
        return;
    }

    var original = pet.Species;
    pet.Species = "Demo";        // Short fixed value avoids MaxLength(30) issues.

    var rows = db.SaveChanges();
    Console.WriteLine($"Tracking update saved. Rows changed: {rows}.  Species changed from '{original}' to '{pet.Species}'.");

    // Revert so the demo is repeatable and seed data stays clean.
    pet.Species = original;
    db.SaveChanges();
}

void RunNoTrackingFailDemo()
{
    // No-tracking query: EF does not track this entity, so SaveChanges will not persist it.
    db.ChangeTracker.Clear();

    var pet = db.Pets
        .AsNoTracking()
        .OrderBy(p => p.PetId)
        .FirstOrDefault();

    if (pet is null)
    {
        Console.WriteLine("No pet found for no-tracking demo.");
        return;
    }

    var originalSpecies = pet.Species;
    pet.Species = "NoTrackFail";

    var rows = db.SaveChanges();

    var dbValue = db.Pets
        .AsNoTracking()
        .Where(p => p.PetId == pet.PetId)
        .Select(p => p.Species)
        .FirstOrDefault();

    Console.WriteLine($"No-tracking update attempted. Rows changed: {rows}. Database value is still '{dbValue}'.");

    // Restore in-memory value so this variable does not mislead later debugging.
    pet.Species = originalSpecies;
}

void RunNoTrackingFixDemo()
{
    // Fix no-tracking update by explicitly marking the detached entity as Modified.
    db.ChangeTracker.Clear();

    var pet = db.Pets
        .AsNoTracking()
        .OrderBy(p => p.PetId)
        .FirstOrDefault();

    if (pet is null)
    {
        Console.WriteLine("No pet found for no-tracking fix demo.");
        return;
    }

    var original = pet.Species;
    pet.Species = "Fixed";       // Short fixed value avoids MaxLength(30) issues.
    db.Entry(pet).State = EntityState.Modified;

    var rows = db.SaveChanges();
    Console.WriteLine($"No-tracking fix applied. Rows changed: {rows}.  Species changed from '{original}' to '{pet.Species}'.");

    // Revert so the demo is repeatable and seed data stays clean.
    pet.Species = original;
    db.Entry(pet).State = EntityState.Modified;
    db.SaveChanges();
}

void RunReports()
{
    Console.WriteLine("--- Report 1: Owners -> Pets -> Treats (Include/ThenInclude) ---");
    var ownerReport = db.Owners
        .AsNoTracking()
        .Include(o => o.Pets)
            .ThenInclude(p => p.PetTreats)
                .ThenInclude(pt => pt.Treat)
        .OrderBy(o => o.FullName)
        .ToList();

    foreach (var owner in ownerReport)
    {
        var totalTreatEvents = owner.Pets.Sum(p => p.PetTreats.Count);
        Console.WriteLine($"{owner.FullName}: {owner.Pets.Count} pets, {totalTreatEvents} treat events");
    }

    Console.WriteLine("--- Report 2: Pet count by species (GroupBy/OrderBy) ---");
    var speciesReport = db.Pets
        .AsNoTracking()
        .GroupBy(p => p.Species)
        .Select(g => new { Species = g.Key, Count = g.Count() })
        .OrderByDescending(x => x.Count)
        .ThenBy(x => x.Species)
        .ToList();

    foreach (var row in speciesReport)
    {
        Console.WriteLine($"{row.Species}: {row.Count}");
    }

    Console.WriteLine("--- Report 3: Trick count by difficulty (GroupBy/OrderBy) ---");
    var trickDifficultyReport = db.Tricks
        .AsNoTracking()
        .GroupBy(t => t.DifficultyLevel)
        .Select(g => new { Difficulty = g.Key, Count = g.Count() })
        .OrderBy(x => x.Difficulty)
        .ToList();

    foreach (var row in trickDifficultyReport)
    {
        Console.WriteLine($"{row.Difficulty}: {row.Count}");
    }

    Console.WriteLine("--- Report 4: Treat usage totals (GroupBy/OrderBy) ---");
    var treatUsageReport = db.PetTreats
        .AsNoTracking()
        .Include(pt => pt.Treat)
        .GroupBy(pt => pt.Treat.Name)
        .Select(g => new
        {
            TreatName = g.Key,
            TotalQuantity = g.Sum(x => x.Quantity),
            TimesGiven = g.Count()
        })
        .OrderByDescending(x => x.TotalQuantity)
        .ThenBy(x => x.TreatName)
        .ToList();

    foreach (var row in treatUsageReport)
    {
        Console.WriteLine($"{row.TreatName}: quantity={row.TotalQuantity}, events={row.TimesGiven}");
    }

    Console.WriteLine("--- Report 5: Top pets by tricks known (Include/OrderBy) ---");
    var topPetsByTricks = db.Pets
        .AsNoTracking()
        .Include(p => p.Tricks)
        .OrderByDescending(p => p.Tricks.Count)
        .ThenBy(p => p.Name)
        .Take(5)
        .ToList();

    foreach (var pet in topPetsByTricks)
    {
        Console.WriteLine($"{pet.Name}: {pet.Tricks.Count} tricks");
    }
}

void RestoreSeedData()
{
    // Dictionary of seeded PetId -> expected Species values.
    var seedSpecies = new Dictionary<int, string>
    {
        { 1,  "Dog"    },
        { 2,  "Cat"    },
        { 10, "Parrot" },
        { 11, "Dog"    },
        { 12, "Rabbit" }
    };

    var changed = false;
    foreach (var (id, expected) in seedSpecies)
    {
        var pet = db.Pets.Find(id);
        if (pet is not null && pet.Species != expected)
        {
            pet.Species = expected;
            changed = true;
        }
    }

    if (changed)
        db.SaveChanges();
}
