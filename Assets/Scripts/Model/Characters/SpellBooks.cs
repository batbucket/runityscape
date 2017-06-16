using Scripts.Model.Spells;
using System.Collections;
using System.Collections.Generic;

namespace Scripts.Model.Characters {
    public class SpellBooks : IEnumerable<SpellBook> {
        public HashSet<SpellBook> Set;

        public SpellBooks() {
            Set = new HashSet<SpellBook>();
        }

        public void AddSpellBook(SpellBook s) {
            Set.Add(s);
        }

        public bool HasSpellBook(SpellBook s) {
            return Set.Contains(s);
        }

        public Spell CreateSpell(SpellBook spell, Character caster, Character target) {
            return spell.BuildSpell(caster, target);
        }

        IEnumerator<SpellBook> IEnumerable<SpellBook>.GetEnumerator() {
            return Set.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return Set.GetEnumerator();
        }
    }
}