using UnityEngine;
using System.Collections;
using System.Linq;

public class Regenerator : Tentacle {

    public Regenerator()
        : base("Icons/tentacle", "Regenerator", 1, 0, 5, 3, 1, Color.white, 2, "A tentacle with high life regeneration that can transfer its life to allies. Unable to attack.") {
    }

    public override void OnBattleStart() {
        (new Regenerate()).Cast(this, this);
    }

    protected override void DecideSpell() {
        if (GetResourceCount(ResourceType.HEALTH, false) > 1) {

            /**
             * Choose lowest health, non-regenerator ally to transfer life to.
             */
            Character target = Game.Instance.PagePresenter
                .Page.GetAllies(this, false)
                .Where(c => !(c is Regenerator))
                .OrderBy(c => c.GetResourceCount(ResourceType.HEALTH, false))
                .FirstOrDefault();

            if (target != null && target.GetResourceCount(ResourceType.HEALTH, false) < target.GetResourceCount(ResourceType.HEALTH, true)) {
                QuickCast(new TransferLife(), target);
            }
        }
    }
}
