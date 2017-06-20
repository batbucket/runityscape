using Scripts.Model.Interfaces;
using Scripts.Model.SaveLoad.SaveObjects;
using Scripts.Model.Spells;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scripts.Model.Stats;
using Scripts.Model.SaveLoad;

namespace Scripts.Model.Characters {
    public class SpellBooks : IEnumerable<ISpellable>, ISaveable<CharacterSpellBooksSave> {
        private readonly HashSet<SpellBook> set;

        public SpellBooks() {
            set = new HashSet<SpellBook>();
        }

        public int HighestSkillCost {
            get {
                return set.Select(s => s.Costs[StatType.SKILL]).Max();
            }
        }

        public int Count {
            get {
                return set.Count;
            }
        }

        public void AddSpellBook(SpellBook s) {
            set.Add(s);
        }

        public bool HasSpellBook(SpellBook s) {
            return set.Contains(s);
        }

        public Spell CreateSpell(SpellBook spell, Character caster, Character target) {
            return spell.BuildSpell(caster, target);
        }

        IEnumerator<ISpellable> IEnumerable<ISpellable>.GetEnumerator() {
            return set.Cast<ISpellable>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return set.GetEnumerator();
        }

        public CharacterSpellBooksSave GetSaveObject() {
            List<SpellbookSave> books = new List<SpellbookSave>();
            foreach (SpellBook s in this) {
                books.Add(new SpellbookSave(s.GetType()));
            }
            return new CharacterSpellBooksSave(books.ToArray());
        }

        public void InitFromSaveObject(CharacterSpellBooksSave saveObject) {
            foreach (SpellbookSave save in saveObject.Books) {
                SpellBook sb = save.ObjectFromID();
                set.Add(sb);
            }
        }
    }
}