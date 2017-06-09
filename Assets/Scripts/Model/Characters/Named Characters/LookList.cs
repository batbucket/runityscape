using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Model.Characters {
    public static class LookList {
        public class Kitsune : Look {
            public Kitsune() : base() {
                this.Name = "Kitsune";
                this.Sprite = Util.GetSprite("fox-head");
                this.Check = "Humanoid fox creature. Those tails don't look very friendly...";
                this.TextColor = Color.magenta;
            }
        }
    }
}