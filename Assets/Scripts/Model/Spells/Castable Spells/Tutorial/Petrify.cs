using UnityEngine;

namespace Scripts.Model.Spells.Named {

    public class Petrify : SpellFactory {

        public Petrify() : base("Petrify", "", SpellType.OFFENSE, TargetType.SINGLE_ENEMY, abbreviation: "PFY", color: Color.magenta, isSelfTargetable: true) {
        }

        public override Hit CreateHit() {
            return new Hit(
                isState: (c, t) => true,
                isIndefinite: (c, t) => true,
                onStart:
                (c, t) => t.IsCharging = false,
                onEnd:
                (c, t) => t.IsCharging = true
                );
        }
    }
}