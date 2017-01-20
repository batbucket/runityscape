using Scripts.Model.Stats;
using Scripts.Model.Stats.Resources;
using System.Collections.Generic;

namespace Scripts.Model.Spells.Named {

    public class TailSpike : SpellFactory {
        private const int DAMAGE = 15;
        private const float VARIANCE = .25f;

        public TailSpike() : base("Tail Spike", "", SpellType.OFFENSE, TargetType.SINGLE_ENEMY) {
        }

        public override Hit CreateHit() {
            return new Hit(
                isState: (c, t) => true,
                calculation: (c, t) =>
                    new Calculation(targetResources: new Dictionary<ResourceType, PairedValue>() {
                    { ResourceType.HEALTH, new PairedValue(0, -Util.Random(DAMAGE, VARIANCE)) }
                    }),
                createText: (c, t, calc) =>
                string.Format(
                    "{0} attacks into {1}'s tails!\n{0} is pierced through by the tails!\n{0} takes <color=red>{2}</color> damage!",
                    t.DisplayName,
                    c.DisplayName,
                    -calc.TargetResources[ResourceType.HEALTH].False),
                sound: (c, t, calc) => "Slash_0"
                );
        }
    }
}