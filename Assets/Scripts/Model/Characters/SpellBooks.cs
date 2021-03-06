﻿using Scripts.Model.Interfaces;
using Scripts.Model.SaveLoad.SaveObjects;
using Scripts.Model.Spells;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scripts.Model.Stats;
using Scripts.Model.SaveLoad;
using System;
using Scripts.Game.Defined.Serialized.Spells;

namespace Scripts.Model.Characters {
    public class SpellBooks : IEnumerable<ISpellable>, IEnumerable<SpellBook>, ISaveable<CharacterSpellBooksSave> {
        private static readonly SpellBook[] DEFAULT_SPELLS = new SpellBook[] { new Attack(), new Wait() };

        private readonly HashSet<SpellBook> set;

        public SpellBooks() {
            set = new HashSet<SpellBook>();
            foreach (SpellBook sb in DEFAULT_SPELLS) {
                set.Add(sb);
            }
        }

        public SpellBooks(params SpellBook[] initialSpells) : this() {
            foreach (SpellBook sb in initialSpells) {
                set.Add(sb);
            }
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
            Util.Assert(s.HasFlag(Spells.Flag.CASTER_REQUIRES_SPELL), "Uncastable spell.");
            set.Add(s);
        }

        public bool HasSpellBook(SpellBook s) {
            return set.Contains(s);
        }

        public Spell CreateSpell(SpellBook spell, SpellParams caster, SpellParams target) {
            return spell.BuildSpell(caster, target);
        }

        IEnumerator<ISpellable> IEnumerable<ISpellable>.GetEnumerator() {
            return set.Cast<ISpellable>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return set.GetEnumerator();
        }

        public override bool Equals(object obj) {
            var item = obj as SpellBooks;

            if (item == null) {
                return false;
            }

            return this.set.SetEquals(item.set);
        }

        public override int GetHashCode() {
            return 0;
        }

        public CharacterSpellBooksSave GetSaveObject() {
            List<SpellBookSave> books = new List<SpellBookSave>();
            IEnumerable<SpellBook> enumer = this;
            foreach (SpellBook s in enumer) {
                books.Add(new SpellBookSave(s.GetType()));
            }
            return new CharacterSpellBooksSave(books);
        }

        public void InitFromSaveObject(CharacterSpellBooksSave saveObject) {
            foreach (SpellBookSave save in saveObject.Books) {
                SpellBook sb = save.CreateObjectFromID();
                set.Add(sb);
            }
        }

        IEnumerator<SpellBook> IEnumerable<SpellBook>.GetEnumerator() {
            return set.GetEnumerator();
        }
    }
}