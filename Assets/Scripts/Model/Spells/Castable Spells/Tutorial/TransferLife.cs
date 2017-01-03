using Scripts.Model.Stats;
using Scripts.Model.Stats.Resources;
using Scripts.View.Effects;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Model.Spells.Named {

    public class TransferLife : SpellFactory {

        public TransferLife() : base("Transfer Life", "Transfer all but 1 Life to an ally.", SpellType.BOOST, TargetType.SINGLE_ALLY) {
        }

        public override Hit CreateHit() {
            return new Hit(
                isState: (c, t, o) => true,
                calculation:
                (c, t, o) => {
                    int count = c.GetResourceCount(ResourceType.HEALTH, false) - 1;
                    return new Calculation(
                        casterResources:
                        new Dictionary<ResourceType, PairedValue>() {
                    { ResourceType.HEALTH, new PairedValue(0, -count) }
                        },
                        targetResources:
                        new Dictionary<ResourceType, PairedValue>() {
                    { ResourceType.HEALTH, new PairedValue(0, count) }
                        }
                    );
                },
                createText: (c, t, calc, o) =>
                string.Format(
                    "{0} transfers {1} {2} to {3}!",
                    c.DisplayName,
                    Util.Color("" + calc.TargetResources[ResourceType.HEALTH].False, Color.magenta),
                    "<color=lime>life</color>",
                    t.DisplayName
                ),
                sound: (c, t, calc, o) => "Zip_0",
                sfx: (c, t, calc, o) => new CharacterEffect[] {
                new HitsplatEffect(c.Presenter.PortraitView, Color.red, "" + calc.CasterResources[ResourceType.HEALTH].False),
                new HitsplatEffect(t.Presenter.PortraitView, Color.green, "" + calc.TargetResources[ResourceType.HEALTH].False)
                }
                );
        }
    }
}