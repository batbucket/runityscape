using UnityEngine;
using System.Collections;
using System;

public static class SpellFactory {
    public static Spell createSpell(string s) {
        Spell spell = (Spell)(Activator.CreateInstance(Type.GetType(s)));
        return spell;
    }

    public static Spell createSpell(string s, Character caster, params Character[] targets) {
        Spell spell = createSpell(s);
        spell.initialize(caster, targets);
        return spell;
    }
}
