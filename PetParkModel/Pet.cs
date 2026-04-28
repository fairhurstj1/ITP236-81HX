namespace PetParkModel;

public class Pet
{
    public int PetId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Species { get; set; } = string.Empty;

    public ICollection<Trick> Tricks { get; set; } = new List<Trick>();
}
