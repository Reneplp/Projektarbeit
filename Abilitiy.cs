public class Ability
{
    public string Name {get ; set; }
    public int Damage {get ; set; }
    public string? StatusEffect {get ; set; }
    public int Accuracy {get ; set; }
    public bool Multihit {get ; set; }
    
public Ability(string name, int damage, string statusEffect, int accuracy, bool multihit = false)
    {   
        Name = name;
        Damage = damage;
        StatusEffect = statusEffect;
        Accuracy = accuracy;
        Multihit = multihit;
    }
}