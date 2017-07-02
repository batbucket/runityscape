using Scripts.Game.Defined.Serialized.Spells;
using Scripts.Model.Characters;

namespace Scripts.Game.Defined.Characters.StartingSpells {
    public class HeroSpells : SpellBooks {
        public HeroSpells() { }
    }
    public class DebugSpells : SpellBooks {
        public DebugSpells() {
            AddSpellBook(new InflictPoison());
        }
    }
}