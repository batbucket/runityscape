using Script.Model.Characters.Named;
using Scripts.Model.Spells.Named;
using Scripts.Model.Stats.Attributes;
using Scripts.Model.Stats.Resources;
using UnityEngine;

namespace Scripts.Model.Characters.Named {

    public class Immortal : ComputerCharacter {

        public Immortal()
            : base(
                new Displays {
                    Loc = "tentacles-skull",
                    Name = "Immortal",
                    Color = Color.white,
                    Check = "Unable to be killed."
                },
                new StartStats {
                    Lvl = 3,
                    Str = 1,
                    Int = 1,
                    Agi = 1,
                    Vit = 1
                }
                ) {
            this.Attributes.Remove(AttributeType.VITALITY);
            this.Resources.Remove(ResourceType.HEALTH);
        }

        protected override bool IsDefeated() {
            return false;
        }

        protected override void DecideSpell() {

        }
    }
}