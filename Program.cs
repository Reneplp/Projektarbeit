Wizard wz1 = new Wizard("TestWizard");

Wizard wz2 = new Wizard("EnemyWizard");

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
                validEntry = true;

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
    Random random = new Random();
    while (player.Health > 0 && enemy.Health > 0)
    {
        Ability selectedAbility = AbilitySelection(player);
        Thread.Sleep(2000);
        player.Attack(selectedAbility, enemy);
        if (enemy.Health <= 0)
        {
            Console.WriteLine($"{enemy.Name} got defeated. You won!");
            break;
        }
        Thread.Sleep(2000);
        enemy.Attack(enemy.Abilities[random.Next(0,enemy.Abilities.Length)], player);
        if (player.Health <= 0)
        {
            Console.WriteLine($"{enemy.Name} defeated you. Better luck next time.");
            break;
        }
    }
}
Fighting(wz1, wz2);

