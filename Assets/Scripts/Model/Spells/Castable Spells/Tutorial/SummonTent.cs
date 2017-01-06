using Script.Model.Characters.Named;
using Scripts.Model.Stats;
using Scripts.Model.Stats.Resources;
using Scripts.Presenter;
using System.Collections.Generic;

namespace Scripts.Model.Spells.Named {

    public class SummonTent : SpellFactory {
        private IList<Mimic> tents;

        public SummonTent(IList<Mimic> tents) : base("SummonTent", "Summon tentacles", SpellType.OFFENSE, TargetType.SELF) {
            this.tents = tents;
        }

        public override Hit CreateHit() {
            return new Hit(
                isState: (c, t) => true,
                perform: (c, t, calc) => {
                    for (int i = 0; i < tents.Count; i++) {
                        Mimic s = tents[i].Summon();
                        Game.Instance.CurrentPage.AddCharacters(c.Side, s);
                    }
                },
                createText: (c, t, calc) => string.Format("Tentacles erupt from the ground on {0}'s side!", c.DisplayName)
                );
        }
    }
}