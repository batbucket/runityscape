using Scripts.Model.Characters;
using Scripts.Model.Spells;
using Scripts.Presenter;

namespace Scripts.Model.Items {

    /// <summary>
    /// Represents an item that can be used up.
    /// </summary>
    public abstract class ConsumableItem : Item {

        public ConsumableItem(string name, string description) : base(name, description) {
        }

        public override Critical CreateCritical() {
            return base.CreateCritical();
        }

        public override Hit CreateHit() {
            return new Hit(
                isState: (c, t, o) => true,
                calculation: (c, t, o) => {
                    return CreateCalculation(c, t);
                },
                perform: (c, t, calc, o) => {
                    Result.NumericPerform(c, t, calc);
                    c.Inventory.Remove(this);
                    OnPerform(c, t);
                },
                createText: (c, t, calc, o) => {
                    if (c == t) {
                        return SelfUseText(c, t, calc);
                    } else {
                        return OtherUseText(c, t, calc);
                    }
                }
            );
        }

        public override Miss CreateMiss() {
            return base.CreateMiss();
        }

        protected abstract Calculation CreateCalculation(Character caster, Character target);

        protected virtual void OnPerform(Character caster, Character target) {
        }

        protected abstract string OtherUseText(Character caster, Character target, Calculation calculation);

        protected abstract string SelfUseText(Character caster, Character target, Calculation calculation);

        protected virtual void SFX(Character caster, Character target, Calculation calculation) {
            Game.Instance.Sound.PlaySound("Sounds/Zip_0");
        }
    }
}