using Scripts.Game.Defined.Serialized.Buffs;
using Scripts.Game.Defined.Serialized.Items;
using Scripts.Game.Defined.Serialized.Spells;
using Scripts.Game.Defined.Serialized.Statistics;
using Scripts.Game.Defined.Unserialized.Buffs;
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
                Shark())
                .AddBuys(
                    new FishHook(),
                    new Cleansing(),
                    new VitalityTrinket(),
                    new IntellectTrinket(),
                    new StrengthTrinket(),
                    new AgilityTrinket()
                );
        }

        public static Trainer OceanTrainer(Page previous, Party party) {
            return new Trainer(
                previous,
                party,
                Siren(),
                    new PurchasedSpell(100, new Purge()),
                    new PurchasedSpell(100, new CrushingBlow()),
                    new PurchasedSpell(200, new MassCheck()),
                    new PurchasedSpell(200, new Arraystrike())
                );
        }

        public static InventoryMaster OceanMaster(Page previous, Party party) {
            return new InventoryMaster(
                previous,
                party,
                Siren(),
                6,
                10,
                200
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
                .AddBuff(new RoughSkin())
                .AddItem(new SharkFin(), Util.IsChance(.50f))
                .AddItem(new Money(), Util.RandomRange(50, 100));
        }

        public static Character SharkPirate() {
            return CharacterUtil.StandardEnemy(
                new Stats(10, 8, 6, 8, 40),
                new Look(
                    "Cap'n Selach",
                    "shark-pirate",
                    "Fierce captain of shark crew.",
                    Breed.FISH
                    ),
                new SharkPirate())
                .AddBuff(new RougherSkin())
                .AddItem(new SharkFin(), Util.IsChance(.75f))
                .AddSpells(new SummonSeaCreatures(), new OneShotKill(), new CastDelayedDeath(), new GiveOverwhelmingPower());
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
                        "tentacle",
                        "Tentacle belonging to a Kraken.",
                        Breed.FISH
                        ),
                    new Attacker()
                );
        }

        public static Character Kraken() {
            return CharacterUtil.StandardEnemy(
                    new Stats(8, 5, 10, 20, 100),
                    new Look(
                        "Kracko",
                        "kraken",
                        "Giant squid thing. Commonly mistaken for a cloud.",
                        Breed.FISH
                        ),
                    new Kraken()
                )
                .AddSpells(new SpawnTentacles())
                .AddSpells(new CrushingBlow())
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
                    "Elemental",
                    "villager",
                    "Sea elemental.",
                    Breed.FISH
                    ),
                new Elemental()
                )
                .AddStats(new Mana())
                .AddSpells(new WaterboltSingle(), new WaterboltMulti())
                .AddBuff(new Insight())
                .AddItem(new Cleansing(), Util.IsChance(.25f));
        }

        public static Character DreadSinger() {
            return CharacterUtil.StandardEnemy(
                    new Stats(10, 5, 20, 20, 25),
                    new Look(
                        "Sea Witch",
                        "siren",
                        "Singer of the voices of death.",
                        Breed.FISH
                        ),
                    new DreadSinger())
                    .AddSpells(new CastDelayedDeath())
                    .AddItem(new Cleansing(), 1);
        }

        public static Character Swarm() {
            return CharacterUtil.StandardEnemy(
                new Stats(5, 3, 10, 2, 15),
                new Look(
                    "Swarm",
                    "angler-fish",
                    "Questionable member of the sea that travels in schools.",
                    Breed.FISH
                    ),
                new Swarm())
                .AddSpells(new EnemyHeal())
                .AddItem(new Money(), Util.RandomRange(5, 10));
        }
    }
}