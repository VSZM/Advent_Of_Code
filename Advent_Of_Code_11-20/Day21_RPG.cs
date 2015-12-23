using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Advent_Of_Code_11_20
{
    class Item
    {
        public string Name { get; }
        public int Cost { get; }
        public int Damage { get; }
        public int Armor { get; }

        public Item(string name, int cost, int damage, int armor)
        {
            Name = name;
            Cost = cost;
            Damage = damage;
            Armor = armor;
        }
    }

    class Unit
    {
        public string Name { get; }
        public List<Item> Items { get { return _items.Select(item => new Item(item.Name, item.Cost, item.Damage, item.Armor)).ToList(); } }
        private readonly List<Item> _items;
        public int HP { get; private set; }
        public int Armor
        {
            get { return _items.Select(item => item.Armor).Sum(); }
        }
        public int Damage
        {
            get { return _items.Select(item => item.Damage).Sum(); }
        }

        public int Cost
        {
            get { return _items.Select(item => item.Cost).Sum(); }
        }

        public Unit(string name, int hp, IEnumerable<Item> gear)
        {
            Name = name;
            HP = hp;
            _items = new List<Item>(gear);
        }

        public bool Attack(Unit other)
        {
            Unit attacker = this, defender = other, tmp;

            while (attacker.HP > 0 && defender.HP > 0)
            {
                defender.HP -= (attacker.Damage - defender.Armor > 0 ? attacker.Damage - defender.Armor : 1);

                // they take turns
                tmp = attacker;
                attacker = defender;
                defender = tmp;
            }

            return HP > 0;
        }
    }



    class Day21Rpg : ISolvable
    {
        private static readonly List<Item> weapons;
        private static readonly List<Item> armors;
        private static readonly List<Item> rings;


        private const string WeaponsStr =
            @"Weapons:    Cost  Damage  Armor
Dagger        8     4       0
Shortsword   10     5       0
Warhammer    25     6       0
Longsword    40     7       0
Greataxe     74     8       0";
        private const string ArmorsStr =
            @"Armor:      Cost  Damage  Armor
Leather      13     0       1
Chainmail    31     0       2
Splintmail   53     0       3
Bandedmail   75     0       4
Platemail   102     0       5";
        private const string RingsStr =
            @"Rings:      Cost  Damage  Armor
Damage +1    25     1       0
Damage +2    50     2       0
Damage +3   100     3       0
Defense +1   20     0       1
Defense +2   40     0       2
Defense +3   80     0       3";
        static Day21Rpg()
        {
            weapons = WeaponsStr.Split('\n').Where(line => !line.Contains(':')).Select(line =>
            {
                string[] splitted_line = line.Replace(" +", "+").Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                //Console.WriteLine(string.Join(", ", splitted_line));
                return new Item(splitted_line[0], int.Parse(splitted_line[1]), int.Parse(splitted_line[2]), int.Parse(splitted_line[3]));
            }).ToList();

            armors = ArmorsStr.Split('\n').Where(line => !line.Contains(':')).Select(line =>
            {
                string[] splitted_line = line.Replace(" +", "+").Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                //Console.WriteLine(string.Join(", ", splitted_line));
                return new Item(splitted_line[0], int.Parse(splitted_line[1]), int.Parse(splitted_line[2]), int.Parse(splitted_line[3]));
            }).ToList();

            rings = RingsStr.Split('\n').Where(line => !line.Contains(':')).Select(line =>
            {
                string[] splitted_line = line.Replace(" +", "+").Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                //Console.WriteLine(string.Join(", ", splitted_line));

                return new Item(splitted_line[0], int.Parse(splitted_line[1]), int.Parse(splitted_line[2]), int.Parse(splitted_line[3]));
            }).ToList();
        }

        public string Solve(string[] inputLines, bool isPart2)
        {

            int min_cost = int.MaxValue;
            int max_cost = int.MinValue;
            var weapon_choices = weapons.Combinations(1).ToList();
            var armor_choices = armors.Combinations(1).ToList();
            armor_choices.AddRange(armors.Combinations(0));
            var ring_choices = rings.Combinations(0).ToList();
            ring_choices.AddRange(rings.Combinations(1));
            ring_choices.AddRange(rings.Combinations(2));
            var all_item_setup = new List<IEnumerable<Item>>[] { weapon_choices, armor_choices, ring_choices }.CartesianProduct();


            foreach (var items in all_item_setup)
            {
                Unit boss = new Unit("boss",
                    int.Parse(inputLines[0].Split().Last()),
                    new List<Item>()
                            {
                                new Item("item",
                                    0,
                                    int.Parse(inputLines[1].Split().Last()),
                                    int.Parse(inputLines[2].Split().Last()))
                            });


                Unit player = new Unit("player", 100, items.SelectMany(list => list));

                if (player.Attack(boss))
                {
                    if (player.Cost < min_cost)
                        min_cost = player.Cost;
                }
                else if (player.Cost > max_cost)
                    max_cost = player.Cost;
            }

            return isPart2 ? max_cost.ToString() : min_cost.ToString();
        }
    }
}
