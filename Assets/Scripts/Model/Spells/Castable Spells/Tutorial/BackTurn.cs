using UnityEngine;
using System.Linq;

public class BackTurn : SpellFactory {
    public BackTurn() : base("BackTurn", "Turn your back to an enemy.", SpellType.DEFENSE, TargetType.SELF, abbreviation: "BCK", color: Color.red) { }

    public override Hit CreateHit() {
        return new Hit(
            isState: (c, t, o) => true,
            duration: (c, t, o) => 6,
            react: (s) => {
                if (s.SpellFactory is Attack) {
                    (new TailSpike()).Cast(s.Target, s.Caster, s);
                }
            },
            onStart: (c, t, o) => c.IsCharging = false,
            onEnd: (c, t, o) => c.IsCharging = true,
            perform: (c, t, calc, o) => {

            },
            createText: (c, t, calc, o) => string.Format("{0} turns their back to their foes!", c)
            );
    }
}