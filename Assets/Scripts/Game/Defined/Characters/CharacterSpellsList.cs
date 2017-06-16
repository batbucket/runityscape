using Scripts.Game.Defined.Spells;
using Scripts.Model.Characters;

namespace Scripts.Game.Defined.Characters.StartingSpells {
    public class DebugSpells : SpellBooks {
        public DebugSpells() {
            AddSpellBook(new Attack());
            AddSpellBook(new InflictPoison());
            AddSpellBook(new Wait());
        }
    }
}