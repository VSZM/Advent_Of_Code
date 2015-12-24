using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent_Of_Code_11_20
{
    delegate Unit ApplyEffectOnUnit(Unit unit);

    abstract class Effect : IEquatable<Effect>
    {
        protected readonly int Lasts;
        public readonly int DamageModifier;
        public readonly int ArmorModifier;
        protected readonly ApplyEffectOnUnit ApplyEffectMethod;

        protected Effect(ApplyEffectOnUnit applyEffectMethod, int lasts, int armorModifier, int damageModifier)
        {
            ApplyEffectMethod = applyEffectMethod;
            Lasts = lasts;
            ArmorModifier = armorModifier;
            DamageModifier = damageModifier;
        }

        protected Effect(Effect copyOf)
        {
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
                    effect => effect.ApplyWearOff()
                    ).Where(effect => effect.Lasts > 0), unitOfOperation.CanAttack);


            return ApplyEffectMethod(unit_with_effect_wore_off_applied);
        }

        public bool Equals(Effect other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Lasts == other.Lasts && GetType() == other.GetType();
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

        protected abstract Effect ApplyWearOff();
    }

    class ShieldEffect : Effect
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
            : base(applyEffectMethod, lasts, armorModifier, damageModifier)
        {
        }

        protected override Effect ApplyWearOff()
        {
            return new ShieldEffect(ApplyEffectMethod, Lasts - 1, ArmorModifier, DamageModifier);
        }
    }

    class PoisonEffect : Effect
    {
        public PoisonEffect()
            : this(unit => new Unit(unit.Name, unit.HP - 3, unit.Mana, unit.Items, unit.Effects, unit.CanAttack),
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
            : base(applyEffectMethod, lasts, armorModifier, damageModifier)
        {
        }

        protected override Effect ApplyWearOff()
        {
            return new PoisonEffect(ApplyEffectMethod, Lasts - 1, ArmorModifier, DamageModifier);
        }
    }

    class RechargeEffect : Effect
    {
        public RechargeEffect()
            : this(unit => new Unit(unit.Name, unit.HP, unit.Mana + 101, unit.Items, unit.Effects, unit.CanAttack), 5, 0, 0)
        {
        }

        private RechargeEffect(ApplyEffectOnUnit applyEffectMethod, int lasts, int armorModifier, int damageModifier)
            : base(applyEffectMethod, lasts, armorModifier, damageModifier)
        {
        }

        public RechargeEffect(Effect copyOf)
            : base(copyOf)
        {
        }

        protected override Effect ApplyWearOff()
        {
            return new RechargeEffect(ApplyEffectMethod, Lasts - 1, ArmorModifier, DamageModifier);
        }
    }

    class RpgFightState : IEquatable<RpgFightState>
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
            return IsPlayerTurn == other.IsPlayerTurn && Player.HP == other.Player.HP && Player.Mana == other.Player.Mana && Player.Effects.Equals(other.Player.Effects) &&
                Npc.HP == other.Npc.HP && Npc.Effects.Equals(other.Npc.Effects);
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
                return hash_code;
            }
        }
    }

    //class RpgBaseOperator : Operator<RpgFightState>
    //{
    //    public RpgBaseOperator(int cost) : base(cost)
    //    {
    //    }

    //    public override Node<RpgFightState> Apply(Node<RpgFightState> node)
    //    {
    //        Unit player_new = node.State.Player;
    //        Unit npc_new = node.State.Npc;

    //        foreach (Effect effect in node.State.Player.Effects)
    //        {
    //            player_new = effect.Apply();
    //        }

    //        foreach (Effect effect in node.State.Npc.Effects)
    //        {
    //            npc_new = effect.Apply();
    //        }

    //        RpgFightState new_state = new RpgFightState(player_new, npc_new, !node.State.IsPlayerTurn);

    //        return new Node<RpgFightState>(new_state, node, this, node.CostSoFar + );
    //    }
    //}

    class Attack : Operator<RpgFightState>
    {
        public Attack()
            : base(0)
        {
        }

        public override Node<RpgFightState> Apply(Node<RpgFightState> node)
        {
            Unit player_new = new Unit(node.State.Player);
            Unit npc_new = new Unit(node.State.Npc);

            player_new = node.State.Player.Effects.Aggregate(player_new, (current, effect) => effect.Apply(current));
            npc_new = node.State.Npc.Effects.Aggregate(npc_new, (current, effect) => effect.Apply(current));

            player_new = player_new.Get_Attacked_By(npc_new);


            RpgFightState new_state = new RpgFightState(player_new, npc_new, !node.State.IsPlayerTurn);

            return new Node<RpgFightState>(new_state, node, this, node.CostSoFar + Cost);
        }
    }

    class MagicMissile : Operator<RpgFightState>
    {
        public MagicMissile()
            : base(53)
        {
        }

        public override Node<RpgFightState> Apply(Node<RpgFightState> node)
        {
            Unit player_new = new Unit(node.State.Player);
            Unit npc_new = new Unit(node.State.Npc);

            // TODO TRY WITH CALLING IT ON THE COPY
            player_new = node.State.Player.Effects.Aggregate(player_new, (current, effect) => effect.Apply(current));
            npc_new = node.State.Npc.Effects.Aggregate(npc_new, (current, effect) => effect.Apply(current));

            player_new = new Unit(player_new.Name, player_new.HP, player_new.Mana - Cost, player_new.Items, player_new.Effects, false, true);
            npc_new = new Unit(npc_new.Name, npc_new.HP - 4, npc_new.Mana, npc_new.Items, npc_new.Effects);

            RpgFightState new_state = new RpgFightState(player_new, npc_new, !node.State.IsPlayerTurn);

            return new Node<RpgFightState>(new_state, node, this, node.CostSoFar + Cost);
        }
    }

    class Drain : Operator<RpgFightState>
    {
        public Drain()
            : base(73)
        {
        }

        public override Node<RpgFightState> Apply(Node<RpgFightState> node)
        {
            Unit player_new = new Unit(node.State.Player);
            Unit npc_new = new Unit(node.State.Npc);

            player_new = node.State.Player.Effects.Aggregate(player_new, (current, effect) => effect.Apply(current));
            npc_new = node.State.Npc.Effects.Aggregate(npc_new, (current, effect) => effect.Apply(current));

            player_new = new Unit(player_new.Name, player_new.HP + 2, player_new.Mana - Cost, player_new.Items, player_new.Effects, false, true);
            npc_new = new Unit(npc_new.Name, npc_new.HP - 2, npc_new.Mana, npc_new.Items, npc_new.Effects);

            RpgFightState new_state = new RpgFightState(player_new, npc_new, !node.State.IsPlayerTurn);

            return new Node<RpgFightState>(new_state, node, this, node.CostSoFar + Cost);
        }
    }

    class Shield : Operator<RpgFightState>
    {
        public Shield()
            : base(113)
        {
        }

        public override Node<RpgFightState> Apply(Node<RpgFightState> node)
        {
            Unit player_new = new Unit(node.State.Player);
            Unit npc_new = new Unit(node.State.Npc);

            player_new = node.State.Player.Effects.Aggregate(player_new, (current, effect) => effect.Apply(current));
            npc_new = node.State.Npc.Effects.Aggregate(npc_new, (current, effect) => effect.Apply(current));

            List<Effect> new_player_effects = new List<Effect>(player_new.Effects)
            {
                new ShieldEffect()
            };

            player_new = new Unit(player_new.Name, player_new.HP, player_new.Mana - Cost, player_new.Items, new_player_effects, false, true);

            RpgFightState new_state = new RpgFightState(player_new, npc_new, !node.State.IsPlayerTurn);

            return new Node<RpgFightState>(new_state, node, this, node.CostSoFar + Cost);
        }
    }

    class Poison : Operator<RpgFightState>
    {
        public Poison()
            : base(173)
        {
        }

        public override Node<RpgFightState> Apply(Node<RpgFightState> node)
        {
            Unit player_new = new Unit(node.State.Player);
            Unit npc_new = new Unit(node.State.Npc);

            player_new = node.State.Player.Effects.Aggregate(player_new, (current, effect) => effect.Apply(current));
            npc_new = node.State.Npc.Effects.Aggregate(npc_new, (current, effect) => effect.Apply(current));

            List<Effect> boss_effects = new List<Effect>(npc_new.Effects)
            {
                new PoisonEffect()
            };

            player_new = new Unit(player_new.Name, player_new.HP, player_new.Mana - Cost, player_new.Items, player_new.Effects, false, true);
            npc_new = new Unit(npc_new.Name, npc_new.HP, npc_new.Mana, npc_new.Items, boss_effects);


            RpgFightState new_state = new RpgFightState(player_new, npc_new, !node.State.IsPlayerTurn);

            return new Node<RpgFightState>(new_state, node, this, node.CostSoFar + Cost);
        }
    }

    class Recharge : Operator<RpgFightState>
    {
        public Recharge()
            : base(229)
        {
        }

        public override Node<RpgFightState> Apply(Node<RpgFightState> node)
        {
            Unit player_new = new Unit(node.State.Player);
            Unit npc_new = new Unit(node.State.Npc);

            player_new = node.State.Player.Effects.Aggregate(player_new, (current, effect) => effect.Apply(current));
            npc_new = node.State.Npc.Effects.Aggregate(npc_new, (current, effect) => effect.Apply(current));

            List<Effect> new_player_effects = new List<Effect>(player_new.Effects)
            {
                new RechargeEffect()
            };

            player_new = new Unit(player_new.Name, player_new.HP, player_new.Mana - Cost, player_new.Items, new_player_effects, false, true);

            RpgFightState new_state = new RpgFightState(player_new, npc_new, !node.State.IsPlayerTurn);

            return new Node<RpgFightState>(new_state, node, this, node.CostSoFar + Cost);
        }
    }

    class RpgProblem : Problem<RpgFightState>
    {
        public RpgProblem(Node<RpgFightState> startNode)
            : base(startNode)
        {
        }

        public override List<Operator<RpgFightState>> Available_Operators(Node<RpgFightState> node)
        {
            if (node.State.IsPlayerTurn)
            {
                var ret = new List<Operator<RpgFightState>> {new Drain(), new MagicMissile()};

                if (!node.State.Player.Effects.Select(effect => effect.GetType() == typeof(Recharge)).Contains(true))
                    ret.Add(new Recharge());
                if (!node.State.Player.Effects.Select(effect => effect.GetType() == typeof(Shield)).Contains(true))
                    ret.Add(new Shield());
                if (!node.State.Npc.Effects.Select(effect => effect.GetType() == typeof(Recharge)).Contains(true))
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

    class Day22MageRpg : ISolvable
    {


        public string Solve(string[] inputLines, bool isPart2)
        {
            Unit boss = new Unit("boss", int.Parse(inputLines[0].Split().Last()), 0, new[] { new Item("boss_weapon", 0, int.Parse(inputLines[1].Split().Last()), 0) }, null);
            Unit player = new Unit("player", 10, 250, null, null, false, true);

            var solver = new OptimalTreeSearchSolver<RpgFightState>(new RpgProblem(new Node<RpgFightState>(new RpgFightState(player, boss, true), null, null, 0)));
            if (!solver.Solve())
                throw new IHaveABadFeelingAboutThisException("There should definitely be a solution");

            return solver.Solution.CostSoFar.ToString();
        }
    }
}
