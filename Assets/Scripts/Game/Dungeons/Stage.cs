using Scripts.Model.Pages;
using System;

namespace Scripts.Game.Dungeons {
    public class Stage {
        public readonly string StageName;
        public readonly Func<Encounter[]> Encounters;

        public Stage(string stageName, Func<Encounter[]> encounters) {
            this.StageName = stageName;
            this.Encounters = encounters;
        }

        public Stage() {
            this.StageName = "Placeholder";
            this.Encounters = () => new Encounter[] { new Encounter(Music.NORMAL) };
        }
    }
}
