﻿using Scripts.Game.Defined.Serialized.Buffs;
using Scripts.Game.Defined.Serialized.Spells;
using Scripts.Game.Defined.Unserialized.Spells;
using Scripts.Model.Characters;
using Scripts.Model.Spells;
using Scripts.Model.Stats;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Scripts.Game.Serialized.Brains {

    public class Swarm : PriorityBrain {
        private static readonly Attack ATTACK = new Attack();
        private static readonly EnemyHeal HEAL = new EnemyHeal();

        private static bool hasSaidIntroduction;
        private static bool hasSaidSwarmDeath;
        private static bool hasSaidDamagedMessage;

        private int initialEnemiesPresent;

        protected override IList<Spell> GetPriorityPlays() {
            return new Spell[] {
                    CastOnTargetMeetingCondition(HEAL, c => c != this.brainOwner && c.Stats.GetMissingStatCount(StatType.HEALTH) > 0),
                    CastOnLeastTarget(ATTACK, SortByLowestHealth())
                };
        }



        public override string StartOfRoundDialogue() {
            if (currentBattle.TurnCount == 0 && !hasSaidIntroduction) {
                initialEnemiesPresent = allies.Count;
                hasSaidIntroduction = true;
                return Util.PickRandom("We have strength in numbers!/We are many, you are few./You cannnot possibly defeat us together./For the swarm!/In unity we have strength!");
            }
            if (allies.Where(c => c.Brain is Swarm).Count() < initialEnemiesPresent && !hasSaidDamagedMessage && !hasSaidSwarmDeath) {
                hasSaidSwarmDeath = true;
                hasSaidDamagedMessage = true;
                return Util.PickRandom("Our defenses are cracking!/Our perfect unity is ruined./We will never be whole again.");
            }
            return string.Empty;
        }
    }

    public class BlackShuck : PriorityBrain {
        public static readonly SetupCounter COUNTER = new SetupCounter();
        public static readonly Attack ATTACK = new Attack();

        protected override IList<Spell> GetPriorityPlays() {
            return new Spell[] {
                    CastOnRandom(COUNTER),
                    CastOnRandom(ATTACK)
                };
        }

        public override string StartOfRoundDialogue() {
            if (currentBattle.TurnCount == 0) {
                return "(It barks menacingly at you.)";
            }
            return string.Empty;
        }
    }

    public class Siren : BasicBrain {

        public static readonly SpellBook[] DEBUFF_LIST = new SpellBook[] {
            new SingStrengthSong(),
            new SingAgilitySong(),
            new SingIntellectSong(),
            new SingVitalitySong()
        };

        protected override Spell GetSpell() {
            return CastOnRandom(DEBUFF_LIST.ChooseRandom());
        }
    }

    public class DreadSinger : PriorityBrain {

        protected override IList<Spell> GetPriorityPlays() {
            return new Spell[] {
                CastOnRandom(new CastDelayedEternalDeath()),
                CastOnRandom(new CastDelayedDeath()),
                CastOnRandom(new Attack())
            };
        }
    }

    public class Kraken : PriorityBrain {
        private const int TURNS_BETWEEN_TENTACLE_SUMMONS = 8;
        private const float LOW_HEALTH_PERCENTAGE = 0.50f;

        protected override IList<Spell> GetPriorityPlays() {
            return new Spell[] {
                CastOnRandom(new SpawnTentacles(), () => (currentBattle.TurnCount % TURNS_BETWEEN_TENTACLE_SUMMONS) == 0),
                CastOnRandom(new CrushingBlow(), () => (brainOwner.Stats.GetStatPercent(StatType.HEALTH) < LOW_HEALTH_PERCENTAGE)),
                CastOnRandom(new Attack())
            };
        }
    }

    public class Elemental : PriorityBrain {

        protected override IList<Spell> GetPriorityPlays() {
            return new Spell[] {
                CastOnRandom(new WaterboltMulti()),
                CastOnLeastTarget(new WaterboltSingle(), target => target.Stats.GetStatCount(Stats.Get.MOD, StatType.HEALTH))
            };
        }
    }

    public class SharkPirate : BasicBrain {
        private const float PHASE_TWO_HEALTH_PERCENTAGE = 0.50f;
        private const int TURNS_BETWEEN_SUMMONS = 10;
        private const int TURNS_BETWEEN_PHASE_TWO_SPECIAL_SPELLS = 5;
        private static readonly SpellBook SUMMONING = new SummonSeaCreatures();
        private static readonly SpellBook ONE_SHOT_KILL = new OneShotKill();
        private static readonly SpellBook DEATH_CURSE = new CastDelayedDeath();
        private static readonly SpellBook SELF_STRENGTH_BUFF = new GiveOverwhelmingPower();
        private static readonly SpellBook ATTACK = new Attack();

        private static readonly SpellBook[] PHASE_TWO_SPELLS = new SpellBook[] {
            ONE_SHOT_KILL,
            DEATH_CURSE,
            SELF_STRENGTH_BUFF
        };

        private bool isAnnounceSecondPhase;
        private bool hasSummonedWithoutGivingImmunity;
        private int lastTurnCastSpecialSpellInPhaseTwo;

        private bool IsFirstPhase {
            get {
                return brainOwner.Stats.GetStatPercent(StatType.HEALTH) > PHASE_TWO_HEALTH_PERCENTAGE;
            }
        }

        public override string StartOfRoundDialogue() {
            if (currentBattle.TurnCount == 0) {
                return Util.PickRandom("Creatures of the deep, assemble!/Warriors of the sea, assemble!/Ocean creatures, I call for your aid!/Water brethren, I summon you!");
            }
            if (!IsFirstPhase && !isAnnounceSecondPhase) {
                isAnnounceSecondPhase = true;
                return Util.PickRandom("Enough of this! I'll take you down by myself!/You landlubbers are tougher than I thought. I'll have to improvise!");
            }
            return string.Empty;
        }

        protected override Spell GetSpell() {
            Spell chosenSpell = null;
            if (IsFirstPhase) { // phase 1
                if (currentBattle.TurnCount % TURNS_BETWEEN_SUMMONS == 0) {
                    chosenSpell = CastOnRandom(SUMMONING);
                }
            } else { // phase 2
                if (currentBattle.TurnCount - lastTurnCastSpecialSpellInPhaseTwo >= TURNS_BETWEEN_PHASE_TWO_SPECIAL_SPELLS) {
                    lastTurnCastSpecialSpellInPhaseTwo = currentBattle.TurnCount;
                    while (chosenSpell == null) { // Guarentee a spell picked,
                        chosenSpell = CastOnRandom(PHASE_TWO_SPELLS.ChooseRandom());
                    }
                }
            }
            return chosenSpell ?? CastOnRandom(ATTACK);
        }
    }
}