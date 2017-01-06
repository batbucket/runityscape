using Scripts.Model.Characters;
using Scripts.Model.World.Utility;
using Scripts.Model.Spells;
using Scripts.Model.Spells.Named;
using Scripts.Presenter;
using UnityEngine;
using Scripts.Model.Items;
using Scripts.Model.Items.Named;
using Scripts.Model.Stats.Resources;

namespace Scripts.Model.Characters.Named {

    public class Shopkeeper : ComputerCharacter {
        private const string NAME = "Maple";
        private const int HEALING_THRESHOLD = 5;
        private EventFlags flags;

        public Shopkeeper(EventFlags flags) :
            base(
            new Displays { Loc = "cowled", Color = Color.yellow, Name = "???", Check = "\"Fruit dealer of the plateau.\"" },
            new StartStats { Lvl = 5, Str = 1, Int = 2, Agi = 1, Vit = 1 },
            new Items(new Item[] { new Apple() },
                new EquippableItem[] { new OldSword(), new OldArmor() })
            ) {
            this.flags = flags;
            AddResource(new NamedResource.DeathExperience(20));
            Inventory.Gold = 1000;
            if (flags.Ints[Flag.SHOPKEEPER_STATUS] != Flag.SHOPKEEPER_FRIENDLY) {
                Inventory.Add(new SpellTome(new Smite()));
            }
        }

        public override void OnBattleStart() {
            if (flags.Ints[Flag.SHOPKEEPER_STATUS] == Flag.SHOPKEEPER_FRIENDLY) {
                Game.Instance.TextBoxes.AddTextBox(Talk("Hmm? Is there a monster nearby?"));
                (new Unaware()).Cast(this, this);
            } else if (flags.Ints[Flag.SHOPKEEPER_STATUS] == Flag.SHOPKEEPER_NEUTRAL) {
                Game.Instance.TextBoxes.AddTextBox(Talk("What do you think you're doing?"));
            } else if (flags.Ints[Flag.SHOPKEEPER_STATUS] == Flag.SHOPKEEPER_ENEMY) {
                Game.Instance.TextBoxes.AddTextBox(Talk("Another round."));
            }
        }

        protected override void OnTick() {
            this.Name = flags.Bools[Flag.SHOPKEEPER_GAVE_NAME] ? NAME : "???";
        }

        public override void React(Spell spell) {
            if (flags.Ints[Flag.SHOPKEEPER_STATUS] == Flag.SHOPKEEPER_NEUTRAL && spell.SpellFactory.SpellType == SpellType.OFFENSE && State == CharacterState.NORMAL) {
                Game.Instance.TextBoxes.AddTextBox(Talk("So be it."));
                flags.Ints[Flag.SHOPKEEPER_STATUS] = Flag.SHOPKEEPER_ENEMY;
            }
        }

        public override void OnVictory() {
            base.OnVictory();
            Game.Instance.TextBoxes.AddTextBox(Talk("\"Killed by a shopkeeper.\" How's that for an epitath?"));
        }

        public override void OnDefeat() {
            base.OnDefeat();
            Game.Instance.Sound.StopAllSounds();
            if (flags.Ints[Flag.SHOPKEEPER_STATUS] == Flag.SHOPKEEPER_FRIENDLY) {
                Game.Instance.TextBoxes.AddTextBox(Talk("I thought... we were friends..."));
            } else {
                Game.Instance.TextBoxes.AddTextBox(Talk("..."));
            }
        }

        public override void OnKill() {
            base.OnKill();
            flags.Ints[Flag.SHOPKEEPER_STATUS] = Flag.SHOPKEEPER_DEAD;
        }

        protected override void DecideSpell() {
            if (flags.Ints[Flag.SHOPKEEPER_STATUS] == Flag.SHOPKEEPER_ENEMY) {
                if (GetResourceCount(ResourceType.HEALTH, false) <= HEALING_THRESHOLD) {
                    QuickCast(new Apple());
                }
                QuickCast(new Attack());
            }
        }
    }
}