using Scripts.Model.Characters;
using Scripts.Model.Stats;
using Scripts.Model.Stats.Resources;
using System.Collections.Generic;

namespace Scripts.Model.Spells.Named {

    public class Heal : SpellFactory {

        public Heal() : base(SpellType.BOOST, TargetType.SINGLE_ALLY, "Heal", "Heal a target.") {
        }

        public override Hit CreateHit() {
            return new Hit(
                isState: (c, t) => true,
                calculation: (c, t) => new Calculation(targetResources: new Dictionary<ResourceType, PairedValue>() { { ResourceType.HEALTH, new PairedValue(0, 5) } }),
                sound: (c, t, calc) => "Zip_0",
                createText: (c, t, calc) =>
                    string.Format("{0} heals {1} for {2} life.",
                        c.DisplayName,
                        c != t ? t.DisplayName : "themselves",
                        Util.Color(calc.TargetResources[ResourceType.HEALTH].False + "", ResourceType.HEALTH.FillColor))
                );
        }

        protected override bool Castable(Character caster, Character target) {
            return target.GetResourceCount(ResourceType.HEALTH, false) < target.GetResourceCount(ResourceType.HEALTH, true);
        }
    }
}