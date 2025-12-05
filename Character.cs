
public class Character
{
    public string Name { get; set; }
    public string Role { get; set; } // TODO
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public int Armor { get; set; }
    public int Damage { get; set; }
    public int Level { get; set; } // TODO
    public int CritChance { get; set; }
    public string StatusEffect { get; set; }
    public int FreezeCounter { get; set; }
    public int PoisonCounter { get; set; }
    public int TempArmor { get; set; }
    public int TempDamage { get; set; }
    public int ArmorDuration { get; set; }
    public int DamageDuration { get; set; }

    public Ability[] Abilities { get; set; }
    public Character(string name, string role, int health, int armor, int damage, int critChance)
    {
        Name = name;
        Role = role;
        Health = health;
        MaxHealth = health;
        Armor = armor;
        Damage = damage;
        Level = 1;
        CritChance = critChance;
        StatusEffect = "";
        FreezeCounter = 0;
        PoisonCounter = 0;
        TempArmor = 0;
        TempDamage = 0;
        ArmorDuration = 0;
        DamageDuration = 0;
    }
    public void Attack(Ability ability, Character target)
    {
        bool criticalHit = false;
        int armor = target.Armor + target.TempArmor;
        int attackerDamage = Damage + TempDamage;
        int damage = ability.Damage;
        int accuracy = ability.Accuracy;

        if (executeStatusEffect() == false)
        {

            if (RNG.random.Next(100) <= accuracy)
            {
                int baseDamage = (int)(damage * (1 + attackerDamage * 0.5f));

                if (!ability.Multihit)
                {
                    if (RNG.random.Next(100) <= CritChance)
                    {
                        baseDamage *= 2;
                        criticalHit = true;
                    }
                    int finalDamage = (int)(baseDamage / (1 + armor * 0.5f));
                    target.Health -= finalDamage;

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
                    int hitNumber = RNG.random.Next(2, 6);
                    int hitCounter = 1;
                    do
                    {
                        int currentDamage = baseDamage;
                        if (RNG.random.Next(100) <= CritChance)
                        {
                            currentDamage *= 2;
                            criticalHit = true;
                        }

                        int finalDamage = (int)(currentDamage / (1 + armor * 0.5f));
                        target.Health -= finalDamage;

                        if (criticalHit == false)
                        {
                            Console.WriteLine($"{Name} attacked {target.Name} and dealt {finalDamage} damage. {target.Name} HP reduced to {target.Health}.\n");
                        }
                        else
                        {
                            Console.WriteLine($"{Name} attacked {target.Name} for a critical hit and dealt {finalDamage} damage. {target.Name} HP reduced to {target.Health}.\n");
                        }
                        hitCounter++;
                        criticalHit = false;
                    }
                    while (hitCounter <= hitNumber);
                }
                ApplyStatusEffect(ability, target);

            }
            else
            {
                Console.WriteLine("The attack missed!");
            }
        }
    }
    public void ApplyStatusEffect(Ability ability, Character target)
    {
        string applyStatusEffect = ability.StatusEffect.ToLower();
        if (applyStatusEffect != "")
        {
            switch (applyStatusEffect)
            {
                case "burn":
                    int burnChance = RNG.random.Next(100);
                    if (burnChance <= 55)
                    {
                        Console.WriteLine($"{target.Name} is burning!");
                        target.StatusEffect = applyStatusEffect;
                    }
                    break;
                case "freeze":
                    int freezeChance = RNG.random.Next(100);
                    if (freezeChance <= 35 && target.StatusEffect != "freeze")
                    {
                        target.FreezeCounter = 0;
                        Console.WriteLine($"{target.Name} is frozen!");
                        target.StatusEffect = applyStatusEffect;
                    }
                    break;
                case "paralyze":
                    int paralyzeChance = RNG.random.Next(100);
                    if (paralyzeChance <= 45)
                    {
                        Console.WriteLine($"{target.Name} is paralyzed!");
                        target.StatusEffect = applyStatusEffect;
                    }
                    break;
                case "bleed":
                    int bleedChance = RNG.random.Next(100);
                    if (bleedChance <= 75)
                    {
                        Console.WriteLine($"{target.Name} is bleeding!");
                        target.StatusEffect = applyStatusEffect;
                    }
                    break;
                case "poison":
                    int poisonChance = RNG.random.Next(100);
                    if (poisonChance <= 75 && target.StatusEffect != "poison")
                    {
                        target.PoisonCounter = 0;
                        Console.WriteLine($"{target.Name} is poisoned!");
                        target.StatusEffect = applyStatusEffect;
                    }
                    break;
                default:
                    Console.WriteLine("Something went wrong (Statuseffects)");
                    break;
            }
        }
        if (ability.DamageModifier != 0)
        {
            target.TempDamage += ability.DamageModifier;
            target.DamageDuration = ability.ModifierDuration;
            Console.WriteLine($"{target.Name}'s damage changed by {ability.DamageModifier} for {ability.ModifierDuration} turns!");
        }
        if (ability.ArmorModifier != 0)
        {
            target.TempArmor += ability.ArmorModifier;
            target.ArmorDuration = ability.ModifierDuration;
            Console.WriteLine($"{target.Name}'s armor changed by {ability.DamageModifier} for {ability.ModifierDuration} turns!");
        }
    }
    public bool executeStatusEffect()
    {
        switch (StatusEffect)
        {
            case "burn":
                Health -= 5;
                Console.WriteLine($"{Name} lost 5 Hp due to burn. New HP: {Health}");
                break;
            case "freeze":
                FreezeCounter++;
                if (FreezeCounter <= 3)
                {
                    Console.WriteLine($"{Name} is frozen! Freeze Counter {FreezeCounter}");
                    return true;
                }
                else
                {
                    StatusEffect = "";
                    FreezeCounter = 0;
                }
                break;
            case "paralyze":
                int movingChance = RNG.random.Next(100);
                if (movingChance <= 50)
                {
                    Console.WriteLine($"{Name} is paralyzed and can't attack.");
                    return true;
                }
                break;
            case "bleed":
                Health -= 3;
                Console.WriteLine($"{Name} lost 3 Hp due to bleeding. New HP: {Health}");
                break;
            case "poison":
                PoisonCounter++;
                int PoisonDamage = (int)(PoisonCounter * 1.5f);
                Health -= PoisonDamage;
                Console.WriteLine($"{Name} lost {PoisonDamage} Hp due to poison.. New HP: {Health}");
                break;
        }
        if (DamageDuration > 0)
        {
            DamageDuration--;
            if (DamageDuration == 0)
            {
                TempDamage = 0;
                Console.WriteLine($"{Name}'s damage buff wore off.");
            }
        }
        if (ArmorDuration > 0)
        {
            ArmorDuration--;
            if (ArmorDuration == 0)
            {
                TempArmor = 0;
                Console.WriteLine($"{Name}'s armor buff wore off.");
            }
        }
        return false;
    }
}


public class Warrior : Character
{
    public Warrior(string name) : base(name, "Warrior", 100, 3, 1, 5)
    {
        Ability A1 = new Ability("Dummy Attack", 5, 100);
        Ability A2 = new Ability("War Cry", 0, 100)
        {
            DamageModifier = 2,
            ModifierDuration = 3
        };
        Ability[] warriorAbilities = { A1, A2 };

        Abilities = warriorAbilities;
    }
}
public class Wizard : Character
{
    public Wizard(string name) : base(name, "Wizard", 100, 1, 3, 15)
    {
        Ability A1 = new Ability("Arcane Eruption", 25, 50);
        Ability A2 = new Ability("Arcane Missiles", 5, 95)
        {
            Multihit = true
        };
        Ability A3 = new Ability("Volcanic Shot", 5, 95)
        {
            StatusEffect = "burn"
        };
        Ability[] wizardAbilities = { A1, A2, A3 };

        Abilities = wizardAbilities;
    }
}
public class Rogue : Character
{
    public Rogue(string name) : base(name, "Rogue", 100, 2, 2, 30) { }
}

public static class RNG
{
    public static Random random = new Random();
}