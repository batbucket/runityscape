using System;
using System.Collections.Generic;
using Scripts.Model.Interfaces;
using Scripts.Model.Pages;
using Scripts.Model.Processes;
using Scripts.Model.Spells;

namespace Scripts.Model.Characters {
    public static class BrainList {
        public static readonly SpellList.Attack Attack = new SpellList.Attack();

        public class Player : Brain {
            private Battle battle;

            public override void DetermineAction(Action<IPlayable> addPlay) {
                Grid main = new Grid("Main");
                main.Array = Util.GetArray(
                    new Tuple(0, GenerateTargets(main, Attack, addPlay))
                    );
                battle.Actions = main.Array;
            }

            protected override void PageSetupHelper(Battle battle) {
                this.battle = battle;
            }

            private Grid GenerateTargets(Grid previous, SpellBook sb, Action<IPlayable> addFunc) {
                Grid grid = new Grid(sb.Name);
                grid.Tooltip = Attack.CreateDescription(Owner.Stats);
                ICollection<Character> targets = sb.TargetType.GetTargets(Owner, battle);
                IButtonable[] buttons = new IButtonable[targets.Count + 1];
                buttons[0] = previous;

                int index = 1;
                foreach (Character myTarget in targets) {
                    Character target = myTarget;
                    buttons[index++] =
                        new Process(target.Look.DisplayName,
                                    string.Format("{0} will target {1} with Attack.\n", Owner.Look.DisplayName, target.Look.DisplayName, Attack.CreateDescription(Owner.Stats)),
                                    () => {
                                        addFunc(Spells.CreateSpell(Attack, this.Owner, target));

                                        //previous.Invoke();
                                    },
                                    () => Attack.IsCastable(Spells, Owner.Stats, target.Stats));
                }
                grid.Array = buttons;
                return grid;
            }
        }

        public class DebugAI : Brain {
            public override void DetermineAction(Action<IPlayable> addPlay) {
                addPlay(Spells.CreateSpell(Attack, Owner, foes.PickRandom()));
            }
        }
    }
}