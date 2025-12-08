Wizard wz1 = new Wizard("Rene");

Warrior w1 = new Warrior("Gabriel");
Gambler g1 = new Gambler("Gamble");
ElementalMage E1 = new ElementalMage("Elemental");
Necromancer N1 = new Necromancer("Necro");

ClockworkSentinel c1 = new ClockworkSentinel();

Ability AbilitySelection(Character player)
{
    int choice;
    bool validEntry = false;
    do
    {
        int counter = 0;
        Console.WriteLine("Choose your Ability:");
        foreach (Ability i in player.Abilities)
        {
            counter++;
            Console.WriteLine($"{counter} {i.Name}, Damage:{i.Damage}, Accuracy: {i.Accuracy}%");
        }
        string playerchoice = Console.ReadLine();
        if (int.TryParse(playerchoice, out choice))
        {
            if (choice > 0 && choice <= player.Abilities.Length)
            {
                return player.Abilities[choice - 1];
            }
            else
            {
                Console.WriteLine("Invalid entry. Try again!");
                Thread.Sleep(2000);
                Console.Clear();
            }
        }
        else
        {
            Console.WriteLine("Invalid entry. Try again!");
            Thread.Sleep(2000);
            Console.Clear();
        }
    }
    while (!validEntry);
    return null;
}

void Fighting(Character player, Character enemy)
{
    while (player.Health > 0 && enemy.Health > 0)
    {
        Ability selectedAbility = AbilitySelection(player);
        Thread.Sleep(2000);
        player.Attack(selectedAbility, enemy);
        if (enemy.Health <= 0)
        {
            Console.WriteLine($"{enemy.Name} got defeated. You won!");
            player.GainXP(20);
            break;
        }
        Thread.Sleep(2000);
        enemy.Attack(enemy.Abilities[RNG.random.Next(0,enemy.Abilities.Length)], player);
        if (player.Health <= 0)
        {
            Console.WriteLine($"{enemy.Name} defeated you. Better luck next time.");
            break;
        }
    }
}

void Healing(Character player)
{
    player.Health = player.MaxHealth;
    Console.WriteLine($"You found a fountain and drink from it. You feel strengthened.. HP set to {player.Health}\n");
}

Fighting(E1, c1);
Healing(wz1);
