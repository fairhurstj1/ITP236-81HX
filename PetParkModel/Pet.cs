using System.ComponentModel.DataAnnotations;

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
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the species classification for the pet.
    /// </summary>
    [Required]
    [MaxLength(30)]
    public string Species { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the owner foreign key.
    /// </summary>
    public int OwnerId { get; set; }

    /// <summary>
    /// Gets or sets the owner of this pet.
    /// </summary>
    public Owner Owner { get; set; } = null!;

    /// <summary>
    /// Gets or sets the tricks associated with this pet.
    /// </summary>
    public ICollection<Trick> Tricks { get; set; } = new List<Trick>();

    /// <summary>
    /// Gets or sets treat links associated with this pet.
    /// </summary>
    public ICollection<PetTreat> PetTreats { get; set; } = new List<PetTreat>();
}
