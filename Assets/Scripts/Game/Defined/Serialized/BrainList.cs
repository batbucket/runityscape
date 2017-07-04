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
                PageUtil.GenerateTargets(battle, main, owner, Attack, handlePlay),
                PageUtil.GenerateSpellBooks(battle, main, owner, Attack, handlePlay),
                PageUtil.GenerateItems(battle, main, owner, handlePlay),
                PageUtil.GenerateEquipmentGrid(main, owner, handlePlay)
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

    public static class RuinsBrains {
        public class Villager : PriorityBrain {
            public static readonly Attack Attack = new Attack();

            protected override IList<Func<bool>> SetupPriorityActions() {
                return new Func<bool>[] {
                CastOnRandom(Attack)
            };
            }
        }

        public class Knight : PriorityBrain {
            public static readonly Attack Attack = new Attack();
            public static readonly SetupCounter Counter = new SetupCounter();

            protected override IList<Func<bool>> SetupPriorityActions() {
                return new Func<bool>[] {
                CastOnRandom(Counter),
                CastOnRandom(Attack)
            };
            }
        }
        public class Healer : PriorityBrain {
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
}