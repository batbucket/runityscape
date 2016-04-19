using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Spare : SpellFactory {
    public const string NAME = "Spare";
    public const string DESCRIPTION = "Attempt to spare your foe.";
    public const SpellType SPELL_TYPE = SpellType.MERCY;
    public const TargetType TARGET_TYPE = TargetType.SINGLE_ENEMY;
    public const string SUCCESS_CAST = "* {0} is sparing {1}.";
    public static readonly Dictionary<ResourceType, int> COSTS = new Dictionary<ResourceType, int>();
    int amount;

    public Spare() : base(NAME, DESCRIPTION, SPELL_TYPE, TARGET_TYPE, COSTS) { }

    protected override IDictionary<string, SpellComponent> CreateComponents(Character caster, Character target, Spell spell) {
        throw new NotImplementedException();
    }
}
