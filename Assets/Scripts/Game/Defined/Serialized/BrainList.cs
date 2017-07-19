using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Collections;
using Scripts.Model.Acts;

namespace Scripts.Game.Defined.Serialized.Characters {

    public class Player : Brain {
        public static readonly Attack ATTACK = new Attack();

        public override void DetermineAction() {
            Grid main = new Grid("Main");
            main.List = new IButtonable[] {
                PageUtil.GenerateTargets(currentBattle, main, owner, ATTACK, GetAttackSprite(owner.Equipment), handlePlay),
                null,
                null,
                null,
                PageUtil.GenerateActions(currentBattle, main, owner, ATTACK, temporarySpells, handlePlay),
                PageUtil.GenerateSpellBooks(currentBattle, main, owner, ATTACK, handlePlay),
                PageUtil.GenerateItemsGrid(currentBattle, main, owner, handlePlay),
                PageUtil.GenerateEquipmentGrid(main, owner, handlePlay),
            };
            currentBattle.Actions = main.List;
        }

        private Sprite GetAttackSprite(Equipment equipment) {
            if (equipment.HasEquip(EquipType.WEAPON)) {
                return equipment.PeekItem(EquipType.WEAPON).Icon;
            } else {
                return Util.GetSprite("fist");
            }
        }
    }

    public class DebugAI : Brain {
        public static readonly Attack ATTACK = new Attack();

        public override void DetermineAction() {
            //addPlay(Spells.CreateSpell(Attack, Owner, foes.PickRandom()));
        }
    }

    public static class RuinsBrains {
        public class Villager : PriorityBrain {
            public static readonly Attack ATTACK = new Attack();

            protected override IList<Func<IPlayable>> SetupPriorityPlays() {
                return new Func<IPlayable>[] {
                CastOnRandom(ATTACK)
            };
            }
        }

        public class Knight : PriorityBrain {
            public static readonly Attack ATTACK = new Attack();
            public static readonly SetupCounter COUNTER = new SetupCounter();

            protected override IList<Func<IPlayable>> SetupPriorityPlays() {
                return new Func<IPlayable>[] {
                CastOnRandom(COUNTER),
                CastOnRandom(ATTACK)
            };
            }
        }

        public class Healer : PriorityBrain {
            public static readonly Attack ATTACK = new Attack();
            public static readonly Heal HEAL = new Heal();

            protected override IList<Func<IPlayable>> SetupPriorityPlays() {
                return new Func<IPlayable>[] {
                    CastOnTargetMeetingCondition(HEAL, c => c.Stats.GetMissingStatCount(StatType.HEALTH) > 0)
                };
            }
        }

        public class Kitsune : BasicBrain {
            private const int TURNS_BETWEEN_CLONES = 4;
            public static readonly SpellBook ATTACK = new Attack();
            public static readonly SpellBook CLONE = new ReflectiveClone();
            private bool commentedOnDeadHealers;
            private int lastTurnCloned;

            private bool IsAnyHealersAlive {
                get {
                    return currentBattle.GetAllies(owner.Character).Count(c => c.Brain is Healer) > 0;
                }
            }

            private bool ShouldCastClone {
                get {
                    return CloneCount == 0 && ((currentBattle.TurnCount - lastTurnCloned) >= TURNS_BETWEEN_CLONES) && !IsAnyHealersAlive;
                }
            }

            private int CloneCount {
                get {
                    return currentBattle.GetAllies(owner.Character).Count(c => c.HasFlag(Model.Characters.Flag.IS_CLONE));
                }
            }

            protected override IPlayable GetPlay() {
                // Phase 1: Attacks only while healers are up
                if (IsAnyHealersAlive) {
                    return CastOnRandom(ATTACK);
                } else { // Phase 2: Start using clone
                    Util.Log("Should I cast clone? " + ShouldCastClone);
                    if (ShouldCastClone) {
                        lastTurnCloned = currentBattle.TurnCount;
                        return CastOnRandom(CLONE);
                    } else {
                        return CastOnRandom(ATTACK);
                    }
                }
            }

            public override string ReactToSpell(Spell spell) {
                int cloneCount = CloneCount;
                if (CloneCount > 0 && spell.Book.SpellType == SpellType.OFFENSE && spell.Result.Type.IsSuccessfulType && owner.Stats.State == State.ALIVE) {
                    if (cloneCount == 4) {
                        return Util.PickRandom("A little observant, huh?/You will not land another./Nothing but luck./How did you...?/A one in five chance.");
                    } else if (cloneCount == 3) {
                        return Util.PickRandom("Learning from your mistakes, huh?/So you've learned from the first clone?/A one in four chance.");
                    } else if (cloneCount == 2) {
                        return Util.PickRandom("A one in three chance./Slow learner, huh?/Took you long enough.");
                    } else { // 1 clone
                        return Util.PickRandom("A one in two chance./A coin flip./Took you long enough./I'm surprised it took you this long to figure out the real one.");
                    }
                }
                return string.Empty;
            }

            public override string StartOfRoundDialogue() {
                if (!commentedOnDeadHealers && !IsAnyHealersAlive) {
                    commentedOnDeadHealers = true;
                    return Util.PickRandom("Know that I will not fall as easily as these spirits, mayfly./Killing those who are already dead... How unimpressive./They would have only gotten in the way.");
                }
                return string.Empty;
            }
        }

        public class KitsuneClone : BasicBrain {
            public static readonly Attack ATTACK = new Attack();

            private int CloneCount {
                get {
                    return currentBattle.GetAllies(owner.Character).Count(c => c.HasFlag(Model.Characters.Flag.IS_CLONE));
                }
            }

            protected override IPlayable GetPlay() {
                return this.CastOnRandom(ATTACK);
            }

            public override string ReactToSpell(Spell spell) {
                if (spell.Book.SpellType == SpellType.OFFENSE && spell.Result.Type.IsSuccessfulType) {
                    if (CloneCount == 1) {
                        return Util.PickRandom("Not very lucky, are you?/How mindless.../Took you long enough./As they say: fifth time is the charm./Now that's just sad./Unfortunate. For you.");
                    } else {
                        return Util.PickRandom("You destroy but a phantom./Merely a simulacrum of myself./I am but a clone./A replication of my true self./A powerless repetition./Just kidding.");
                    }
                }
                return string.Empty;
            }
        }
    }
}