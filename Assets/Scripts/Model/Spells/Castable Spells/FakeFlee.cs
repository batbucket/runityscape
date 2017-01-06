using Scripts.Model.Characters;
using Scripts.Model.Pages;
using Scripts.Presenter;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Scripts.Model.Spells.Named {

    public class FakeFlee : SpellFactory {

        public FakeFlee() : base("Flee", "Escape from battle.", SpellType.MERCY, TargetType.SELF) {
        }

        public override Hit CreateHit() {
            return new Hit(
                isState: (c, t) => true,
                perform: (c, t, calc) => Game.Instance.StartCoroutine(Escape(t)),
                createText: (c, t, calc) => string.Format("{0} flees from battle!", t.DisplayName)
                );
        }

        protected override bool Castable(Character caster, Character target) {
            return !Game.Instance.CurrentPage.GetEnemies(caster).Any(c => c.State == CharacterState.DEFEAT);
        }

        private IEnumerator Escape(Character escapee) {
            escapee.IsCharging = false;
            yield return new WaitForSeconds(0.5f);
            Game.Instance.CurrentPage.GetCharacters(escapee.Side).Remove(escapee);
        }
    }
}