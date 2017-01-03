using Scripts.Model.Stats;
using Scripts.Model.Stats.Resources;
using System.Collections.Generic;

namespace Scripts.Model.Spells.Named {

    public class TailSpike : SpellFactory {
        private const int DAMAGE = 10;
        private const float VARIANCE = .125f;

        public TailSpike() : base("TailSpike", "", SpellType.OFFENSE, TargetType.SINGLE_ENEMY) {
        }

        public override Hit CreateHit() {
            return new Hit(
                isState: (c, t, o) => true,
                calculation: (c, t, o) =>
                    new Calculation(targetResources: new Dictionary<ResourceType, PairedValue>() {
                    { ResourceType.HEALTH, new PairedValue(0, -Util.Random(DAMAGE, VARIANCE)) }
                    }),
                perform: (c, t, calc, o) => {
                    Result.NumericPerform(c, t, calc);
                    o.Calculation = new Calculation();
                    o.Result = o.Miss;
                    o.Result.CreateText = (c2, t2, calc2, o2) => "";
                },
                createText: (c, t, calc, o) =>
                string.Format(
                    "{0} attacks into {1}'s tails! The tails extend out, slicing into {0}!\n{0} took <color=red>{2}</color> counter damage!",
                    t.DisplayName,
                    c.DisplayName,
                    -calc.TargetResources[ResourceType.HEALTH].False),
                sound: (c, t, calc, o) => "Slash_0"
                );
        }
    }
}