using Scripts.Model.Characters;
using Scripts.Model.Characters.Named;
using Scripts.Model.World.Pages;
using Scripts.Model.World.Utility;
using Scripts.Model.Pages;
using System;

namespace Scripts.Model.World.PageGenerators {

    public class Ruins : PageGenerator {
        private const string BATTLE_MUSIC = "Hero Immortal";

        private Page camp;
        private Party party;

        public Ruins(EventFlags flags, Page camp, Party party) : base("Ruins", "The ruins of some fallen civilization bordering the edge of the world.") {
            this.camp = camp;
            this.party = party;
            encounters.Add(
                TypicalBattle(
                    "A human in a white priestly gown blocks your way. Their clothing is in perfect condition.",
                    () => 0.10f,
                    new Regenerator()
                    )
                );

            encounters.Add(
                TypicalBattle(
                    "A human knight in white armor blocks your way, drawing their sword. Their armor looks completely new and untouched.",
                    () => 0.25f,
                    new Lasher()
                    )
            );

            encounters.Add(
                TypicalBattle(
                    "A human knight and bishop pair blokc the way.",
                    () => 0.05f,
                    new Lasher(),
                    new Regenerator()
                    )
            );

            encounters.Add(
                    new Encounter(
                    () => new ReadPage(party,
                        "Deep in the ruins, you come across a lengthy bridge across the edge of the world. Way off in the distance,"
                        + " you see a temple aligned with the bridge.\nYou discovered the Temple!\n(Visit this location using Places back at camp.)",
                        "Ruins",
                        () => flags.Bools[Flag.DISCOVERED_TEMPLE] = true,
                        camp
                        ),
                    () => 0.20f,
                    () => !flags.Bools[Flag.DISCOVERED_TEMPLE])
                );

            encounters.Add(
                    new Encounter(
                        () => new ShopPage(flags, camp, party),
                        () => 0.30f,
                        () => flags.Ints[Flag.SHOPKEEPER_STATUS] == Flag.SHOPKEEPER_NEUTRAL || flags.Ints[Flag.SHOPKEEPER_STATUS] == Flag.SHOPKEEPER_FRIENDLY)
                );

            encounters.Add(
                new Encounter(
                    () => new BattlePage(party,
                    new BattleResult(camp),
                    new BattleResult(camp),
                    "",
                    "Ruins",
                    "You come across Maple. She doesn't look too happy.", new Character[] { new Shopkeeper(flags) }),
                    () => 0.30f,
                    () => flags.Ints[Flag.SHOPKEEPER_STATUS] == Flag.SHOPKEEPER_ENEMY)
                );
        }

        private Encounter TypicalBattle(string text, Func<float> chance, Func<bool> isEnabled, params Character[] enemies) {
            return new Encounter(
                () => new BattlePage(party, new BattleResult(camp), new BattleResult(camp), BATTLE_MUSIC, "Ruins", text, enemies),
                chance,
                isEnabled);
        }

        private Encounter TypicalBattle(string text, Func<float> chance, params Character[] enemies) {
            return new Encounter(
                () => new BattlePage(party, new BattleResult(camp), new BattleResult(camp), BATTLE_MUSIC, "Ruins", text, enemies),
                chance);
        }
    }
}