public class Ability
{
    public string Name {get ; set; }
    public int Damage {get ; set; }
    public int Accuracy {get ; set; }
    public bool Multihit {get ; set; }
    public string? StatusEffect {get ; set; }
    public int ArmorModifier {get ; set; }
    public int DamageModifier {get ; set; }
    public int ModifierDuration {get ; set; }
    public bool SelfCast {get ; set; }
    
public Ability(string name, int damage, int accuracy)
    {   
        Name = name;
        Damage = damage;
        Accuracy = accuracy;
        SelfCast = false;
    }
}