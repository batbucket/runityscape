using Scripts.Game.Defined.Characters;
using Scripts.Model.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Game.Defined.Characters {
    public static class Debug {
        public static Look NotKitsune() {
            return new Look(
                "Kitsune",
                "fox-head",
                "Humanoid fox creature. Those tails don't look very friendly...",
                "undfeined lol",
                Breed.MONSTER,
                Color.magenta
                );
        }
    }

    public static class RuinsLooks {
        public static Look Hero(string name) {
            return new Look(
                name,
                "person",
                "It's you!",
                "Did you really just check yourself?",
                Breed.HUMAN
                );
        }

        public static Look Villager() {
            return new Look(
                "F. Villager",
                "haunting",
                "The spirit of some fallen villager.",
                "The easiest enemy. Can only do 0-1 damage.",
                Breed.SPIRIT
                );
        }

        public static Look Knight() {
            return new Look(
                "F. Knight",
                "spectre",
                "The spirit of some fallen knight.",
                "During its counter, avoid using Attack.",
                Breed.SPIRIT
                );
        }

        public static Look Healer() {
            return new Look(
                "F. Healer",
                "spectre",
                "The spirit of some fallen healer.",
                "Will heal injured targets. Should be attacked first in most cases.",
                Breed.SPIRIT
                );
        }
    }
}

namespace Scripts.Game.Undefined.Characters {
    public class DummyLook : Look {
        public DummyLook(Breed breed, string name, Sprite sprite, string tip) : base() {
            this.Name = name;
            this.Sprite = sprite;
            this.tooltip = tip;
            this.textColor = Color.white;
            this.breed = breed;
        }
    }
}