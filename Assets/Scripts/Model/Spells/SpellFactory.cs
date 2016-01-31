using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public static class SpellFactory {
    public static Spell CreateSpell(Character caster, string spellName) {
        Spell spell = (Spell)(Activator.CreateInstance(Type.GetType(spellName), caster));
        return spell;
    }

    public static Spell createSpell(Character caster, string spellName, List<Character> targets) {
        Spell spell = CreateSpell(caster, spellName);
        return spell;
    }
}
