using UnityEngine;
using System.Collections;

public class SpellDetails {
    Spell _spell;
    public Spell Spell { get { return _spell; } }

    Result _res;
    public Result Result { get { return _res; } }

    Calculation _calc;
    public Calculation Calculation { get { return _calc; } }

    public SpellDetails(Spell spell, Result res, Calculation calc) {
        this._spell = spell;
        this._res = res;
        this._calc = calc;
    }
}
