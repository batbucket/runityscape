using Scripts.Game.Defined.Characters;
using Scripts.Game.Defined.Characters.StartingSpells;
using Scripts.Game.Defined.Serialized.Characters;
using Scripts.Game.Defined.Serialized.Items.Consumables;
using Scripts.Game.Defined.Serialized.Items.Equipment;
using Scripts.Game.Defined.Serialized.Items.Misc;
using Scripts.Game.Defined.Serialized.Spells;
using Scripts.Game.Defined.Serialized.Statistics;
using Scripts.Game.Defined.StartingStats;
using Scripts.Game.Undefined.StartingStats;
using Scripts.Model.Characters;
using Scripts.Model.Items;
using Scripts.Model.Spells;
using Scripts.Model.Stats;
using UnityEngine;

namespace Scripts.Game.Defined.Characters {
    public static class CharacterList {
        #region Character Builder stuff
        private static Character RemoveFlag(this Character c, Model.Characters.Flag flag) {
            c.RemoveFlag(flag);
            return c;
        }

        private struct ItemCount {
            public readonly Item Item;
            public readonly int Count;
            public ItemCount(Item item, int count) {
                this.Count = count;
                this.Item = item;
            }

            public ItemCount(Item item) {
                this.Item = item;
                this.Count = 1;
            }
        }

        private static Character StandardEnemy(Stats stats, Look look, Brain brain) {
            Character enemy = new Character(stats, look, brain);
            enemy.AddFlag(Model.Characters.Flag.DROPS_ITEMS);
            enemy.AddFlag(Model.Characters.Flag.GIVES_EXPERIENCE);
            return enemy;
        }

        private static Character AddStats(this Character c, params Stat[] stats) {
            foreach (Stat stat in stats) {
                c.Stats.AddStat(stat);
            }
            return c;
        }

        private static Character AddSpells(this Character c, params SpellBook[] books) {
            foreach (SpellBook sb in books) {
                c.Spells.AddSpellBook(sb);
            }
            return c;
        }

        private static Character AddFlags(this Character c, params Model.Characters.Flag[] flags) {
            foreach (Model.Characters.Flag flag in flags) {
                c.AddFlag(flag);
            }
            return c;
        }

        private static Character AddItems(this Character c, params ItemCount[] items) {
            foreach (ItemCount itemCount in items) {
                c.Inventory.ForceAdd(itemCount.Item, itemCount.Count);
            }
            return c;
        }

        private static Character AddItem(this Character c, Item item, int count, bool isAdded = true) {
            if (isAdded) {
                c.Inventory.ForceAdd(item, count);
            }
            return c;
        }

        private static Character AddItem(this Character c, Item item, bool isAdded = true) {
            return c.AddItem(item, 1, isAdded);
        }

        private static Character AddEquips(this Character c, params EquippableItem[] equips) {
            foreach (EquippableItem equip in equips) {
                Inventory dummy = new Inventory();
                dummy.ForceAdd(equip);
                c.Equipment.AddEquip(dummy, new Model.Buffs.BuffParams(c.Stats, c.Id), equip);
            }
            return c;
        }

        private static Character AddEquip(this Character c, EquippableItem equip, bool isAdded = true) {
            if (isAdded) {
                Inventory dummy = new Inventory();
                dummy.ForceAdd(equip);
                c.Equipment.AddEquip(dummy, new Model.Buffs.BuffParams(c.Stats, c.Id), equip);
            }
            return c;
        }

        #endregion

        public static Character Hero(string name) {
            return new Character(
                new Stats(0, 1, 1, 2, 5),
                new Look(
                    name,
                    "person",
                    "It's you!",
                    Breed.PROGRAMMER
                    ),
                new Player())
                .AddFlags(Model.Characters.Flag.PLAYER, Model.Characters.Flag.PERSISTS_AFTER_DEFEAT)
                .AddStats(new Experience());
        }

        public static Character Partner(string name) {
            return new Character(
                new Stats(0, 1, 2, 1, 5),
                new Look(
                    name,
                    "visored-helm",
                    "Has a helmet icon despite not wearing one.",
                    Breed.HUMAN
                    ),
                new Player())
                .AddFlags(Model.Characters.Flag.PLAYER, Model.Characters.Flag.PERSISTS_AFTER_DEFEAT)
                .AddStats(new Experience());
        }

        public static Character TestEnemy() {
            Character c = new Character(
                new Stats(5, 1, 1, 5, 1),
                    new Look(
                    "Test Enemy",
                    "fox-head",
                    "Hello???",
                    Breed.UNKNOWN,
                    Color.magenta
                    ),
                new Player())
                .AddItems(new ItemCount(new Apple(), 7), new ItemCount(new PoisonArmor()), new ItemCount(new Money(), 100))
                .AddFlags(Model.Characters.Flag.DROPS_ITEMS)
                .AddSpells(new InflictPoison(), new SetupCounter())
                .AddStats(new Skill());
            return c;
        }

        public static class Field {

            public static Character Villager() {
                return StandardEnemy(
                    new Stats(2, 1, 1, 1, 2),
                    new Look(
                        "Ghost",
                        "haunting",
                        "A villager who didn't make it.",
                        Breed.SPIRIT
                        ),
                    new FieldBrains.Attacker())
                    .AddItem(new Money(), Util.Range(0, 3));
            }

            public static Character Knight() {
                return StandardEnemy(
                    new Stats(3, 1, 2, 2, 5),
                    new Look(
                        "Spectre",
                        "spectre",
                        "A knight who didn't make it. May be armed.",
                        Breed.SPIRIT
                        ),
                    new FieldBrains.Attacker())
                    .AddItem(new Item[] { new BrokenSword(), new GhostArmor() }.ChooseRandom(), Util.IsChance(.50f));
            }

            public static Character BigKnight() {
                return StandardEnemy(
                    new Stats(3, 1, 2, 2, 15),
                    new Look(
                        "Big Knight",
                        "spectre",
                        "It's a big guy.",
                        Breed.SPIRIT
                        ),
                    new FieldBrains.BigKnight())
                    .AddStats(new Skill())
                    .AddSpells(new SetupCounter());
            }

            public static Character Healer() {
                return StandardEnemy(
                    new Stats(3, 1, 5, 5, 1),
                    new Look(
                        "Spirit Healer",
                        "health-normal",
                        "Healer in life. Healer in death.",
                        Breed.SPIRIT
                        ),
                    new FieldBrains.Healer())
                    .AddItem(new Apple())
                    .AddSpells(new Heal());
            }

            public static Character Illusionist() {
                return StandardEnemy(
                    new Stats(3, 2, 3, 8, 15),
                    new Look(
                        "Illusionist",
                        "spectre",
                        "A wicked master of illusions.",
                        Breed.SPIRIT
                        ),
                    new FieldBrains.Illusionist())
                    .AddSpells(new Blackout());
            }

            private static Look ReplicantLook() {
                return new Look(
                        "Replika",
                        "spectre",
                        string.Empty,
                        Breed.SPIRIT,
                        Color.magenta
                        );
            }

            public static Character Replicant() {
                return StandardEnemy(
                    new Stats(10, 2, 5, 10, 30),
                    ReplicantLook(),
                    new FieldBrains.Replicant()
                    )
                .AddFlags(Model.Characters.Flag.PERSISTS_AFTER_DEFEAT)
                .AddSpells(new ReflectiveClone(), new SetupCounter());
            }
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
                  new Look(name, spriteLoc, tip, breed),
                  new DebugAI()) {

        }
    }
}