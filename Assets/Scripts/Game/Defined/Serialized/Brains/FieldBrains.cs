using Scripts.Game.Defined.Serialized.Spells;
using Scripts.Model.Characters;
using Scripts.Model.Spells;
using Scripts.Model.Stats;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Scripts.Game.Serialized.Brains {

    public class Attacker : PriorityBrain {
        public static readonly Attack ATTACK = new Attack();

        protected override IList<Func<IPlayable>> SetupPriorityPlays() {
            return new Func<IPlayable>[] {
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

    public class BigKnight : PriorityBrain {
        public static readonly SetupCounter COUNTER = new SetupCounter();
        public static readonly Attack ATTACK = new Attack();

        protected override IList<Func<IPlayable>> SetupPriorityPlays() {
            return new Func<IPlayable>[] {
                    CastOnRandom(COUNTER),
                    CastOnRandom(ATTACK)
                };
        }
    }

    public class Illusionist : PriorityBrain {
        public static readonly Blackout BLACKOUT = new Blackout();

        protected override IList<Func<IPlayable>> SetupPriorityPlays() {
            return new Func<IPlayable>[] {
                    CastOnRandom(BLACKOUT)
                };
        }
    }

    public class Replicant : BasicBrain {
        private const int TURNS_BETWEEN_CLONES = 5;
        private const int LOW_HEALTH = 10;
        public static readonly SpellBook ATTACK = new Attack();
        public static readonly SpellBook CLONE = new ReflectiveClone();
        private bool commentedOnDeadHealers;
        private bool commentedOnLowHealth;
        private int lastTurnCloned;

        private bool IsAnyHealersAlive {
            get {
                return currentBattle.GetAllies(brainOwner).Count(c => c.Brain is Healer) > 0;
            }
        }

        private bool ShouldCastClone {
            get {
                return CloneCount == 0 && (((currentBattle.TurnCount - lastTurnCloned) >= TURNS_BETWEEN_CLONES) || IsLowHealth) && !IsAnyHealersAlive;
            }
        }

        private bool IsLowHealth {
            get {
                return brainOwner.Stats.GetStatCount(Stats.Get.MOD, StatType.HEALTH) <= LOW_HEALTH;
            }
        }

        private int CloneCount {
            get {
                return currentBattle.GetAllies(brainOwner).Count(c => c.HasFlag(Model.Characters.Flag.IS_CLONE));
            }
        }

        protected override IPlayable GetPlay() {
            // Phase 1: Attacks only while healers are up
            if (IsAnyHealersAlive) {
                return CastOnRandom(ATTACK);
            } else { // Phase 2: Start using clone
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
            if (cloneCount > 0 && spell.Book.SpellType == SpellType.OFFENSE && spell.Result.Type.IsSuccessfulType && brainOwner.Stats.State == State.ALIVE) {
                return Util.PickRandom("A little observant, huh?/You will not land another./Nothing but luck./How did you...?/");
            }
            return string.Empty;
        }

        public override string StartOfRoundDialogue() {
            if (!commentedOnDeadHealers && !IsAnyHealersAlive) {
                commentedOnDeadHealers = true;
                return Util.PickRandom("Killing those who are already dead... How unimpressive./They would have only gotten in the way.");
            }
            if (!commentedOnLowHealth && IsLowHealth) {
                commentedOnLowHealth = true;
                return Util.PickRandom("How.../Am I holding back...?/...");
            }
            return string.Empty;
        }
    }

    public class ReplicantClone : BasicBrain {
        public static readonly Attack ATTACK = new Attack();

        private int CloneCount {
            get {
                return currentBattle.GetAllies(brainOwner).Count(c => c.HasFlag(Model.Characters.Flag.IS_CLONE));
            }
        }

        protected override IPlayable GetPlay() {
            return this.CastOnRandom(ATTACK);
        }

        public override string ReactToSpell(Spell spell) {
            if (spell.Book.SpellType == SpellType.OFFENSE && spell.Result.Type.IsSuccessfulType) {
                return Util.PickRandom("You destroy but a phantom./Merely a simulacrum of myself./I am but a clone./A replication of my true self./Just a repetition.");
            }
            return string.Empty;
        }
    }
}