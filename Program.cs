
using System;
using System.Collections.Generic;

namespace Roguelike
{

    class Weapon
    {
        public string Name { get; set; }
        public int Damage { get; set; }
        public int Durability { get; set; }

        public Weapon(string name, int damage, int durability)
        {
            Name = name;
            Damage = damage;
            Durability = durability;
        }

        public int Use()
        {
            if (Durability > 0)
            {
                Durability--;
                return Damage;
            }
            Console.WriteLine($"{Name} сломано!");
            return 1;
        }
    }


    class Aid
    {
        public string Name { get; set; }
        public int HealAmount { get; set; }

        public Aid(string name, int healAmount)
        {
            Name = name;
            HealAmount = healAmount;
        }
    }


    class Enemy
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public Weapon Weapon { get; set; }

        public Enemy(string name, int health, Weapon weapon)
        {
            Name = name;
            MaxHealth = health;
            Health = health;
            Weapon = weapon;
        }

        public int Attack()
        {
            return Weapon.Use();
        }
    }


    class Player
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public Aid AidKit { get; set; }
        public Weapon Weapon { get; set; }
        public int Score { get; set; }

        public Player(string name, int maxHealth, Aid aid, Weapon weapon)
        {
            Name = name;
            MaxHealth = maxHealth;
            Health = maxHealth;
            AidKit = aid;
            Weapon = weapon;
            Score = 0;
        }

        public void Heal()
        {
            if (AidKit != null)
            {
                Health += AidKit.HealAmount;
                if (Health > MaxHealth) Health = MaxHealth;
                Console.WriteLine($"{Name} использовал {AidKit.Name} и восстановил {AidKit.HealAmount} HP");
            }
            else
            {
                Console.WriteLine("Аптечки нет!");
            }
        }

        public int Attack()
        {
            return Weapon.Use();
        }
    }

    class Program
    {
        static Random rnd = new Random();
        static List<string> enemyNames = new List<string> { "Гоблин", "Нежить", "Скелет", "Бандито" };

        static Weapon RandomWeapon()
        {
            string[] names = { "Меч", "Кинжал", "Топор", "Метла" };
            return new Weapon(
            names[rnd.Next(names.Length)],
            rnd.Next(5, 16),
            rnd.Next(3, 8)
            );
        }

        static Enemy GenerateEnemy()
        {
            return new Enemy(
            enemyNames[rnd.Next(enemyNames.Count)],
            rnd.Next(20, 41),
            RandomWeapon()
            );
        }

        static void Main()
        {
            Console.Write("Введите имя игрока: ");
            string playerName = Console.ReadLine();
            var player = new Player(
            playerName,
            50,
            new Aid("Аптечка", 20),
            RandomWeapon()
            );

            Console.WriteLine($"Привет, {player.Name}! Твоя цель — набрать как можно больше очков.");
            Console.WriteLine("Команды: [A]таковать, [H]илиться, [Q]выход");

            while (player.Health > 0)
            {
                Enemy enemy = GenerateEnemy();
                Console.WriteLine($"\nПоявился враг: {enemy.Name} ({enemy.Health} HP) с оружием {enemy.Weapon.Name}");

                while (enemy.Health > 0 && player.Health > 0)
                {
                    Console.Write($"\nВаш HP: {player.Health}, Оружие: {player.Weapon.Name}({player.Weapon.Durability}) | Команда: ");
                    string cmd = Console.ReadLine().ToLower();

                    if (cmd == "a")
                    {
                        int damage = player.Attack();
                        enemy.Health -= damage;
                        Console.WriteLine($"Вы ударили {enemy.Name} на {damage} урона!");
                    }
                    else if (cmd == "h")
                    {
                        player.Heal();
                    }
                    else if (cmd == "q")
                    {
                        Console.WriteLine("Выход из игры...");
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Неверная команда!");
                    }

                    if (enemy.Health > 0)
                    {
                        int enemyDamage = enemy.Attack();
                        player.Health -= enemyDamage;
                        Console.WriteLine($"{enemy.Name} атакует на {enemyDamage} урона!");
                    }
                }

                if (player.Health > 0)
                {
                    player.Score += 10;
                    Console.WriteLine($"Вы победили {enemy.Name}! Очки: {player.Score}");
                }
            }

            Console.WriteLine($"Игра окончена. Ваш результат: {player.Score} очков.");
        }
    }
}