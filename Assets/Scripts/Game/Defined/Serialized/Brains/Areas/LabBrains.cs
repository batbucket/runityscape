using System;
using System.Collections.Generic;
using Scripts.Model.Characters;
using Scripts.Model.Spells;
using Scripts.Game.Defined.Serialized.Spells;
using Scripts.Game.Defined.Unserialized.Spells;
using System.Linq;
using Scripts.Model.Stats;

namespace Scripts.Game.Serialized.Brains {

    public class LabKnight : PriorityBrain {
        private static readonly SpellBook ATTACK = new Attack();
        private static readonly SpellBook CRUSHING_BLOW = new CrushingBlow();
        private static readonly SpellBook DEFEND = new SetupDefend();

        public override string StartOfRoundDialogue() {
            if (CRUSHING_BLOW.IsMeetPreTargetRequirements(brainOwner.Stats)) {
                return Util.PickRandom("Now it's over!/Taste vengeance!/Power!/Eliminate!");
            }
            return string.Empty;
        }

        protected override IList<Spell> GetPriorityPlays() {
            return new Spell[] {
                this.CastOnRandom(DEFEND, () => currentBattle.TurnCount == 0),
                this.CastOnRandom(CRUSHING_BLOW),
                this.CastOnLeastTarget(ATTACK, SortByLowestHealth())
            };
        }
    }

    public class LabBigKnight : PriorityBrain {
        private static readonly SpellBook ATTACK = new Attack();
        private static readonly SpellBook REVIVE = new UnholyRevival();

        private static readonly SpellBook[] SKILL_SPELLS = new SpellBook[] {
            new CrushingBlow(),
            new SetupDefend()
        };

        private SpellBook chosenSkillSpell;

        private Character DeadPartner {
            get {
                return allies.Where(c => c.Stats.State == State.DEAD).FirstOrDefault();
            }
        }

        public override string StartOfRoundDialogue() {
            if (chosenSkillSpell == null || brainOwner.Stats.GetStatCount(Stats.Get.MOD, StatType.SKILL) == 0) {
                chosenSkillSpell = Util.ChooseRandom(SKILL_SPELLS);
            }
            if (REVIVE.IsCastable(brainOwner, allies)) {
                return string.Format("{0}! It is not yet your time!", DeadPartner.Look.Name);
            }
            return string.Empty;
        }

        protected override IList<Spell> GetPriorityPlays() {
            return new Spell[] {
                this.CastOnRandom(REVIVE),
                this.CastOnRandom(chosenSkillSpell),
                this.CastOnLeastTarget(ATTACK, SortByLowestHealth())
            };
        }
    }
}