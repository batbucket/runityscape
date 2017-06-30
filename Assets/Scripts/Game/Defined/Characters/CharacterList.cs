using Scripts.Game.Defined.Characters;
using Scripts.Game.Defined.Characters.StartingSpells;
using Scripts.Game.Defined.Serialized.Characters;
using Scripts.Game.Defined.Serialized.Items.Consumables;
using Scripts.Game.Defined.Serialized.Items.Equipment;
using Scripts.Game.Defined.Serialized.Items.Misc;
using Scripts.Game.Defined.StartingStats;
using Scripts.Game.Undefined.StartingStats;
using Scripts.Model.Characters;

namespace Scripts.Game.Defined.Characters {
    public class Kitsune : Character {
        public Kitsune() : base(new KitsuneStats(), new KitsuneLook(), new Player(), new DebugSpells()) {
            this.Inventory.Add(new Apple());
            this.Inventory.Add(new Apple());
            this.Inventory.Add(new Apple());
            this.Inventory.Add(new Apple());
            this.Inventory.Add(new Apple());
            this.Inventory.Add(new Apple());
            this.Inventory.Add(new Apple());
            this.Inventory.Add(new PoisonArmor());
            this.Inventory.Add(new Money(), 100);
            this.flags.Add(Flag.DROPS_ITEMS);
        }
    }

    public class Hero : Character {
        public Hero(string name) : base(new HeroStats(), new HeroLook(name), new Player(), new HeroSpells()) {
            flags.Add(Flag.PLAYER);
            flags.Add(Flag.PERSISTS_AFTER_DEFEAT);
        }
    }
}

namespace Scripts.Game.Undefined.Characters {
    public class CreditsDummy : Character {
        public CreditsDummy(
            Breed breed,
            int level,
            string name,
            string spriteLoc,
            string tip)
            : base(new DummyStats(level),
                  new DummyLook(breed, name, Util.GetSprite(spriteLoc), tip),
                  new DebugAI(),
                  new DebugSpells()) {

        }
    }
}