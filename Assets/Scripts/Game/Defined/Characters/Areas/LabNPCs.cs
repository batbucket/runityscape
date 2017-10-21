using Scripts.Game.Defined.Serialized.Buffs;
using Scripts.Game.Defined.Serialized.Spells;
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
    }

    public static class Ocean {
    }
}