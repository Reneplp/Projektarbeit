public class Character
{
    public string Name {get ; set; }
    public string Role {get ; set; }
    public int Health {get ; set; }
    public int Armor {get ; set; }
    public int Damage {get ; set; }
    public int Level {get ; set; }
    public int CritChance {get ; set; }
    public bool IsStatusEffect {get ; set; }

    public Ability[] Abilities {get ; set; }
    public Character(string name, string role, int health, int armor, int damage, int critChance)
    {
        Name = name;
        Role = role;
        Health = health;
        Armor = armor;
        Damage = damage;
        Level = 1;
        CritChance = critChance;
        IsStatusEffect = false;
    }
}

public class Warrior : Character
{
    public Warrior(string name) : base(name, "Warrior", 100, 3, 1, 5) {}
}
public class Wizard : Character
{
    public Wizard (string name) : base(name, "Wizard", 100, 1, 3, 15)
    {
        Ability A1 = new Ability("Arcane Eruption", 80, "", 50);
        Ability A2 = new Ability("Arcane Missiles", 10, "", 95, true);
        Ability[] wizardAbilities = {A1, A2};

        Abilities = wizardAbilities;
    }
}
public class Rogue : Character
{
    public Rogue (string name) : base(name, "Rogue", 100, 2, 2, 30) {}
}