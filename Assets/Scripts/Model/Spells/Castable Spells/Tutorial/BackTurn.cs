using Scripts.Model.Stats.Resources;
using Scripts.Model.TextBoxes;
using Scripts.Presenter;
using UnityEngine;

namespace Scripts.Model.Spells.Named {

    public class BackTurn : SpellFactory {
        private bool isIndefinite;

        public BackTurn() : base(
            "Back Turn",
            "Back is turned to the enemy.",
            SpellType.DEFENSE,
            TargetType.SELF,
            new Cost[] { new Cost(ResourceType.SKILL, 2) }, abbreviation: "BAK", color: Color.red) {
        }

        public BackTurn(bool isIndefinite) : this() {
            this.isIndefinite = isIndefinite;
        }

        public override Hit CreateHit() {
            return new Hit(
                isState: (c, t) => true,
                duration: (c, t) => 6,
                isIndefinite: (c, t) => isIndefinite,
                react: (s) => {
                    if (s.SpellFactory is Attack) {
                        s.Result = s.Miss;
                        s.Result.CreateText = (c, t, calc) => null;
                        s.Calculation.TargetResources[ResourceType.HEALTH].False = 0;
                        (new TailSpike()).Cast(s.Target, s.Caster);
                    }
                },
                onStart: (c, t) => c.IsCharging = false,
                onEnd: (c, t) => {
                    c.IsCharging = true;
                    Game.Instance.TextBoxes.AddTextBox(new TextBox(string.Format("{0} is no longer turned around.", c.DisplayName)));
                },
                perform: (c, t, calc) => {
                },
                createText: (c, t, calc) => string.Format("{0} turns their back to their foes!", c.DisplayName)
                );
        }
    }
}