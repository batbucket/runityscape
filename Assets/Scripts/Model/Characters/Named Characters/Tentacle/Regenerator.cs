using UnityEngine;
using System.Collections;
using System.Linq;
using System;

public class Regenerator : Mimic {
    public override int ExperienceGiven {
        get {
            return 10;
        }
    }

    public override int GoldGiven {
        get {
            return Util.Range(10, 20);
        }
    }

    public Regenerator()
        : base(
            new Displays {
                Loc = "Icons/tentacle",
                Name = "Regenerator",
                Color = Color.white,
                Check = "A non-combatative tentacle with high life regeneration and can transfer its life to allies. Known as a \"regenerador\" in other languages."
            },
            new Displays {
                Loc = "Icons/hospital-cross",
                Name = "Bishop?",
                Color = Color.white,
                Check = "A non-combatative magic user with healing spells. Rumor has it that a bishop can run extremely quickly, but only in ordinal directions."
            },
            new Attributes {
                Lvl = 2,
                Str = 1,
                Int = 2,
                Agi = 1,
                Vit = 2
            }
            ) {
        AddResource(new NamedResource.Mana((NamedAttribute.Intelligence)Attributes[AttributeType.INTELLIGENCE]));
    }

    public override void OnBattleStart() {
        AddToResource(ResourceType.MANA, false, GetResourceCount(ResourceType.MANA, true));
    }

    protected override void DecideSpell() {
        if (isTransformed) {
            if (!Buffs.Any(b => b.SpellFactory.Name == "Regenerate")) {
                QuickCast(new Regenerate());
            }

            if (GetResourceCount(ResourceType.HEALTH, false) > 1) {

                /**
                 * Choose lowest health, non-regenerator ally to transfer life to.
                 */
                Character target = Game.Instance
                    .CurrentPage.GetAllies(this)
                    .Where(c => !(c is Regenerator) && c.State == CharacterState.NORMAL)
                    .OrderBy(c => c.GetResourceCount(ResourceType.HEALTH, false))
                    .FirstOrDefault();

                if (target != null && target.GetResourceCount(ResourceType.HEALTH, false) < target.GetResourceCount(ResourceType.HEALTH, true)) {
                    QuickCast(new TransferLife(), target);
                }
            }
        } else {
            /**
             * Choose lowest health, ally to transfer life to (includes other regenerators).
             */
            Character target = Game.Instance
                .CurrentPage.GetAllies(this)
                .Where(c => c.State == CharacterState.NORMAL)
                .OrderBy(c => c.GetResourceCount(ResourceType.HEALTH, false))
                .FirstOrDefault();
            QuickCast(new Heal(), target);
        }
    }

    public override Mimic Summon() {
        return new Regenerator();
    }
}
