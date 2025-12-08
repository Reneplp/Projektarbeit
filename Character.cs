
public class Character
{
    public string Name { get; set; }
    public string Role { get; set; } // TODO
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public int Armor { get; set; }
    public int Damage { get; set; }
    public int Level { get; set; }
    public int CritChance { get; set; }
    public string StatusEffect { get; set; }
    public int FreezeCounter { get; set; }
    public int PoisonCounter { get; set; }
    public int TempArmor { get; set; }
    public int TempDamage { get; set; }
    public int ArmorDuration { get; set; }
    public int DamageDuration { get; set; }
    public int XP { get; set; }
    public int XPToNextLevel => Level * 25;

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
        XP = 0;
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
                if (ability.Damage == 0)
                {
                    ApplyStatusEffect(ability, target);
                    ReduceBuffDuration();
                    return;
                }
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
                        Console.WriteLine($"{Name} attacked {target.Name} with {ability.Name} and dealt {finalDamage} damage. {target.Name} HP reduced to {target.Health}.\n");
                    }
                    else
                    {
                        Console.WriteLine($"{Name} attacked {target.Name} with {ability.Name} for a critical hit and dealt {finalDamage} damage. {target.Name} HP reduced to {target.Health}.\n");
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
                            Console.WriteLine($"{Name} attacked {target.Name} with {ability.Name} and dealt {finalDamage} damage. {target.Name} HP reduced to {target.Health}.\n");
                        }
                        else
                        {
                            Console.WriteLine($"{Name} attacked {target.Name} with {ability.Name} for a critical hit and dealt {finalDamage} damage. {target.Name} HP reduced to {target.Health}.\n");
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
    public void ReduceBuffDuration()
    {
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
    }
    public void ApplyStatusEffect(Ability ability, Character target)
    {
        Character affectedTarget;

        if (ability.SelfCast == true)
        {
            affectedTarget = this;
        }
        else
        {
            affectedTarget = target;
        }
        string applyStatusEffect = ability.StatusEffect?.ToLower();
        if (applyStatusEffect != "")
        {
            switch (applyStatusEffect)
            {
                case "burn":
                    int burnChance = RNG.random.Next(100);
                    if (burnChance <= 55)
                    {
                        Console.WriteLine($"{affectedTarget.Name} is burning!");
                        affectedTarget.StatusEffect = applyStatusEffect;
                    }
                    break;
                case "freeze":
                    int freezeChance = RNG.random.Next(100);
                    if (freezeChance <= 35 && affectedTarget.StatusEffect != "freeze")
                    {
                        affectedTarget.FreezeCounter = 0;
                        Console.WriteLine($"{affectedTarget.Name} is frozen!");
                        affectedTarget.StatusEffect = applyStatusEffect;
                    }
                    break;
                case "paralyze":
                    int paralyzeChance = RNG.random.Next(100);
                    if (paralyzeChance <= 45)
                    {
                        Console.WriteLine($"{affectedTarget.Name} is paralyzed!");
                        affectedTarget.StatusEffect = applyStatusEffect;
                    }
                    break;
                case "bleed":
                    int bleedChance = RNG.random.Next(100);
                    if (bleedChance <= 75)
                    {
                        Console.WriteLine($"{affectedTarget.Name} is bleeding!");
                        affectedTarget.StatusEffect = applyStatusEffect;
                    }
                    break;
                case "poison":
                    int poisonChance = RNG.random.Next(100);
                    if (poisonChance <= 75 && affectedTarget.StatusEffect != "poison")
                    {
                        affectedTarget.PoisonCounter = 0;
                        Console.WriteLine($"{affectedTarget.Name} is poisoned!");
                        affectedTarget.StatusEffect = applyStatusEffect;
                    }
                    break;
            }
        }
        if (ability.DamageModifier != 0)
        {
            affectedTarget.TempDamage += ability.DamageModifier;
            affectedTarget.DamageDuration = ability.ModifierDuration;
            Console.WriteLine($"{affectedTarget.Name}'s damage changed by {ability.DamageModifier} for {ability.ModifierDuration} turns!");
        }
        if (ability.ArmorModifier != 0)
        {
            affectedTarget.TempArmor += ability.ArmorModifier;
            affectedTarget.ArmorDuration = ability.ModifierDuration;
            Console.WriteLine($"{affectedTarget.Name}'s armor changed by {ability.ArmorModifier} for {ability.ModifierDuration} turns!");
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
        return false;
    }

    public void GainXP(int amount)
    {
        XP += amount;
        Console.WriteLine($"{Name} gained {amount} XP.");

        while (XP >= XPToNextLevel)
        {
            XP -= XPToNextLevel;
            LevelUp();
        }
    }
    public void LevelUp()
    {
        Level++;
        MaxHealth += 10;
        Health = MaxHealth;
        Damage += 1;
        Armor += 1;

        Console.WriteLine($"--- LEVEL UP! {Name} reached Level {Level}! ---");
        Console.WriteLine("Stats increased:");
        Console.WriteLine($"Max Health: {MaxHealth}");
        Console.WriteLine($"Damage: {Damage}");
        Console.WriteLine($"Armor: {Armor}");
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
            ModifierDuration = 3,
            SelfCast = true
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
        Abilities = [A1, A2, A3];
    }
}
public class Rogue : Character
{
    public Rogue(string name) : base(name, "Rogue", 100, 2, 2, 30) { }
}

public class FireElemental : Character
{
    public FireElemental() : base("Fire Elemental", "Monster", 65, 0, 4, 5)
    {
        Ability A1 = new Ability("Flame Burst", 7, 90);
        Ability A2 = new Ability("Ignite", 3, 85)
        {
            StatusEffect = "burn"
        };
        Ability A3 = new Ability("Flame Surge", 0, 100)
        {
            DamageModifier = 2,
            ModifierDuration = 2,
            SelfCast = true
        };
        Abilities = [A1, A2, A3];
    }
}

public class WaterElemental : Character
{
    public WaterElemental() : base("Water Elemental", "Monster", 85, 1, 3, 10)
    {
        Ability A1 = new Ability("Water Jet", 6, 95);
        Ability A2 = new Ability("Soaked", 0, 90)
        {
            ArmorModifier = -1,
            ModifierDuration = 3
        };
        Ability A3 = new Ability("Ice Shard", 4, 80)
        {
            StatusEffect = "freeze"
        };
        Abilities = [A1, A2, A3];
    }
}

public class EarthElemental : Character
{
    public EarthElemental() : base("Earth Elemental", "Monster", 120, 3, 2, 2)
    {
        Ability A1 = new Ability("Rock Slam", 4, 90);
        Ability A2 = new Ability("Stone Skin", 0, 100)
        {
            ArmorModifier = 2,
            ModifierDuration = 3,
            SelfCast = true
        };
        Ability A3 = new Ability("Quake", 6, 70);
        Abilities = [A1, A2, A3];
    }
}

public class AirElemental : Character
{
    public AirElemental() : base("Air Elemental", "Monster", 60, 0, 3, 30)
    {
        Ability A1 = new Ability("Gust", 5, 95);
        Ability A2 = new Ability("Slicing Wind", 4, 85)
        {
            Multihit = true
        };
        Ability A3 = new Ability("Thicken", 0, 100)
        {
            ArmorModifier = 2,
            ModifierDuration = 3,
            SelfCast = true
        };
        Abilities = [A1, A2, A3];
    }
}

public class ShadowWraith : Character
{
    public ShadowWraith() : base("Shadow Wraith", "Monster", 70, 1, 2, 15)
    {
        Ability A1 = new Ability("Dark Touch", 5, 90);
        Ability A2 = new Ability("Curse", 0, 100)
        {
            DamageModifier = -2,
            ModifierDuration = 3
        };
        Ability A3 = new Ability("Slash", 4, 85)
        {
            StatusEffect = "bleed"
        };
        Abilities = [A1, A2, A3];
    }
}

public class Behir : Character
{
    public Behir() : base("Behir", "Monster", 80, 1, 2, 10)
    {
        Ability A1 = new Ability("Bolt Strike", 5, 90);
        Ability A2 = new Ability("Thunder Fang", 2, 85)
        {
            StatusEffect = "paralyze"
        };
        Ability A3 = new Ability("Harden Scale", 0, 100)
        {
            ArmorModifier = 2,
            ModifierDuration = 3,
            SelfCast = true
        };
        Abilities = [A1, A2, A3];
    }
}
public class ClockworkSentinel : Character
{
    public ClockworkSentinel() : base("Clockwork Sentinel", "Monster", 110, 2, 3, 5)
    {
        Ability A1 = new Ability("Metal Fist", 6, 90);
        Ability A2 = new Ability("Reinforce", 0, 100)
        {
            ArmorModifier = 2,
            ModifierDuration = 2,
            SelfCast = true
        };
        Ability A3 = new Ability("Overclock", 0, 100)
        {
            DamageModifier = 2,
            ModifierDuration = 2,
            SelfCast = true
        };
        Abilities = [A1, A2, A3];
    }
}
public class WebDeveloper : Character
{
    public WebDeveloper() : base("Web Developer", "Monster", 75, 0, 3, 20)
    {
        Ability A1 = new Ability("Venomous Bite", 4, 90)
        {
            StatusEffect = "poison"
        };
        Ability A2 = new Ability("Rapid Strikes", 3, 90)
        {
            Multihit = true
        };
        Ability A3 = new Ability("Web Trap", 0, 80)
        {
            DamageModifier = -1,
            ModifierDuration = 3
        };
        Abilities = [A1, A2, A3];
    }
}
public class ArcaneTitan : Character
{
    public ArcaneTitan() : base("Arcane Titan", "Monster", 180, 3, 5, 10)
    {
        Ability A1 = new Ability("Titan Smash", 10, 90);
        Ability A2 = new Ability("Arcane Shockwave", 6, 85)
        {
            DamageModifier = -2,
            ModifierDuration = 2
        };
        Ability A3 = new Ability("Frozen Tomb", 4, 75)
        {
            StatusEffect = "freeze"
        };
        Ability A4 = new Ability("Rune Barrier", 0, 100)
        {
            ArmorModifier = 3,
            ModifierDuration = 3,
            SelfCast = true
        };
        Abilities = [A1, A2, A3, A4];
    }
}
public class Aeternyx : Character
{
    public Aeternyx() : base("Aeternyx, the primordial Hydra", "Monster", 260, 4, 6, 20)
    {
        Ability A1 = new Ability("Hydra Strike", 12, 90);
        Ability A2 = new Ability("Searing Bite", 6, 85)
        {
            DamageModifier = 2,
            ModifierDuration = 2,
            SelfCast = true
        };
        Ability A3 = new Ability("Chilling Roar", 0, 100)
        {
            ArmorModifier = -2,
            ModifierDuration = 2
        };
        Ability A4 = new Ability("Toxic Breath", 5, 85)
        {
            StatusEffect = "poison"
        };
        Ability A5 = new Ability("Scale Regrowth", 0, 100)
        {
            ArmorModifier = 4,
            ModifierDuration = 3,
            SelfCast = true
        };
        Abilities = [A1, A2, A3, A4, A5];
    }
}
public static class RNG
{
    public static Random random = new Random();
}