using Scripts.Model.Characters;
using Scripts.Model.Stats.Resources;
using Scripts.View.Effects;
using UnityEngine;

namespace Scripts.Model.Spells.Named {

    public class Unaware : SpellFactory {

        public Unaware() : base("Unaware", "This unit's guard is down. Attacks that strike this unit will deal massive damage.", SpellType.OTHER, TargetType.SELF, abbreviation: "UNA", color: Color.red) {
        }

        public override Hit CreateHit() {
            return new Hit(
                isState: (c, t) => true,
                isIndefinite: (c, t) => t.State == CharacterState.NORMAL,
                react: (s) => {
                    if (s.Calculation.TargetResources[ResourceType.HEALTH].False < 0) {
                        s.Calculation.TargetResources[ResourceType.HEALTH].False = -s.Target.GetResourceCount(ResourceType.HEALTH, true) * Random.Range(99, 999);
                        s.Result.CreateText = (c, t, calc) =>
                        string.Format(
                            "{0} strikes {1} while their guard is down!\n{1} takes <color=red>{2}</color> damage!",
                            c.DisplayName,
                            t.DisplayName,
                            -calc.TargetResources[ResourceType.HEALTH].False);
                    }
                }
                );
        }
    }
}