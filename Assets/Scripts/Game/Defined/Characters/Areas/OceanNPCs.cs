using Scripts.Game.Defined.Serialized.Buffs;
using Scripts.Game.Defined.Serialized.Items;
using Scripts.Game.Defined.Serialized.Spells;
using Scripts.Game.Defined.Serialized.Statistics;
using Scripts.Game.Defined.Unserialized.Spells;
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
                    new FishHook(),
                    new Cleanse()
                );
        }

        public static Character Shark() {
            return CharacterUtil.StandardEnemy(
                new Stats(5, 5, 6, 8, 35),
                new Look(
                    "Shark",
                    "shark",
                    "Hatless shark.",
                    Breed.FISH
                    ),
                new Attacker())
                .AddItem(new Money(), Util.RandomRange(50, 100));
        }

        public static Character SharkPirate() {
            return CharacterUtil.StandardEnemy(
                new Stats(6, 8, 6, 8, 60),
                new Look(
                    "Cap'n Shark",
                    "shark-pirate",
                    "Fierce captain of shark crew.",
                    Breed.FISH
                    ),
                new Attacker())
                .AddItem(new Money(), Util.RandomRange(5, 15));
        }

        public static Character Siren() {
            return CharacterUtil.StandardEnemy(
                    new Stats(6, 4, 10, 10, 20),
                    new Look(
                        "Siren",
                        "siren",
                        "Sings a mean tune.",
                        Breed.FISH
                    ),
                    new Siren()
                ).AddSpells(Game.Serialized.Brains.Siren.DEBUFF_LIST);
        }

        public static Character Tentacle() {
            return CharacterUtil.StandardEnemy(
                    new Stats(7, 3, 5, 1, 5),
                    new Look(
                        "Tentacle",
                        "shark",
                        "Tentacle belonging to a Kraken.",
                        Breed.FISH
                        ),
                    new Attacker()
                );
        }

        public static Character Kraken() {
            return CharacterUtil.StandardEnemy(
                    new Stats(8, 5, 10, 10, 100),
                    new Look(
                        "Kraken",
                        "shark",
                        "Giant squid thing.",
                        Breed.FISH
                        ),
                    new Kraken()
                )
                .AddSpells(new SpawnTentacles())
                .AddStats(new Skill());
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

        public static Character Elemental() {
            return CharacterUtil.StandardEnemy(
                new Stats(9, 5, 20, 15, 20),
                new Look(
                    "Undine",
                    "villager",
                    "Sea elemental.",
                    Breed.FISH
                    ),
                new Elemental()
                )
                .AddStats(new Mana())
                .AddSpells(new WaterboltSingle(), new WaterboltMulti())
                .AddBuff(new Insight());
        }

        public static Character DreadSinger() {
            return CharacterUtil.StandardEnemy(
                    new Stats(10, 5, 20, 20, 25),
                    new Look(
                        "Sea Witch",
                        "siren",
                        "Singer of the voices of dread.",
                        Breed.FISH
                        ),
                    new DreadSinger());
        }

        public static Character Swarm() {
            return CharacterUtil.StandardEnemy(
                new Stats(2, 1, 5, 2, 15),
                new Look(
                    "Swarm",
                    "angler-fish",
                    "Questionable member of the sea that travels in schools.",
                    Breed.FISH
                    ),
                new Swarm())
                .AddItem(new Money(), Util.RandomRange(0, 1));
        }
    }
}