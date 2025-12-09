int runNumber = 1;
bool gameRunning = true;
float runDifficulty = 1.0f;
float fightDifficulty = 1.0f;
Character player = null;
bool endbossDefeated = false;

void ApplyDifficulty(Character enemy)
{
    float totalMultiplier = runDifficulty * fightDifficulty;

    enemy.MaxHealth = (int)(enemy.MaxHealth * (1 + (totalMultiplier - 1) * 1.1f));
    enemy.Health = enemy.MaxHealth; // immer voll starten

    enemy.Damage = (int)(enemy.Damage * (1 + (totalMultiplier - 1) * 1.25f));
    enemy.Armor = (int)(enemy.Armor * (1 + (totalMultiplier - 1) * 0.75f));

    enemy.CritChance = Math.Min(
        enemy.CritChance + (int)(totalMultiplier * 5),
        75
    );
}


Character CreateStartingCharacter()
{
    Console.WriteLine("What's your characters name?");
    string characterName = Console.ReadLine();

    Console.WriteLine("Choose your class:");
    Console.WriteLine("1 - Warrior");
    Console.WriteLine("2 - Wizard");
    Console.WriteLine("3 - Rogue");

    while (true)
    {
        string input = Console.ReadLine();

        switch (input)
        {
            case "1":
                Console.WriteLine($"You've created a new Warrior named {characterName}.");
                return new Warrior(characterName);

            case "2":
                Console.WriteLine($"You've created a new Wizard named {characterName}.");
                return new Wizard(characterName);
            case "3":
                Console.WriteLine($"You've created a new Rogue named {characterName}.");
                return new Rogue(characterName);
            default:
                Console.WriteLine("Invalid choice, try again.");
                break;
        }
    }
}

void ChooseSubclass(ref Character player)
{
    Console.WriteLine("Choose your specialization:");

    if (player is Warrior)
    {
        Console.WriteLine("1 - Berserker");
        Console.WriteLine("2 - Knight");
        Console.WriteLine("3 - Paladin");

        string choice = Console.ReadLine();

        if (choice == "1") player = new Berserker(player);
        else if (choice == "2") player = new Knight(player);
        else if (choice == "3") player = new Paladin(player);
    }
    else if (player is Wizard)
    {
        Console.WriteLine("1 - Necromancer");
        Console.WriteLine("2 - Elemental Mage");
        Console.WriteLine("3 - Battlemage");

        string choice = Console.ReadLine();

        if (choice == "1") player = new Necromancer(player);
        else if (choice == "2") player = new ElementalMage(player);
        else if (choice == "3") player = new Battlemage(player);
    }
    else if (player is Rogue)
    {
        Console.WriteLine("1 - Assassin");
        Console.WriteLine("2 - Ranger");
        Console.WriteLine("3 - Gambler");

        string choice = Console.ReadLine();

        if (choice == "1") player = new Assassin(player);
        else if (choice == "2") player = new Ranger(player);
        else if (choice == "3") player = new Gambler(player);
    }

    player.HasSubclass = true;
}



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
        Console.WriteLine("Your choice:");
        string playerchoice = Console.ReadLine();
        Console.WriteLine();
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
            if (enemy is Aeternyx && enemy.Health <= 0)
            {
                endbossDefeated = true;
            }
            Console.WriteLine($"{enemy.Name} got defeated. You won!");
            player.GainXP(20);
            player.StatusEffect = "";
            Thread.Sleep(2000);
            break;
        }
        Thread.Sleep(2000);
        enemy.Attack(enemy.Abilities[RNG.random.Next(0, enemy.Abilities.Length)], player);
        if (player.Health <= 0)
        {
            Console.WriteLine($"{enemy.Name} defeated you. Better luck next time.");
            Thread.Sleep(2000);
            break;
        }
    }
}

void Healing(Character player)
{
    player.Health = player.MaxHealth;
    Console.WriteLine($"You found a fountain and drink from it. You feel strengthened.. HP set to {player.Health}\n");
}

Character CreateMonsterById(int id)
{
    switch (id)
    {
        case 0: return new FireElemental();
        case 1: return new WaterElemental();
        case 2: return new EarthElemental();
        case 3: return new AirElemental();
        case 4: return new ShadowWraith();
        case 5: return new Behir();
        case 6: return new WebDeveloper();
        case 7: return new ClockworkSentinel();
        default: return new FireElemental();
    }
}
while (gameRunning)
{
    Console.Clear();
    Console.WriteLine($"===== RUN {runNumber} =====");

    if (player == null)
    {
        player = CreateStartingCharacter();
    }
    if (runNumber == 1)
    {
        Thread.Sleep(2000);
        Console.WriteLine("\nYou open your eyes. You're laying on the floor, not quite understanding where you are.\n");
        Thread.Sleep(2000);
        Console.WriteLine("Your head aches.\n");
        Thread.Sleep(2000);
        Console.WriteLine("There is a door.\n");
    }
    else if (runNumber == 2)
    {
        Thread.Sleep(2000);
        Console.WriteLine("\nYou wake up in a stone tower.\n");
        Thread.Sleep(2000);
        Console.WriteLine("Again.\n");
        Thread.Sleep(2000);
    }
    else
    {
        Thread.Sleep(1000);
        Console.WriteLine("\nThe tower rebuilds itself around you.\n");
    }
    Thread.Sleep(2000);


    List<int> monsterPool = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 };

    int fightCounter = 1;

    while (fightCounter <= 10 && player.Health > 0)
    {
        Character enemy;

        if (fightCounter == 5)
        {
            enemy = new ArcaneTitan();
            Console.WriteLine("A powerful foe appears...\n");
        }
        else if (fightCounter == 10)
        {
            enemy = new Aeternyx();
            Console.WriteLine("THE FINAL BOSS APPROACHES!\n");
        }
        else
        {
            int randomIndex = RNG.random.Next(monsterPool.Count);
            int monsterId = monsterPool[randomIndex];

            monsterPool.RemoveAt(randomIndex);

            enemy = CreateMonsterById(monsterId);
            ApplyDifficulty(enemy);
        }

        if (fightCounter == 1)
        {
            Console.WriteLine("You step through the door.\n");
            Thread.Sleep(2000);
        }
        else
        {
            string[] travelTexts =
            {
           "Another corridor stretches before you.\n",
           "The tower feels endless.\n",
           "You keep moving forward.\n"
            };

            Console.WriteLine(travelTexts[RNG.random.Next(travelTexts.Length)]);
            Console.WriteLine("You open the next door..");
            Thread.Sleep(2000);
        }

        Console.WriteLine($"Room {fightCounter}: You're attacked by a {enemy.Name}\n");
        Thread.Sleep(2000);

        Fighting(player, enemy);

        if (player.Health <= 0)
        {
            Console.WriteLine("You died...");
            break;
        }

        Healing(player);
        if (fightCounter == 5 && !player.HasSubclass)
        {
            ChooseSubclass(ref player);
        }
        fightDifficulty += 0.35f;
        fightCounter++;
        Thread.Sleep(2000);
    }

    if (endbossDefeated == true)
    {
        runDifficulty += 0.75f;
    }
    fightDifficulty = 1.0f;

    Console.WriteLine("Start a new run?");
    Console.WriteLine("1 - Continue with current character");
    Console.WriteLine("2 - Create new character");
    Console.WriteLine("3 - Exit");

    string input = Console.ReadLine();

    if (input == "1")
    {
        player.Health = player.MaxHealth;
        player.StatusEffect = "";
        runNumber++;
    }
    else if (input == "2")
    {
        player = null;
    }
    else if (input == "3")
    {
        gameRunning = false;
    }
}
