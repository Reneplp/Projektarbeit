public class SaveGame
{
    public string Name { get; set; }
    public string Role { get; set; }

    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public int Armor { get; set; }
    public int Damage { get; set; }
    public int CritChance { get; set; }
    public int Level { get; set; }
    public int XP { get; set; }

    public bool HasSubclass { get; set; }

    public float RunDifficulty { get; set; }
    public int RunNumber { get; set; }
}