using UnityEngine;
using System.Collections;
using System;

public class LesserHeal : Spell
{

    public LesserHeal()
    {
        setSpellDamage(-1);
        setSpellName("Lesser Heal");
        setResourceCost(1);
        setResourceCostType(ResourceType.MANA);
        setSpellType(SpellType.BOOST);
        //setUser(//put a character )

        setTextBoxDescription(string.Format("{0} warms {1} with a glowing aura\n{1} recovered {2} damage!", user, target, -1*spellDamage));
    }

    public override void stubMethod()
    {
        throw new NotImplementedException();
    }
}