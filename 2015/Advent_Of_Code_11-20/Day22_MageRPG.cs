using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;

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
        public readonly EffectEnum EffectType;
        public readonly int Lasts;
        public readonly int DamageModifier;
        public readonly int ArmorModifier;
        protected readonly ApplyEffectOnUnit ApplyEffectMethod;

        protected Effect(EffectEnum effectType, ApplyEffectOnUnit applyEffectMethod, int lasts, int armorModifier, int damageModifier)
        {
            EffectType = effectType;
            ApplyEffectMethod = applyEffectMethod;
            Lasts = lasts;
            ArmorModifier = armorModifier;
            DamageModifier = damageModifier;
        }

        protected Effect(Effect copyOf)
        {
            EffectType = copyOf.EffectType;
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
                    effect => effect.EffectType == EffectType ? effect.ApplyWearOff() : effect).
                    Where(effect => effect.Lasts > 0),
                unitOfOperation.CanAttack,
                unitOfOperation.CanUseSpells);


            return ApplyEffectMethod(unit_with_effect_wore_off_applied);
        }

        public bool Equals(Effect other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Lasts == other.Lasts && EffectType == other.EffectType;
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

            return node.State.Npc.HP/* / (node.State.Player.HP + 1 + node.State.Player.Armor + node.State.Player.Effects.Count + node.State.Npc.Effects.Count) */* 15;
            /*         int player_turns_left = node.State.Player.HP / (node.State.Npc.Damage - node.State.Player.Armor);
            int boss_turns_left = node.State.Npc.HP / 4;

            return boss_turns_left / (float)player_turns_left - node.State.Player.Mana ;
     */
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
            bool ret = IsPlayerTurn == other.IsPlayerTurn && Player.HP == other.Player.HP &&
                       Player.Mana == other.Player.Mana && Npc.HP == other.Npc.HP
                       && Player.Effects.Equals(other.Player.Effects) && Npc.Effects.Equals(other.Npc.Effects);

            return ret;
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
        private readonly bool _addBleed;

        public MagicMissile(bool bleed)
            : base(ManaCost)
        {
            _addBleed = bleed;
        }

        public override Node<RpgFightState> Apply(Node<RpgFightState> node)
        {
            var player_new = new Unit(node.State.Player);
            var npc_new = new Unit(node.State.Npc);

            player_new = player_new.Effects.Aggregate(player_new, (current, effect) => effect.Apply(current));
            npc_new = npc_new.Effects.Aggregate(npc_new, (current, effect) => effect.Apply(current));

            player_new = new Unit(player_new.Name, player_new.HP - (_addBleed ? 1 : 0), player_new.Mana - Cost, player_new.Items, player_new.Effects, false, true);
            npc_new = new Unit(npc_new.Name, npc_new.HP - 4, npc_new.Mana, npc_new.Items, npc_new.Effects);

            var new_state = new RpgFightState(player_new, npc_new, !node.State.IsPlayerTurn);

            return new Node<RpgFightState>(new_state, node, this, node.CostSoFar + Cost);
        }
    }

    internal class Drain : Operator<RpgFightState>
    {
        public const int ManaCost = 73;
        private readonly bool _addBleed;


        public Drain(bool bleed)
            : base(ManaCost)
        {
            _addBleed = bleed;
        }

        public override Node<RpgFightState> Apply(Node<RpgFightState> node)
        {
            var player_new = new Unit(node.State.Player);
            var npc_new = new Unit(node.State.Npc);

            player_new = player_new.Effects.Aggregate(player_new, (current, effect) => effect.Apply(current));
            npc_new = npc_new.Effects.Aggregate(npc_new, (current, effect) => effect.Apply(current));

            player_new = new Unit(player_new.Name, player_new.HP + 2 - (_addBleed ? 1 : 0), player_new.Mana - Cost, player_new.Items, player_new.Effects, false, true);
            npc_new = new Unit(npc_new.Name, npc_new.HP - 2, npc_new.Mana, npc_new.Items, npc_new.Effects);

            var new_state = new RpgFightState(player_new, npc_new, !node.State.IsPlayerTurn);

            return new Node<RpgFightState>(new_state, node, this, node.CostSoFar + Cost);
        }
    }

    internal class Shield : Operator<RpgFightState>
    {
        private readonly bool _addBleed;
        public const int ManaCost = 113;

        public Shield(bool bleed)
            : base(113)
        {
            _addBleed = bleed;
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

            player_new = new Unit(player_new.Name, player_new.HP - (_addBleed ? 1 : 0), player_new.Mana - Cost, player_new.Items, new_player_effects, false, true);

            var new_state = new RpgFightState(player_new, npc_new, !node.State.IsPlayerTurn);

            return new Node<RpgFightState>(new_state, node, this, node.CostSoFar + Cost);
        }
    }

    internal class Poison : Operator<RpgFightState>
    {
        private readonly bool _addBleed;
        public const int ManaCost = 173;

        public Poison(bool bleed)
            : base(ManaCost)
        {
            _addBleed = bleed;
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

            player_new = new Unit(player_new.Name, player_new.HP - (_addBleed ? 1 : 0), player_new.Mana - Cost, player_new.Items, player_new.Effects, false, true);
            npc_new = new Unit(npc_new.Name, npc_new.HP, npc_new.Mana, npc_new.Items, boss_effects);


            var new_state = new RpgFightState(player_new, npc_new, !node.State.IsPlayerTurn);

            return new Node<RpgFightState>(new_state, node, this, node.CostSoFar + Cost);
        }
    }

    internal class Recharge : Operator<RpgFightState>
    {
        private readonly bool _addBleed;
        public const int ManaCost = 229;



        public Recharge(bool bleed)
            : base(ManaCost)
        {
            _addBleed = bleed;
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

            player_new = new Unit(player_new.Name, player_new.HP - (_addBleed ? 1 : 0), player_new.Mana - Cost, player_new.Items, new_player_effects, false, true);

            var new_state = new RpgFightState(player_new, npc_new, !node.State.IsPlayerTurn);

            return new Node<RpgFightState>(new_state, node, this, node.CostSoFar + Cost);
        }
    }

    internal class RpgProblem : Problem<RpgFightState>
    {
        private static readonly Queue<Operator<RpgFightState>> PlayerOperators;
        private static readonly Operator<RpgFightState> RECHARGE_OPERATOR = new Recharge(false);
        private static readonly Operator<RpgFightState> SHIELD_OPERATOR = new Shield(false);
        private static readonly Operator<RpgFightState> POISON_OPERATOR = new Poison(false);
        private static readonly Operator<RpgFightState> DRAIN_OPERATOR = new Drain(false);
        private static readonly Operator<RpgFightState> MAGIC_MISSILE_OPERATOR = new MagicMissile(false);
        private static readonly Operator<RpgFightState> ATTACK_OPERATOR = new Attack();
        private static readonly Operator<RpgFightState> HARD_RECHARGE_OPERATOR = new Recharge(true);
        private static readonly Operator<RpgFightState> HARD_SHIELD_OPERATOR = new Shield(true);
        private static readonly Operator<RpgFightState> HARD_POISON_OPERATOR = new Poison(true);
        private static readonly Operator<RpgFightState> HARD_DRAIN_OPERATOR = new Drain(true);
        private static readonly Operator<RpgFightState> HARD_MAGIC_MISSILE_OPERATOR = new MagicMissile(true);


        private readonly bool _isHard;



        static RpgProblem()
        {
            PlayerOperators = new Queue<Operator<RpgFightState>>();
            PlayerOperators.Enqueue(POISON_OPERATOR);
            PlayerOperators.Enqueue(MAGIC_MISSILE_OPERATOR);
            PlayerOperators.Enqueue(RECHARGE_OPERATOR);
            PlayerOperators.Enqueue(MAGIC_MISSILE_OPERATOR);
            PlayerOperators.Enqueue(POISON_OPERATOR);
            PlayerOperators.Enqueue(SHIELD_OPERATOR);
            PlayerOperators.Enqueue(MAGIC_MISSILE_OPERATOR);
            PlayerOperators.Enqueue(MAGIC_MISSILE_OPERATOR);
        }


        public RpgProblem(Node<RpgFightState> startNode, bool isHard)
            : base(startNode)
        {
            _isHard = isHard;
        }

        public override List<Operator<RpgFightState>> Available_Operators(Node<RpgFightState> node)
        {
            if (node.State.IsPlayerTurn)
            {
                var ret = new List<Operator<RpgFightState>>();

                if (node.State.Player.HP <= 0)
                    return ret;

                if (Drain.ManaCost < node.State.Player.Mana)
                    ret.Add(_isHard ? HARD_DRAIN_OPERATOR : DRAIN_OPERATOR);
                if (MagicMissile.ManaCost < node.State.Player.Mana)
                    ret.Add(_isHard ? HARD_MAGIC_MISSILE_OPERATOR : MAGIC_MISSILE_OPERATOR);
                if (!node.State.Player.Effects.Select(effect => effect.EffectType == EffectEnum.Recharge && effect.Lasts > 1).Contains(true) && Recharge.ManaCost < node.State.Player.Mana)
                    ret.Add(_isHard ? HARD_RECHARGE_OPERATOR : RECHARGE_OPERATOR);
                if (!node.State.Player.Effects.Select(effect => effect.EffectType == EffectEnum.Shield && effect.Lasts > 1).Contains(true) && Shield.ManaCost < node.State.Player.Mana)
                    ret.Add(_isHard ? HARD_SHIELD_OPERATOR : SHIELD_OPERATOR);
                if (!node.State.Npc.Effects.Select(effect => effect.EffectType == EffectEnum.Poison && effect.Lasts > 1).Contains(true) && Poison.ManaCost < node.State.Player.Mana)
                    ret.Add(_isHard ? HARD_POISON_OPERATOR : POISON_OPERATOR);

                return ret;
            }
            return new List<Operator<RpgFightState>>() { ATTACK_OPERATOR };
        }

        public override bool Is_Goal_State(Node<RpgFightState> node)
        {
            return node.State.Npc.HP <= 0 && node.State.Player.HP > 0;
        }
    }

    internal class Day22MageRpg : ISolvable
    {


        public string Solve(string[] inputLines, bool isPart2)
        {
            var boss = new Unit("boss", int.Parse(inputLines[0].Split().Last()), 0, new[] { new Item("boss_weapon", 0, int.Parse(inputLines[1].Split().Last()), 0) }, null);
            var player = new Unit("player", 50, 500, null, null, false, true);

            var solver = new BestSolutionLimitedGraphSearchSolver<RpgFightState>(new RpgProblem(new Node<RpgFightState>(new RpgFightState(player, boss, true), null, null, 0), isPart2), 1350);
            if (!solver.Solve())
                throw new IHaveABadFeelingAboutThisException("There should definitely be a solution");



            return solver.Solution.CostSoFar.ToString();
        }
    }

}
