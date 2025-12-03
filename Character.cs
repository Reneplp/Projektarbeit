public class Character
{
    public string Name { get; set; }
    public string Role { get; set; } // TODO
    public int Health { get; set; }
    public int Armor { get; set; }
    public int Damage { get; set; }
    public int Level { get; set; } // TODO
    public int CritChance { get; set; }
    public bool IsStatusEffect { get; set; } // TODO

    public Ability[] Abilities { get; set; }
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
    public void Attack(Ability ability, Character target)
    {
        bool criticalHit = false;
        int armor = target.Armor;
        int targetHP = target.Health;
        int damage = ability.Damage;
        int accuracy = ability.Accuracy;

        Random random = new Random();

        if (random.Next(100) <= accuracy)
        {
            int baseDamage = (int)(damage * (1 + Damage * 0.5f));

            if (random.Next(100) <= CritChance)
            {
                baseDamage *= 2;
                criticalHit = true;
            }

            int finalDamage = (int)(baseDamage / (1 + armor * 0.5f));
            target.Health = targetHP - finalDamage;

            if (criticalHit == false)
            {
                Console.WriteLine($"{Name} attacked {target.Name} and dealt {finalDamage} damage. {target.Name} HP reduced to {target.Health}.\n");
            }
            else
            {
                Console.WriteLine($"{Name} attacked {target.Name} for a critical hit and dealt {finalDamage} damage. {target.Name} HP reduced to {target.Health}.\n");
            }
        }
        else
        {
            Console.WriteLine("The attack missed!");
        }
    }
}


public class Warrior : Character
{
    public Warrior(string name) : base(name, "Warrior", 100, 3, 1, 5) { }
}
public class Wizard : Character
{
    public Wizard(string name) : base(name, "Wizard", 100, 1, 3, 15)
    {
        Ability A1 = new Ability("Arcane Eruption", 80, "", 50);
        Ability A2 = new Ability("Arcane Missiles", 10, "", 95, true);
        Ability[] wizardAbilities = { A1, A2 };

        Abilities = wizardAbilities;
    }
}
public class Rogue : Character
{
    public Rogue(string name) : base(name, "Rogue", 100, 2, 2, 30) { }
}