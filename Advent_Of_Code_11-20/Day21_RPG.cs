using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;

namespace Advent_Of_Code_11_20
{
    class Item : IEquatable<Item>
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

        public bool Equals(Item other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Name, other.Name) && Cost == other.Cost && Damage == other.Damage && Armor == other.Armor;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Item) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Name.GetHashCode();
                hashCode = (hashCode*397) ^ Cost;
                hashCode = (hashCode*397) ^ Damage;
                hashCode = (hashCode*397) ^ Armor;
                return hashCode;
            }
        }
    }

    class Unit : IEquatable<Unit>
    {
        public string Name { get; }
        public ReadOnlyCollection<Item> Items { get { return _items.Select(item => new Item(item.Name, item.Cost, item.Damage, item.Armor)).ToList().AsReadOnly(); } }

        public ReadOnlyCollection<Effect> Effects
        {
            get { return new HashSet<Effect>(_effects).ToList().AsReadOnly(); }
        }

        private readonly HashSet<Effect> _effects; 
        private readonly List<Item> _items;
        public int Mana { get; }
        public int HP { get; }
        public bool CanAttack { get; }
        public bool CanUseSpells { get; }

        public int Armor
        {
            get { return _items.Select(item => item.Armor).Sum() + _effects.Select(effect => effect.ArmorModifier).Sum() ; }
        }
        public int Damage
        {
            get { return _items.Select(item => item.Damage).Sum() + _effects.Select(effect => effect.DamageModifier).Sum(); }
        }

        public int Cost
        {
            get { return _items.Select(item => item.Cost).Sum(); }
        }

        public Unit(string name, int hp, int mana, IEnumerable<Item> gear, IEnumerable<Effect> effects, bool canAttack = true, bool canUseSpells = false)
        {
            Name = name;
            HP = hp;
            Mana = mana;
            _items = new List<Item>();
            if(gear != null)
                foreach (var item in gear)
                {
                    _items.Add(item);
                }
            _effects = new HashSet<Effect>();
            if(effects != null)
                foreach (var effect in effects)
                {
                    _effects.Add(effect);
                }

            CanAttack = canAttack;
            CanUseSpells = canUseSpells;
        }

        public Unit(Unit player)
        {
            Name = player.Name;
            HP = player.HP;
            _items = new List<Item>(player._items);
            _effects = new HashSet<Effect>(player._effects);
            CanAttack = player.CanAttack;
            CanUseSpells = player.CanUseSpells;
        }

        public Unit Get_Attacked_By(Unit attacker)
        {
            return !attacker.CanAttack ? this : new Unit(Name, HP - (attacker.Damage - Armor > 0 ? attacker.Damage - Armor : 1), Mana, _items, attacker._effects);
        }

        public Unit Add_Effect(Effect effect)
        {
            HashSet<Effect> new_effects = new HashSet<Effect>(Effects) {effect};
            return new Unit(Name, HP, Mana, _items, new_effects, CanAttack);
        }

        public bool Fight(Unit other)
        {
            string attacker_name = Name;

            Unit attacker = this, defender = other;

            while (attacker.HP > 0 && defender.HP > 0)
            {
                defender = new Unit(defender.Name, defender.HP - (attacker.Damage - defender.Armor > 0 ? attacker.Damage - defender.Armor : 1), defender.Mana, defender.Items, defender._effects);

                // they take turns
                var tmp = attacker;
                attacker = defender;
                defender = tmp;
            }

            return attacker.Name == attacker_name ? attacker.HP > 0 : defender.HP > 0;
        }


        public bool Equals(Unit other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Unit) obj);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
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
            var all_item_setup = new[] { weapon_choices, armor_choices, ring_choices }.CartesianProduct();


            foreach (var items in all_item_setup)
            {
                Unit boss = new Unit("boss",
                    int.Parse(inputLines[0].Split().Last()),
                    0,
                    new List<Item>()
                            {
                                new Item("item",
                                    0,
                                    int.Parse(inputLines[1].Split().Last()),
                                    int.Parse(inputLines[2].Split().Last()))
                            },null);


                Unit player = new Unit("player", 100, 0, items.SelectMany(list => list), null);

                if (player.Fight(boss))
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
