namespace PetParkModel;

/// <summary>
/// Represents a pet that can learn one or more tricks.
/// </summary>
public class Pet
{
    /// <summary>
    /// Gets or sets the primary key for the pet.
    /// </summary>
    public int PetId { get; set; }

    /// <summary>
    /// Gets or sets the pet's display name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the species classification for the pet.
    /// </summary>
    public string Species { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the tricks associated with this pet.
    /// </summary>
    public ICollection<Trick> Tricks { get; set; } = new List<Trick>();
}
