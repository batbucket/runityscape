using System;
using System.Collections.Generic;
using Scripts.Model.Interfaces;
using Scripts.Model.Pages;
using Scripts.Model.Processes;
using Scripts.Model.Spells;
using UnityEngine;
using Scripts.Model.Characters;
using Scripts.Game.Defined.Spells;
using Scripts.Model.Items;
using Scripts.Game.Defined.Serialized.Spells;
using Scripts.Model.Stats;

namespace Scripts.Game.Defined.Serialized.Characters {

    public class Player : Brain {
        public static readonly Attack Attack = new Attack();

        public override void DetermineAction() {
            Grid main = new Grid("Main");
            main.List = new IButtonable[] {
                PageUtil.GenerateTargets(battle, main, Owner, Attack, handlePlay),
                PageUtil.GenerateSpellBooks(battle, main, Owner, Attack, handlePlay),
                PageUtil.GenerateItems(battle, main, Owner, handlePlay),
                PageUtil.GenerateEquipmentGrid(main, Owner, handlePlay)
            };
            battle.Actions = main.List;
        }
    }

    public class DebugAI : Brain {
        public static readonly Attack Attack = new Attack();

        public override void DetermineAction() {
            //addPlay(Spells.CreateSpell(Attack, Owner, foes.PickRandom()));
        }
    }

    public class VillagerBrain : PriorityBrain {
        public static readonly Attack Attack = new Attack();

        protected override IList<Func<bool>> SetupPriorityActions() {
            return new Func<bool>[] {
                CastOnRandom(Attack)
            };
        }
    }

    public class KnightBrain : PriorityBrain {
        public static readonly Attack Attack = new Attack();
        public static readonly SetupCounter Counter = new SetupCounter();

        protected override IList<Func<bool>> SetupPriorityActions() {
            return new Func<bool>[] {
                CastOnRandom(Counter),
                CastOnRandom(Attack)
            };
        }
    }

    public class HealerBrain : PriorityBrain {
        public static readonly Attack Attack = new Attack();
        public static readonly Heal Heal = new Heal();

        protected override IList<Func<bool>> SetupPriorityActions() {
            return new Func<bool>[] {
                CastOnTargetMeetingCondition(Heal, c => c.Stats.GetMissingStatCount(StatType.HEALTH) > 0),
                CastOnRandom(Attack)
            };
        }
    }
}