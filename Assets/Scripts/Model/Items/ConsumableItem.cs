using Scripts.Model.Characters;
using Scripts.Model.Spells;
using Scripts.Presenter;

namespace Scripts.Model.Items {

    /// <summary>
    /// Represents an item that can be used up.
    /// </summary>
    public abstract class ConsumableItem : Item {

        public ConsumableItem(string name, string description, bool isKeyItem = false) : base(name, description, isKeyItem) {
        }

        public override Critical CreateCritical() {
            return base.CreateCritical();
        }

        public override Hit CreateHit() {
            return new Hit(
                isState: (c, t) => true,
                calculation: (c, t) => {
                    return CreateCalculation(c, t);
                },
                perform: (c, t, calc) => {
                    Result.NumericPerform(c, t, calc);
                    c.Inventory.Remove(this);
                    OnPerform(c, t);
                },
                createText: (c, t, calc) => {
                    if (c == t) {
                        return SelfUseText(c, t, calc);
                    } else {
                        return OtherUseText(c, t, calc);
                    }
                },
                sound: (c, t, calc) => "Zip_0"
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