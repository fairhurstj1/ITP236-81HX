using System.ComponentModel.DataAnnotations;

namespace PetParkModel;

/// <summary>
/// Bridge entity linking pets and treats with additional details.
/// </summary>
public class PetTreat
{
    /// <summary>
    /// Gets or sets the pet foreign key.
    /// </summary>
    public int PetId { get; set; }

    /// <summary>
    /// Gets or sets the pet navigation.
    /// </summary>
    public Pet Pet { get; set; } = null!;

    /// <summary>
    /// Gets or sets the treat foreign key.
    /// </summary>
    public int TreatId { get; set; }

    /// <summary>
    /// Gets or sets the treat navigation.
    /// </summary>
    public Treat Treat { get; set; } = null!;

    /// <summary>
    /// Gets or sets when the treat was given.
    /// </summary>
    public DateTime DateGiven { get; set; }

    /// <summary>
    /// Gets or sets quantity of treats given.
    /// </summary>
    [Range(1, 10)]
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets optional notes for the treat event.
    /// </summary>
    [MaxLength(200)]
    public string? Notes { get; set; }
}
