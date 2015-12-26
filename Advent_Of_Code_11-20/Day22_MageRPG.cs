using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent_Of_Code_11_20
{
    internal delegate Unit ApplyEffectOnUnit(Unit unit);

    enum EffectEnum
    {
        Shield,
        Recharge,
        Poison
    }

    internal abstract class Effect : IEquatable<Effect>, ICloneable
    {
        private readonly EffectEnum _effectType;
        protected readonly int Lasts;
        public readonly int DamageModifier;
        public readonly int ArmorModifier;
        protected readonly ApplyEffectOnUnit ApplyEffectMethod;

        protected Effect(EffectEnum effectType, ApplyEffectOnUnit applyEffectMethod, int lasts, int armorModifier, int damageModifier)
        {
            _effectType = effectType;
            ApplyEffectMethod = applyEffectMethod;
            Lasts = lasts;
            ArmorModifier = armorModifier;
            DamageModifier = damageModifier;
        }

        protected Effect(Effect copyOf)
        {
            _effectType = copyOf._effectType;
            ApplyEffectMethod = copyOf.ApplyEffectMethod;
            ArmorModifier = copyOf.ArmorModifier;
            DamageModifier = copyOf.DamageModifier;
        }

        public Unit Apply(Unit unitOfOperation)
        {
            var unit_with_effect_wore_off_applied = new Unit(unitOfOperation.Name,
                unitOfOperation.HP,
                unitOfOperation.Mana,
                unitOfOperation.Items,
                unitOfOperation.Effects.Select(
                    effect => effect._effectType == _effectType ? effect.ApplyWearOff() : effect).
                    Where(effect => effect.Lasts > 0),
                unitOfOperation.CanAttack,
                unitOfOperation.CanUseSpells);


            return ApplyEffectMethod(unit_with_effect_wore_off_applied);
        }

        public bool Equals(Effect other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Lasts == other.Lasts && _effectType == other._effectType;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Effect)obj);
        }

        public override int GetHashCode()
        {
            return Lasts;
        }

        public abstract object Clone();

        protected abstract Effect ApplyWearOff();
    }

    internal class ShieldEffect : Effect
    {
        public ShieldEffect()
            : this(unit => unit, 6, 7, 0)
        {
        }

        public ShieldEffect(Effect copyOf)
            : base(copyOf)
        {
        }

        private ShieldEffect(ApplyEffectOnUnit applyEffectMethod, int lasts, int armorModifier, int damageModifier)
            : base(EffectEnum.Shield, applyEffectMethod, lasts, armorModifier, damageModifier)
        {
        }

        public override object Clone()
        {
            return new ShieldEffect(ApplyEffectMethod, Lasts, ArmorModifier, DamageModifier);
        }

        protected override Effect ApplyWearOff()
        {
            return new ShieldEffect(ApplyEffectMethod, Lasts - 1, ArmorModifier, DamageModifier);
        }
    }

    internal class PoisonEffect : Effect
    {
        public PoisonEffect()
            : this(unit => new Unit(unit.Name, unit.HP - 3, unit.Mana, unit.Items, unit.Effects, unit.CanAttack, unit.CanUseSpells),
                6,
                0,
                0)
        {
        }

        public PoisonEffect(Effect copyOf)
            : base(copyOf)
        {
        }
        private PoisonEffect(ApplyEffectOnUnit applyEffectMethod, int lasts, int armorModifier, int damageModifier)
            : base(EffectEnum.Poison, applyEffectMethod, lasts, armorModifier, damageModifier)
        {
        }

        public override object Clone()
        {
            return new PoisonEffect(ApplyEffectMethod, Lasts, ArmorModifier, DamageModifier);
        }

        protected override Effect ApplyWearOff()
        {
            return new PoisonEffect(ApplyEffectMethod, Lasts - 1, ArmorModifier, DamageModifier);
        }
    }

    internal class RechargeEffect : Effect
    {
        public RechargeEffect()
            : this(
                  unit
                  =>
                  new Unit(unit.Name, unit.HP, unit.Mana + 101, unit.Items, unit.Effects, unit.CanAttack, unit.CanUseSpells)
                  , 5, 0, 0)
        {
        }

        private RechargeEffect(ApplyEffectOnUnit applyEffectMethod, int lasts, int armorModifier, int damageModifier)
            : base(EffectEnum.Recharge, applyEffectMethod, lasts, armorModifier, damageModifier)
        {
        }

        public RechargeEffect(Effect copyOf)
            : base(copyOf)
        {
        }

        public override object Clone()
        {
            return new RechargeEffect(ApplyEffectMethod, Lasts, ArmorModifier, DamageModifier);
        }

        protected override Effect ApplyWearOff()
        {
            return new RechargeEffect(ApplyEffectMethod, Lasts - 1, ArmorModifier, DamageModifier);
        }
    }

    internal class RpgHeuristic : IHeuristic<RpgFightState>
    {
        public float Heuristic_Distance(Node<RpgFightState> node, Problem<RpgFightState> p)
        {
            return ((float)p.StartNode.State.Player.Mana / node.State.Player.Mana + (float)p.StartNode.State.Player.HP / node.State.Player.HP - (float)p.StartNode.State.Npc.HP / node.State.Npc.HP) 
                / (node.State.Npc.Effects.Count + node.State.Player.Effects.Count) * node.State.Npc.HP;
        }
    }

    internal class RpgFightState : IEquatable<RpgFightState>
    {
        public readonly Unit Player;
        public readonly Unit Npc;
        public readonly bool IsPlayerTurn;

        public RpgFightState(Unit player, Unit npc, bool isPlayerTurn)
        {
            Player = player;
            Npc = npc;
            IsPlayerTurn = isPlayerTurn;
        }

        public bool Equals(RpgFightState other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return IsPlayerTurn == other.IsPlayerTurn && Player.HP == other.Player.HP && Player.Mana == other.Player.Mana && Npc.HP == other.Npc.HP
                && Player.Effects.Equals(other.Player.Effects) && Npc.Effects.Equals(other.Npc.Effects)
                ;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((RpgFightState)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash_code = Player.GetHashCode();
                hash_code = (hash_code * 397) ^ Npc.GetHashCode();
                hash_code = (hash_code * 397) ^ IsPlayerTurn.GetHashCode();
                hash_code = (hash_code * 397) ^ Npc.HP;
                hash_code = (hash_code * 397) ^ Player.HP;
                hash_code = (hash_code * 397) ^ Player.Mana;
                return hash_code;
            }
        }
    }

    internal class Attack : Operator<RpgFightState>
    {
        public Attack()
            : base(0)
        {
        }

        public override Node<RpgFightState> Apply(Node<RpgFightState> node)
        {
            var player_new = new Unit(node.State.Player);
            var npc_new = new Unit(node.State.Npc);

            player_new = player_new.Effects.Aggregate(player_new, (current, effect) => effect.Apply(current));
            npc_new = npc_new.Effects.Aggregate(npc_new, (current, effect) => effect.Apply(current));

            if (npc_new.HP > 0)
                player_new = player_new.Get_Attacked_By(npc_new);


            var new_state = new RpgFightState(player_new, npc_new, !node.State.IsPlayerTurn);

            return new Node<RpgFightState>(new_state, node, this, node.CostSoFar + Cost);
        }
    }

    internal class MagicMissile : Operator<RpgFightState>
    {
        public const int ManaCost = 53;

        public MagicMissile()
            : base(ManaCost)
        {
        }

        public override Node<RpgFightState> Apply(Node<RpgFightState> node)
        {
            var player_new = new Unit(node.State.Player);
            var npc_new = new Unit(node.State.Npc);

            player_new = player_new.Effects.Aggregate(player_new, (current, effect) => effect.Apply(current));
            npc_new = npc_new.Effects.Aggregate(npc_new, (current, effect) => effect.Apply(current));

            player_new = new Unit(player_new.Name, player_new.HP, player_new.Mana - Cost, player_new.Items, player_new.Effects, false, true);
            npc_new = new Unit(npc_new.Name, npc_new.HP - 4, npc_new.Mana, npc_new.Items, npc_new.Effects);

            var new_state = new RpgFightState(player_new, npc_new, !node.State.IsPlayerTurn);

            return new Node<RpgFightState>(new_state, node, this, node.CostSoFar + Cost);
        }
    }

    internal class Drain : Operator<RpgFightState>
    {
        public const int ManaCost = 73;

        public Drain()
            : base(ManaCost)
        {
        }

        public override Node<RpgFightState> Apply(Node<RpgFightState> node)
        {
            var player_new = new Unit(node.State.Player);
            var npc_new = new Unit(node.State.Npc);

            player_new = player_new.Effects.Aggregate(player_new, (current, effect) => effect.Apply(current));
            npc_new = npc_new.Effects.Aggregate(npc_new, (current, effect) => effect.Apply(current));

            player_new = new Unit(player_new.Name, player_new.HP + 2, player_new.Mana - Cost, player_new.Items, player_new.Effects, false, true);
            npc_new = new Unit(npc_new.Name, npc_new.HP - 2, npc_new.Mana, npc_new.Items, npc_new.Effects);

            var new_state = new RpgFightState(player_new, npc_new, !node.State.IsPlayerTurn);

            return new Node<RpgFightState>(new_state, node, this, node.CostSoFar + Cost);
        }
    }

    internal class Shield : Operator<RpgFightState>
    {
        public const int ManaCost = 113;

        public Shield()
            : base(113)
        {
        }

        public override Node<RpgFightState> Apply(Node<RpgFightState> node)
        {
            var player_new = new Unit(node.State.Player);
            var npc_new = new Unit(node.State.Npc);

            player_new = player_new.Effects.Aggregate(player_new, (current, effect) => effect.Apply(current));
            npc_new = npc_new.Effects.Aggregate(npc_new, (current, effect) => effect.Apply(current));

            var new_player_effects = new List<Effect>(player_new.Effects)
            {
                new ShieldEffect()
            };

            player_new = new Unit(player_new.Name, player_new.HP, player_new.Mana - Cost, player_new.Items, new_player_effects, false, true);

            var new_state = new RpgFightState(player_new, npc_new, !node.State.IsPlayerTurn);

            return new Node<RpgFightState>(new_state, node, this, node.CostSoFar + Cost);
        }
    }

    internal class Poison : Operator<RpgFightState>
    {
        public const int ManaCost = 173;

        public Poison()
            : base(ManaCost)
        {
        }

        public override Node<RpgFightState> Apply(Node<RpgFightState> node)
        {
            var player_new = new Unit(node.State.Player);
            var npc_new = new Unit(node.State.Npc);

            player_new = player_new.Effects.Aggregate(player_new, (current, effect) => effect.Apply(current));
            npc_new = npc_new.Effects.Aggregate(npc_new, (current, effect) => effect.Apply(current));

            var boss_effects = new List<Effect>(npc_new.Effects)
            {
                new PoisonEffect()
            };

            player_new = new Unit(player_new.Name, player_new.HP, player_new.Mana - Cost, player_new.Items, player_new.Effects, false, true);
            npc_new = new Unit(npc_new.Name, npc_new.HP, npc_new.Mana, npc_new.Items, boss_effects);


            var new_state = new RpgFightState(player_new, npc_new, !node.State.IsPlayerTurn);

            return new Node<RpgFightState>(new_state, node, this, node.CostSoFar + Cost);
        }
    }

    internal class Recharge : Operator<RpgFightState>
    {
        public const int ManaCost = 229;

        public Recharge()
            : base(ManaCost)
        {
        }

        public override Node<RpgFightState> Apply(Node<RpgFightState> node)
        {
            var player_new = new Unit(node.State.Player);
            var npc_new = new Unit(node.State.Npc);

            player_new = player_new.Effects.Aggregate(player_new, (current, effect) => effect.Apply(current));
            npc_new = npc_new.Effects.Aggregate(npc_new, (current, effect) => effect.Apply(current));

            var new_player_effects = new List<Effect>(player_new.Effects)
            {
                new RechargeEffect()
            };

            player_new = new Unit(player_new.Name, player_new.HP, player_new.Mana - Cost, player_new.Items, new_player_effects, false, true);

            var new_state = new RpgFightState(player_new, npc_new, !node.State.IsPlayerTurn);

            return new Node<RpgFightState>(new_state, node, this, node.CostSoFar + Cost);
        }
    }

    internal class RpgProblem : Problem<RpgFightState>
    {
        private static readonly Queue<Operator<RpgFightState>> PlayerOperators;

        static RpgProblem()
        {
            PlayerOperators = new Queue<Operator<RpgFightState>>();
            PlayerOperators.Enqueue(new Recharge());
            PlayerOperators.Enqueue(new Shield());
            PlayerOperators.Enqueue(new Drain());
            PlayerOperators.Enqueue(new Poison());
            PlayerOperators.Enqueue(new MagicMissile());
            PlayerOperators.Enqueue(new MagicMissile());
        }
        public RpgProblem(Node<RpgFightState> startNode)
            : base(startNode)
        {
        }

        public override List<Operator<RpgFightState>> Available_Operators(Node<RpgFightState> node)
        {
            if (node.State.IsPlayerTurn)
            {
                //  return new List<Operator<RpgFightState>> { player_operators.Dequeue() };
                var ret = new List<Operator<RpgFightState>>();

                if (node.State.Player.HP <= 0)
                    return ret;

                if (Drain.ManaCost < node.State.Player.Mana)
                    ret.Add(new Drain());
                if (MagicMissile.ManaCost < node.State.Player.Mana)
                    ret.Add(new MagicMissile());

                if (!node.State.Player.Effects.Select(effect => effect.GetType() == typeof(Recharge)).Contains(true) && Recharge.ManaCost < node.State.Player.Mana)
                    ret.Add(new Recharge());
                if (!node.State.Player.Effects.Select(effect => effect.GetType() == typeof(Shield)).Contains(true) && Shield.ManaCost < node.State.Player.Mana)
                    ret.Add(new Shield());
                if (!node.State.Npc.Effects.Select(effect => effect.GetType() == typeof(Poison)).Contains(true) && Poison.ManaCost < node.State.Player.Mana)
                    ret.Add(new Poison());

                return ret;
            }
            return new List<Operator<RpgFightState>>() { new Attack() };
        }

        public override bool Is_Goal_State(Node<RpgFightState> node)
        {
            return node.State.Npc.HP <= 0;
        }
    }

    internal class Day22MageRpg : ISolvable
    {


        public string Solve(string[] inputLines, bool isPart2)
        {
            var boss = new Unit("boss", int.Parse(inputLines[0].Split().Last()), 0, new[] { new Item("boss_weapon", 0, int.Parse(inputLines[1].Split().Last()), 0) }, null);
            var player = new Unit("player", 10, 250, null, null, false, true);

            var solver = new OptimisticSearch<RpgFightState>(new RpgProblem(new Node<RpgFightState>(new RpgFightState(player, boss, true), null, null, 0)), new RpgHeuristic());
            if (!solver.Solve())
                throw new IHaveABadFeelingAboutThisException("There should definitely be a solution");

            return solver.Solution.CostSoFar.ToString();
        }
    }
}
