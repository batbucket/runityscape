using Scripts.Model.Stats.Resources;
using Scripts.View.Effects;
using UnityEngine;

namespace Scripts.Model.Spells.Named {

    public class Defeated : SpellFactory {

        public Defeated() : base("Defeated", "This unit has been defeated. Additional damage will execute, regardless of Life.", SpellType.OTHER, TargetType.SELF, abbreviation: "DFT", color: Color.red) {
        }

        public override Hit CreateHit() {
            return new Hit(
                isState: (c, t, o) => true,
                isIndefinite: (c, t, o) => true,
                onStart: (c, t, o) => t.IsCharging = false,
                react: (s) => {
                    if (s.Calculation.TargetResources[ResourceType.HEALTH].False < 0) {
                        s.Target.OnKill();
                    }
                },
                onEnd: (c, t, o) => t.IsCharging = true,
                sfx: (c, t, calc, o) => new CharacterEffect[] { new DefeatEffect(t.Presenter.PortraitView) }
                );
        }
    }
}