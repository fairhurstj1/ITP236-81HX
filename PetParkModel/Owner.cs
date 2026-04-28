using System.ComponentModel.DataAnnotations;

namespace PetParkModel;

/// <summary>
/// Represents a pet owner who can own many pets.
/// </summary>
public class Owner
{
    /// <summary>
    /// Gets or sets the primary key for the owner.
    /// </summary>
    public int OwnerId { get; set; }

    /// <summary>
    /// Gets or sets the owner full name.
    /// </summary>
    [Required]
    [MaxLength(80)]
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the pets owned by this owner.
    /// </summary>
    public ICollection<Pet> Pets { get; set; } = new List<Pet>();
}
