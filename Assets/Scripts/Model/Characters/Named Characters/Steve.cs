using UnityEngine;
using System.Linq;

public class Steve : ComputerCharacter {

    public Steve() : base("laughing_shinx", "Steve", 0, 10, 2, 5, 5, Color.red, 4) {
        AddResource(new NamedResource.Skill());
        Selections[Selection.SPELL].Add(new Counter());
    }

    bool done;

    protected override void DecideSpell() {
        if (!done && !Buffs.Select(b => b.SpellFactory.Name).Contains("Counter") && Game.Instance.PagePresenter.Page.GetAllies(this.Side)[0] == this) {
            QuickCast(new Counter());
            done = true;
        }

        if ((GetResourceCount(ResourceType.HEALTH, false) + 0.0f) / GetResourceCount(ResourceType.HEALTH, true) < .5f) {
            QuickCast(new Meditate());
        }
    }

    protected override void WhileFullCharge() {
        DecideSpell();
    }
}
