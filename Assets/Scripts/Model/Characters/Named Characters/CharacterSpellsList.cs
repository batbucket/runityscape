using Scripts.Model.Characters;
using Scripts.Model.Spells;

public static class CharacterSpellsList {
    public class DebugSpells : Spells {
        public DebugSpells() {
            AddSpellBook(new SpellList.Attack());
            AddSpellBook(new SpellList.Poison());
            AddSpellBook(new SpellList.Wait());
        }
    }
}