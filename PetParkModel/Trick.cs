namespace PetParkModel;

/// <summary>
/// Represents a trick that can be associated with one or more pets.
/// </summary>
public class Trick
{
    /// <summary>
    /// Gets or sets the primary key for the trick.
    /// </summary>
    public int TrickId { get; set; }

    /// <summary>
    /// Gets or sets the trick name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the difficulty level for the trick.
    /// </summary>
    public string DifficultyLevel { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the pets that know this trick.
    /// </summary>
    public ICollection<Pet> Pets { get; set; } = new List<Pet>();
}
