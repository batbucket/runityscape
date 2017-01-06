using Scripts.Model.Characters;
using Scripts.Model.Stats.Resources;
using Scripts.View.Effects;
using UnityEngine;

namespace Scripts.Model.Spells.Named {

    public class Regenerate : SpellFactory {

        public Regenerate() : base("Regenerate", "Recover health over time.", SpellType.DEFENSE, TargetType.SELF, abbreviation: "REG", color: Color.green, isSelfTargetable: true) {
        }

        public override Hit CreateHit() {
            return new Hit(
                isState: (c, t) => true,
                isIndefinite: (c, t) => t.State == CharacterState.NORMAL,
                timePerTick: (c, t) => 0f,
                perform: (c, t, calc) => t.AddToResource(ResourceType.HEALTH, false, 0.05f),
                createText: (c, t, calc) => string.Format("{0} is regenerating <color=lime>life</color>.", c.DisplayName),
                sfx: (c, t, calc) => new CharacterEffect[0]
                );
        }
    }
}