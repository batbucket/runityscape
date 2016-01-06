using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public abstract class Item : Spell
{
    public const SpellType SPELL_TYPE = SpellType.BOOST;
    public const TargetType TARGET_TYPE = TargetType.SINGLE_ALLY;

    public Item(Character caster, string name) : base(caster, name, SPELL_TYPE, TARGET_TYPE) {
    }
}
