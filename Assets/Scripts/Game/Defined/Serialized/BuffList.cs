using Scripts.Game.Defined.Spells;
using Scripts.Model.Buffs;
using Scripts.Model.Characters;
using Scripts.Model.Spells;
using Scripts.Model.Stats;
using System.Collections.Generic;

namespace Scripts.Game.Defined.Serialized.Spells {
    public class Poison : Buff {
        public Poison() : base(2, Util.GetSprite("fox-head"), "Poisoned", "Loses health at the end of each turn.", true) { }

        protected override IList<SpellEffect> OnEndOfTurnHelper(Model.Characters.Stats owner) {
            return new SpellEffect[] {
                new AddToModStat(owner, StatType.HEALTH, -1)
            };
        }
    }

    public class Checked : Buff {
        public Checked() : base(Util.GetSprite("magnifying-glass"), "Checked", "Resource visibility increased.", true) { }

        protected override IList<SpellEffect> OnApplyHelper(Stats owner) {
            return new SpellEffect[] { new AddToResourceVisibility(owner, 1) };
        }

        protected override IList<SpellEffect> OnTimeOutHelper(Stats owner) {
            return new SpellEffect[] { new AddToResourceVisibility(owner, -1) };
        }
    }
}