using UnityEngine;
using System.Collections;
using System;

public class SearingStrike :Spell {

    public SearingStrike()
    {
        setSpellDamage(1);
        setSpellName("Searing Strike");
        setResourceCost(2);
        setResourceCostType(ResourceType.SKILL);
        setSpellType(SpellType.OFFENSE);
        //setUser(//put a character )
        
        setTextBoxDescription(string.Format("{0} launches a bolt of FIRE at {1}!\n{1} took {2} damage!", user, target, spellDamage));
    }

    public override void stubMethod()
    {
        throw new NotImplementedException();
    }
}
