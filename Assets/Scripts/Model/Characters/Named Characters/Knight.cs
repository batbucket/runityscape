using System;
using UnityEngine;

namespace Scripts.Model.Characters.Named {

    public class Knight : ComputerCharacter {

        public Knight()
            : base(new Displays { Loc = "gladius", Name = "???", Color = Color.white, Check = "Yourself." },
                  new StartStats { Lvl = 1, Str = 1, Int = 1, Agi = 1, Vit = 1 }
                  ) { }

        protected override void DecideSpell() {
        }
    }
}