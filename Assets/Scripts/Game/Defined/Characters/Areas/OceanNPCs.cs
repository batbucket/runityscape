using Scripts.Game.Defined.Serialized.Items;
using Scripts.Game.Defined.Serialized.Spells;
using Scripts.Game.Defined.Serialized.Statistics;
using Scripts.Game.Serialized;
using Scripts.Game.Serialized.Brains;
using Scripts.Game.Shopkeeper;
using Scripts.Model.Characters;
using Scripts.Model.Pages;

namespace Scripts.Game.Defined.Characters {

    public static class OceanNPCs {

        public static Shop OceanShop(Page previous, Flags flags, Party party) {
            return new Shop(
                previous,
                "Fishy Market",
                flags,
                party,
                0.6f,
                1f,
                SharkPirate())
                .AddBuys(
                    new FishHook()
                );
        }

        public static Character Shark() {
            return CharacterUtil.StandardEnemy(
                new Stats(12, 1, 6, 8, 35),
                new Look(
                    "Cap'n Shark",
                    "shark-pirate",
                    "Hatless shark.",
                    Breed.FISH
                    ),
                new Attacker())
                .AddItem(new Money(), Util.RandomRange(5, 15));
        }

        public static Character SharkPirate() {
            return CharacterUtil.StandardEnemy(
                new Stats(12, 1, 6, 8, 35),
                new Look(
                    "Cap'n Shark",
                    "shark-pirate",
                    "Fierce captain of shark crew.",
                    Breed.FISH
                    ),
                new Attacker())
                .AddItem(new Money(), Util.RandomRange(5, 15));
        }

        public static Character BlackShuck() {
            return CharacterUtil.StandardEnemy(
                new Stats(3, 10, 2, 2, 10),
                new Look(
                    "Black Shuck",
                    "spectre",
                    "Its growl sends a shiver down your spine",
                    Breed.BEAST
                    ),
                new BlackShuck())
                .AddStats(new Skill())
                .AddSpells(new SetupCounter());
        }
    }
}