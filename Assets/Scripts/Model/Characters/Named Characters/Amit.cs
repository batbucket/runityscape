using UnityEngine;
using System.Collections;
using System;

public class Amit : PlayerCharacter {
    public Amit() : base(Util.GetSprite("crying_mudkip"), "Amit", 0, 5, 5, 5, 5, Color.white) {
        Side = false;
        AddResource(new NamedResource.Skill());
        this.Attack = new Attack();
        this.Selections[Selection.SPELL].Add(new Meditate());
        this.Selections[Selection.SPELL].Add(new Counter());
        this.Selections[Selection.ACT].Add(new Check());
        this.Selections[Selection.ITEM].Add(new Lobster(2));
        this.Selections[Selection.ITEM].Add(new Scimitar(2));
        this.Selections[Selection.MERCY].Add(new Spare());
    }
}