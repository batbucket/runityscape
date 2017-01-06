using Scripts.Model.Characters;
using Scripts.Model.Spells;
using Scripts.Model.Stats.Resources;

namespace Scripts.Model.Items.Named {

    public class SpellTome : ConsumableItem {
        private SpellFactory spell;

        public SpellTome(SpellFactory spell)
            : base(
                string.Format("Tome: {0}", spell.Name),
                string.Format("Teaches {0} - {1}: {2}", spell.Name, spell.GetCosts(), spell.Description),
                true) {
            this.spell = spell;
        }

        protected override void OnPerform(Character caster, Character target) {
            target.Spells.Add(spell);
            target.AddResource(new NamedResource.Skill());
        }

        protected override Calculation CreateCalculation(Character caster, Character target) {
            return new Calculation();
        }

        protected override string OtherUseText(Character caster, Character target, Calculation calculation) {
            return string.Format("{0} uses {2} on {1}.\n{1} learns the spell {3}.", caster.Name, target.Name, this.name, this.spell.Name);
        }

        protected override string SelfUseText(Character caster, Character target, Calculation calculation) {
            return string.Format("{0} uses {1} on themselves.\n{0} learns the spell {2}.", caster.Name, this.name, spell.Name);
        }
    }
}