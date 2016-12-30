using UnityEngine;
using System.Collections;

public class Ruins : PageGenerator {
    private const string BATTLE_MUSIC = "Hero Immortal";

    public Ruins(Flags flags, Page camp, Party party) : base("Ruins", "The ruins of some fallen civilization bordering the edge of the world.") {

        encounters.Add(
            new Encounter(
                () => new BattlePage(party, camp, camp,
                BATTLE_MUSIC,
                "Ruins",
                "A human in a white priestly gown blocks your way. Their clothing is in perfect condition.",
                new Regenerator()),
                () => 0.15f)
            );

        encounters.Add(
                new Encounter(
                    () => new BattlePage(party, camp, camp,
                    BATTLE_MUSIC,
                    "Ruins",
                    "A human knight in white armor blocks your way and draws their sword. Their armor looks completely new and untouched.",
                 new Lasher()),
                 () => 0.35f)
            );

        encounters.Add(
                new Encounter(
                    () => new BattlePage(party, camp, camp,
                    BATTLE_MUSIC,
                    "Ruins",
                    "A knight and bishop block your way.",
                new Lasher(),
                new Regenerator()),
                () => 0.05f)
            );

        encounters.Add(
                new Encounter(
                () => new ReadPage(party,
                    "Deep in the ruins, you come across a lengthy bridge across the edge of the world. Way off in the distance,"
                    + " you see a temple aligned with the bridge.\nYou discovered the Temple!\n(Visit this location using [Places] back at camp.)",
                    "Ruins",
                    () => flags.Bools[Flag.DISCOVERED_TEMPLE] = true,
                    camp
                    ),
                () => flags.Bools[Flag.DISCOVERED_TEMPLE] ? 0.15f : 0)
            );

        encounters.Add(
                new Encounter(
                    () => new Shop(flags, camp, party),
                    () => 10.30f)
            );
    }
}
