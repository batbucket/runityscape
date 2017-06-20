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
}