using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Spare : SpellFactory {
    public const string NAME = "Spare";
    public static readonly string DESCRIPTION = string.Format("Attempt to spare a foe.");
    public const SpellType SPELL_TYPE = SpellType.MERCY;
    public const TargetType TARGET_TYPE = TargetType.SINGLE_ENEMY;
    public const string SUCCESS_TEXT = "{0} is sparing {1}.";

    public Spare() : base(NAME, DESCRIPTION, SPELL_TYPE, TARGET_TYPE) { }

    public override Hit CreateHit() {
        return new Hit(
            sound: (c, t, calc, o) => {
                return "Blip_0";
            }
        );
    }
}
