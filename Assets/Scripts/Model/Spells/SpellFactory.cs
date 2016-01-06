using UnityEngine;
using System.Collections;
using System;

public static class SpellFactory {
    public static Spell createSpell(Character caster, string spellName) {
        Spell spell = (Spell)(Activator.CreateInstance(Type.GetType(spellName), caster));
        return spell;
    }

    public static Spell createSpell(Character caster, string spellName, params Character[] targets) {
        Spell spell = createSpell(caster, spellName);
        spell.setTargets(targets);
        return spell;
    }
}
