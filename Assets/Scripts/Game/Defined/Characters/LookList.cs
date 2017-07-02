using Scripts.Game.Defined.Characters;
using Scripts.Model.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Game.Defined.Characters {
    public class KitsuneLook : Look {
        public KitsuneLook() : base() {
            this.Name = "Kitsune";
            this.Sprite = Util.GetSprite("fox-head");
            this.tooltip = "Humanoid fox creature. Those tails don't look very friendly...";
            this.textColor = Color.magenta;
        }
    }

    public class HeroLook : Look {
        public HeroLook(string name) : base() {
            this.Name = name;
            this.Sprite = Util.GetSprite("person");
            this.tooltip = string.Empty;
            this.textColor = Color.white;
        }
    }

    public static class Ruins {
        class Villager : Look {
            public Villager() : base() {
                this.Name = "Fallen Villager";
                this.Sprite = Util.GetSprite("");
                this.tooltip = "The spirit of some passed villager. It still defends the ruin.";
            }
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