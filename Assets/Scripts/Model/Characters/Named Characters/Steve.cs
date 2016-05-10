using UnityEngine;
using System.Linq;

public class Steve : ComputerCharacter {

    public Steve() : base(Util.GetSprite("laughing_shinx"), "Steve", 0, 10, 2, 5, 5, Color.red, 4) {
        AddResource(ResourceType.SKILL, 3);
        Selections[Selection.SPELL].Add(new Counter());
    }

    bool done;

    protected override void DecideSpell() {
        if (!done && !Buffs.Select(b => b.SpellFactory.Name).Contains("Counter") && Game.Instance.PagePresenter.Page.GetAllies(this.Side)[0] == this) {
            QuickCast(new Counter());
            done = true;
        }

        if (Resources[ResourceType.HEALTH].GetRatio() < .5f) {
            QuickCast(new Meditate());
        }
    }

    protected override void WhileFullCharge() {
        DecideSpell();
    }
}
