using Scripts.Game.Defined.Serialized.Buffs;
using Scripts.Game.Defined.Serialized.Items;
using Scripts.Game.Defined.Serialized.Spells;
using Scripts.Game.Defined.Serialized.Statistics;
using Scripts.Game.Defined.Unserialized.Buffs;
using Scripts.Game.Defined.Unserialized.Spells;
using Scripts.Game.Serialized.Brains;
using Scripts.Game.Shopkeeper;
using Scripts.Model.Characters;
using Scripts.Model.Pages;

namespace Scripts.Game.Defined.Characters {

    public static class LabNPCs {

        public static Trainer Trainer(Page previous, Party party) {
            return new Trainer(
                    previous,
                    party,
                    Ruins.Cultist(),
                    new PurchasedSpell(200, new Revive()),
                    new PurchasedSpell(200, new Inspire()),
                    new PurchasedSpell(500, new MagicMissile()),
                    new PurchasedSpell(500, new SelfHeal())
                );
        }

        public static class Ruins {

            public static Character Cultist() {
                return CharacterUtil.StandardEnemy(
                    new Stats(10, 8, 5, 1, 25),
                    new Look("Spectre",
                             "villager",
                             "A not-so-innocent villager.",
                             Breed.SPIRIT),
                    new Attacker()
                    );
            }

            public static Character Enforcer() {
                return CharacterUtil.StandardEnemy(
                    new Stats(12, 12, 8, 5, 80),
                    new Look("Enforcer",
                             "knight",
                             "Augmented knight.",
                             Breed.SPIRIT),
                    new LabKnight()
                    )
                    .AddSpells(new CrushingBlow());
            }

            public static Character Darkener() {
                return CharacterUtil.StandardEnemy(
                    new Stats(12, 5, 20, 15, 50),
                    new Look("Darkener", "illusionist", "Powerful illusionist.", Breed.SPIRIT),
                    new Illusionist()
                    )
                    .AddSpells(new Blackout());
            }

            public static Character BigKnightA() {
                return BigKnight("Perse");
            }

            public static Character BigKnightB() {
                return BigKnight("Verance");
            }

            public static Character Mage() {
                return CharacterUtil.StandardEnemy(
                        new Stats(12, 4, 20, 20, 40),
                        new Look("Warlock",
                                 "wizard",
                                 "Hello",
                                 Breed.SPIRIT),
                        new Warlock()
                    ).AddSpells(new Inferno())
                    .AddBuff(new UnholyInsight());
            }

            public static Character Cleric() {
                return CharacterUtil.StandardEnemy(
                        new Stats(12, 4, 20, 20, 40),
                        new Look("Cleric",
                                 "white-mage",
                                 "Hello",
                                 Breed.SPIRIT),
                        new Cleric()
                    ).AddSpells(new SetupDefend(), new PlayerHeal())
                    .AddBuff(new UnholyInsight());
            }

            private static Character BigKnight(string name) {
                return CharacterUtil.StandardEnemy(
                    new Stats(15, 15, 10, 10, 120),
                    new Look(
                        name,
                        "big-knight",
                        "One of a pair of knights known for their determination.",
                        Breed.SPIRIT
                        ),
                    new LabBigKnight()
                    ).AddFlags(Flag.PERSISTS_AFTER_DEFEAT)
                    .AddSpells(new UnholyRevival());
            }
        }

        public static class Ocean {

            public static Character Shark() {
                return CharacterUtil.StandardEnemy(
                    new Stats(5, 5, 6, 8, 60),
                    new Look(
                        "Razor Shark",
                        "shark",
                        "Shark who needs lotion.",
                        Breed.FISH
                        ),
                    new Attacker())
                    .AddBuff(new RougherSkin())
                    .AddItem(new Money(), Util.RandomRange(50, 100));
            }

            public static Character Siren() {
                return CharacterUtil.StandardEnemy(
                        new Stats(6, 4, 10, 10, 40),
                        new Look(
                            "Enthraller",
                            "siren",
                            "Sings a mean tune.",
                            Breed.FISH
                        ),
                        new Siren()
                    ).AddSpells(Game.Serialized.Brains.Siren.DEBUFF_LIST);
            }

            public static Character Tentacle() {
                return CharacterUtil.StandardEnemy(
                        new Stats(7, 3, 25, 1, 20),
                        new Look(
                            "Lasher",
                            "shark",
                            "Tentacle belonging to a Kraken.",
                            Breed.FISH
                            ),
                        new Attacker()
                    )
                    .AddBuff(new RoughestSkin());
            }

            public static Character Kraken() {
                return CharacterUtil.StandardEnemy(
                        new Stats(8, 10, 10, 20, 200),
                        new Look(
                            "Octavio",
                            "shark",
                            "Giant squid thing. Commonly mistaken for a DJ.",
                            Breed.FISH
                            ),
                        new Kraken()
                    )
                    .AddSpells(new SpawnTentacles())
                    .AddSpells(new CrushingBlow())
                    .AddStats(new Skill());
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
                    new Elemental())
                    .AddStats(new Mana())
                    .AddSpells(new WaterboltSingle(), new WaterboltMulti())
                    .AddBuff(new UnholyInsight());
            }

            public static Character DreadSinger() {
                return CharacterUtil.StandardEnemy(
                        new Stats(10, 5, 20, 20, 25),
                        new Look(
                            "Sea Witch",
                            "siren",
                            "Singer of the voices of eternal death.",
                            Breed.FISH
                            ),
                        new DreadSinger())
                        .AddSpells(new CastDelayedEternalDeath())
                        .AddItem(new Cleansing(), 1);
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
                    .AddItem(new Money(), Util.RandomRange(5, 10));
            }
        }
    }
}