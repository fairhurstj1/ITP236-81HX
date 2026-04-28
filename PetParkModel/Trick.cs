namespace PetParkModel;

public class Trick
{
    public int TrickId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string DifficultyLevel { get; set; } = string.Empty;

    public ICollection<Pet> Pets { get; set; } = new List<Pet>();
}
