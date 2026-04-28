using System.ComponentModel.DataAnnotations;

namespace PetParkModel;

/// <summary>
/// Represents a treat that can be given to many pets.
/// </summary>
public class Treat
{
    /// <summary>
    /// Gets or sets the primary key for the treat.
    /// </summary>
    public int TreatId { get; set; }

    /// <summary>
    /// Gets or sets the treat name.
    /// </summary>
    [Required]
    [MaxLength(60)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the calorie amount for the treat.
    /// </summary>
    [Range(1, 500)]
    public int Calories { get; set; }

    /// <summary>
    /// Gets or sets pet treat links for this treat.
    /// </summary>
    public ICollection<PetTreat> PetTreats { get; set; } = new List<PetTreat>();
}
