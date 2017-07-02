using Scripts.Game.Serialized;
using Scripts.Model.Characters;
using Scripts.Model.Pages;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Game.Pages {
    public abstract class ExplorableArea {
        public readonly Explore Type;

        protected Flags flags;
        private Page camp;
        private Party party;
        private string location;
        private PageGenerator generator;

        public ExplorableArea(Explore type, Page camp, Flags flags, Party party, string location, Sprite sprite, string description) {
            this.Type = type;
            this.camp = camp;
            this.flags = flags;
            this.party = party;
            this.location = location;
            this.generator = new PageGenerator(location, sprite, description);
            SetupEncounters();
        }

        public PageGenerator Generator {
            get {
                return generator;
            }
        }

        protected abstract IEnumerable<Encounter> GetEncounters();

        private void SetupEncounters() {
            foreach (Encounter e in GetEncounters()) {
                generator.AddEncounter(e);
            }
        }

        protected Encounter RandomBattle(Rarity rarity, params Character[] enemies) {
            return new Encounter(rarity, GetPageFunc(enemies));
        }

        private Func<Page> GetPageFunc(IEnumerable<Character> enemies) {
            return () =>
            new Battle
                (camp,
                camp,
                Music.NORMAL,
                location,
                party.Collection,
                enemies);
        }
    }
}