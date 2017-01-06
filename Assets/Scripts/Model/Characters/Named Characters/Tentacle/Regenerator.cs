using Script.Model.Characters.Named;
using Scripts.Model.Characters;
using Scripts.Model.Items;
using Scripts.Model.Items.Named;
using Scripts.Model.Spells.Named;
using Scripts.Model.Stats.Resources;
using Scripts.Presenter;
using System.Linq;
using UnityEngine;

namespace Scripts.Model.Characters.Named {

    public class Regenerator : Mimic {

        public Regenerator()
            : base(
                new Displays {
                    Loc = "tentacle",
                    Name = "Regenerator",
                    Color = Color.white,
                    Check = "A non-combatative tentacle with high life regeneration that can transfer its life to allies. Known as a \"regenerador\" in other languages."
                },
                new Displays {
                    Loc = "hospital-cross",
                    Name = "Bishop",
                    Color = Color.white,
                    Check = "A non-combatative magic user with healing spells. Rumor has it that a bishop can run extremely quickly, but only in ordinal directions."
                },
                new StartStats {
                    Lvl = 5,
                    Str = 1,
                    Int = 2,
                    Agi = 3,
                    Vit = 2
                },
                new Items { Inventory = new Item[] { new Apple() } }
                ) {
            this.Spells.Add(new Regenerate());
            this.Spells.Add(new TransferLife());
            this.Spells.Add(new Heal());
            AddResource(new NamedResource.DeathExperience(5));
            this.Inventory.Gold = Util.Random(10, .50f);
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
                    .OrderBy(c => ((float)c.GetResourceCount(ResourceType.HEALTH, false) / c.GetResourceCount(ResourceType.HEALTH, true)))
                    .FirstOrDefault();
                QuickCast(new Heal(), target);
            }
        }

        public override Mimic Summon() {
            return new Regenerator();
        }
    }
}