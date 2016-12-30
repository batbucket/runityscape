using UnityEngine;
using System.Collections;
using System;

public class SpellTome : ConsumableItem {
    private SpellFactory spell;

    public SpellTome(SpellFactory spell)
        : base(
            string.Format("Tome: {0}", spell.Name),
            string.Format("Teaches {0} - {1}: {2}", spell.Name, spell.GetCosts(), spell.Description)) {
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
        return string.Format("{0} taught the Spell: {1} to {2}.", caster.Name, spell.Name, target.Name);
    }

    protected override string SelfUseText(Character caster, Character target, Calculation calculation) {
        return string.Format("{0} taught themselves {1}.", caster.Name, spell.Name);
    }
}
