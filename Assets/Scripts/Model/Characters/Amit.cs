using UnityEngine;
using System.Collections;

public class Amit : PlayerCharacter {
    public Amit() : base(Util.getSprite("crying_mudkip"), "Amit", 0, 5, 5, 5, 5) {
        addResources(ResourceFactory.createResource(ResourceType.SKILL, 3));
        fightSpells.Add("Attack");
    }
}