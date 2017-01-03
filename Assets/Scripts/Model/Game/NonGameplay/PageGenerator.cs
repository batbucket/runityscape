using Scripts.Model.Interfaces;
using Scripts.Presenter;
using System.Collections.Generic;

namespace Scripts.Model.World.Utility {

    /// <summary>
    /// Used to generate random encounters when the user
    /// decides to EXPLORE an area.
    /// </summary>
    public class PageGenerator : IButtonable {
        protected IList<Encounter> encounters;

        private readonly int[] flags;

        private string description;

        private string name;

        public PageGenerator(
                             string name,
                             string description,
                             params Encounter[] encounters) {
            this.name = name;
            this.description = description;
            this.encounters = new List<Encounter>();
            this.AddEncounters(encounters);
        }

        public PageGenerator(string name, string description) : this(name, description, new Encounter[0]) {
        }

        public string ButtonText {
            get {
                return name;
            }
        }

        public IList<Encounter> Encounters {
            get {
                return encounters;
            }
        }

        public bool IsInvokable {
            get {
                return true;
            }
        }

        public bool IsVisibleOnDisable {
            get {
                return true;
            }
        }

        public string TooltipText {
            get {
                return description;
            }
        }

        public void AddEncounter(Encounter encounter) {
            this.encounters.Add(encounter);
        }

        public void AddEncounters(params Encounter[] encounters) {
            foreach (Encounter e in encounters) {
                this.encounters.Add(e);
            }
        }

        public void Invoke() {
            IList<Encounter> enabledEncounts = new List<Encounter>();

            // Add enabled encounters into pool
            foreach (Encounter e in encounters) {
                enabledEncounts.Add(e);
            }

            // Sum up weights
            float totalSum = 0;
            foreach (Encounter e in encounters) {
                totalSum += e.weight.Invoke();
            }

            float random = UnityEngine.Random.Range(0, totalSum);

            float cumulSum = 0;
            foreach (Encounter e in encounters) {
                cumulSum += e.weight.Invoke();

                if (random <= cumulSum) {
                    Game.Instance.CurrentPage = e.page.Invoke();
                    return;
                }
            }
            Util.Assert(false, "Unable to generate an encounter!");
        }
    }
}