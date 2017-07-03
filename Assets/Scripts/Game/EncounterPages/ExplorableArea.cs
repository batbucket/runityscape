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

        public ExplorableArea(
            Explore type,
            Page camp,
            Flags flags,
            Party party,
            string location,
            Sprite sprite,
            string description) {
            this.Type = type;
            this.camp = camp;
            this.flags = flags;
            this.party = party;
            this.location = location;
            this.generator = new PageGenerator(location, sprite, description);
            SetupGeneratorWithEncounters();
        }

        public ExplorableArea(
            Explore type,
            Page camp,
            Flags flags,
            Party party,
            string location,
            string spriteLoc,
            string description)
            : this(
                  type,
                  camp,
                  flags,
                  party,
                  location,
                  Util.GetSprite(spriteLoc),
                  description) { }

        public PageGenerator Generator {
            get {
                return generator;
            }
        }

        protected abstract IEnumerable<Encounter> GetEncounters();

        protected Encounter RandomBattle(Rarity rarity, params Character[] enemies) {
            return new Encounter(rarity, GetPageFunc(enemies));
        }

        private void SetupGeneratorWithEncounters() {
            foreach (Encounter encounter in GetEncounters()) {
                generator.AddEncounter(encounter);
            }
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