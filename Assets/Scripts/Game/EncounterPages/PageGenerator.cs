using Scripts.Game.Serialized;
using Scripts.Model.Interfaces;
using Scripts.Model.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.Game.Pages {

    public class PageGenerator {

        private int totalWeight;
        private List<Encounter> encounters;

        private string name;
        private Sprite sprite;
        private string description;

        public PageGenerator(string name, Sprite sprite, string description) {
            this.encounters = new List<Encounter>();
            this.name = name;
            this.sprite = sprite;
            this.description = description;
        }

        public string Name {
            get {
                return name;
            }
        }

        public Sprite Sprite {
            get {
                return sprite;
            }
        }

        public string Description {
            get {
                return description;
            }
        }

        public Page GetRandomEncounter(Flags flags) {

            Encounter chosen = null;

            // Override check
            chosen = encounters.Find(e => e.IsOverride(flags));
            if (chosen != null) {
                return chosen.GeneratePage();
            }

            int totalWeight = encounters.Where(e => e.CanOccur(flags)).Sum(e => e.Weight);
            int random = UnityEngine.Random.Range(0, totalWeight);

            // Loop stops when chosenEncounter is set to the correct Page function
            int sum = 0;
            for (int i = 0; i < encounters.Count; i++) {
                Encounter encounter = encounters[i];

                if (encounter.CanOccur(flags)) {
                    sum += encounter.Weight;
                }
                if (random < sum) {
                    return encounter.GeneratePage();
                }
            }

            // This should never happen
            Util.Assert(false, "Unable to find an encounter.");
            return null;
        }

        public void AddEncounter(Encounter encounter) {
            encounters.Add(encounter);
        }
    }
}