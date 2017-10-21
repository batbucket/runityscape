using Scripts.Game.Defined.Serialized.Spells;
using Scripts.Game.Defined.Unserialized.Spells;
using Scripts.Game.Serialized.Brains;
using Scripts.Game.Shopkeeper;
using Scripts.Model.Characters;
using Scripts.Model.Pages;

namespace Scripts.Game.Defined.Characters {

    // HERO - REVIVE, some damage spell (finally)
    // PARTNER - INSPIRE, SELF HEAL
    public static class LabNPCs {

        public static Trainer Trainer(Page previous, Party party) {
            return new Trainer(
                    previous,
                    party,
                    Cultist(),
                    new PurchasedSpell(200, new Revive()),
                    new PurchasedSpell(200, new Inspire()),
                    new PurchasedSpell(500, new MagicMissile()),
                    new PurchasedSpell(500, new SelfHeal())
                );
        }

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

        private static Character BigKnight(string name) {
            return CharacterUtil.StandardEnemy(
                new Stats(15, 15, 10, 10, 120),
                new Look(
                    name,
                    "big-knight",
                    "One of a pair of knights known for their determination.",
                    Breed.SPIRIT
                    ),
                new Attacker()
                );
        }
    }
}