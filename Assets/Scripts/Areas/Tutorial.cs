using UnityEngine;
using System.Collections;
using System;

public class Tutorial : Area {
    public Page Greeting;
    public Page FakeCamp;
    public Page FakePlaces;
    public Page FakeStash;

    public Page PreBridge;
    public Page Regenerator;
    public Process BlockText;
    private bool regenDefeated;
    public Page Bridge;
    public Page TempleEntrance;
    public Page TempleHall;
    public Page TempleThrone;

    public Page DungeonStart;
    public Page[] Dungeon;

    public PlayerCharacter pc;
    public Alestre alestre;

    public Tutorial(PlayerCharacter pc) {
        this.pc = pc;
        this.alestre = new Alestre();

        Greeting = new ReadPage(
            text: "Pure darkness covers your surroundings, near and far. You can move your body but you cannot see it, or anything else.",
            mainCharacter: pc,
            left: new Character[] { pc },
            right: new Character[] { },
            processes: new Process[] {
                new OneShotProcess("Call out",
                "Speak into the darkness.",
                () => {
                    Game.Cutscene(
                        new Event(new TextBox("Press space to advance dialogue.")),
                        new Event(new LeftBox(pc, "H...hello?")),
                        new Event(new TextBox("A soft note hums in your ears.")),
                        new Event(new RightBox(alestre, string.Format("Greetings, {0}. I am {1}.", pc.Name, alestre.Name))),
                        new Event(new RightBox(alestre, "I am the observer assigned to this biosphere.")),
                        new Event(new RightBox(alestre, "You have been granted a second chance to serve your world.")),
                        new Event(new RightBox(alestre, "Enter the place known as Last Temple. There you will learn your purpose.")),
                        new Event(new RightBox(alestre, string.Format("Now, {0}, awaken from your slumber and begin your journey...", pc.Name))),
                        new Event(() => Page = FakeCamp)
                        );
                })
            }
            );

        FakeCamp = new ReadPage(
            text: "You awaken in a grassy green field. "
            + "The warm, soft grass has served as your bed. The sun glows above you."
            + " You are on some sort of small peninsula. Two hulking metal walls as high as the sky guard the attachment to the mainland, connected by the ruins of a giant gate."
            + " The walls curve around the field, ending where the land ends. Aside from one end, the land is surrounded by a sea of nothingness.",
            processes: new Process[] {
                new Process("<color=red>Explore</color>", "not implemented yet :("),
                new Process("Places", "Visit an area you know.", () => Page = FakePlaces),
                new Process(),
                new Process(),

                new Process(),
                new Process(),
                new Process(),
                new Process(),

                new Process(),
                new Process(),
                new Process(),
                new OneShotProcess("Mysterious Box", "What is this...?",
                () => {
                    Game.Instance.AddTextBox(new TextBox(string.Format("TimeCapsule was added to {0}'s Items.", pc.Name)));
                    pc.Selections[Selection.ITEM].Add(new TimeCapsule(1));
                })
            }
            );

        FakePlaces = new ReadPage(
            text: "Behind you, you can barely make out what appears to be a temple, further away on an island. A lengthy stone bridge goes in the same direction...",
            tooltip: "Where will you go?",
            processes: new Process[] {
                new Process("Temple", "Go to the temple.", () => Page = PreBridge),
                new Process(),
                new Process(),
                new Process(),

                new Process(),
                new Process(),
                new Process(),
                new Process(),

                new Process(),
                new Process(),
                new Process(),
                new Process("Back", "Return to the camp.", () => Page = FakeCamp)
            }
            );

        BlockText = new TextProcess(textBox: new TextBox("You dare not attempt to squeeze past the tentacle."));
        PreBridge = new ReadPage(
            text: "Not a single column supports the bridge underneath."
            + " The blue sky shifts to a pure black across the horizon."
            + " You are truly at the edge of the world.",
            processes: new Process[] {
                new Process("Cross", "Attempt to cross the bridge",
                () => {
                    if (regenDefeated) {
                        Page = Bridge;
                    } else {
                        BlockText.Play();
                    }
                    }
                    ),
                new Process("Fight", "Remove the tentacle by force.", () => Page = Regenerator, condition: () => !regenDefeated),
                new Process(),
                new Process(),

                new Process(),
                new Process(),
                new Process(),
                new Process(),

                new Process(),
                new Process(),
                new Process(),
                new Process("Go back", "Return to camp.", () => Page = FakeCamp)
            },
            onEnter:
            () => {
                if (!regenDefeated) {
                    Game.AddTextBox(new TextBox("A petrified tentacle is blocking the entrance to the bridge..."));
                } else {
                    Game.AddTextBox(new TextBox("The path to enter the bridge is clear."));
                }
            }
            );

        bool learnedSmite = false;
        Regenerator = new BattlePage(
            "Regenerator blocks the way!",
            mainCharacter: pc,
            musicLoc: "Hero Immortal",
            left: new Character[] { pc },
            right: new Character[] { new Regenerator() },
            onTick:
            () => {
                if (!learnedSmite && pc.Selections[Selection.EQUIP].Contains(new OldArmor(1)) && pc.Selections[Selection.EQUIP].Contains(new OldSword(1))) {
                    learnedSmite = true;
                    Game.Ordered(
                        new Event(
                            action: () => {
                                pc.AddResource(new NamedResource.Skill());
                                pc.Selections[Selection.SPELL].Add(new Smite());
                            }),
                        new Event(
                            t: new TextBox(string.Format("{0} recalled something.", pc.DisplayName))),
                        new Event(
                            t: new TextBox("<color=yellow>Spell Smite... Two Skill... Attack to gain...</color>", TextEffect.TYPE, timePerLetter: 0.05f), delay: 5.0f)
                        );
                }
            },
            onVictory: () => {
                regenDefeated = true;
                Regenerator.ActionGrid = new Process[] { new Process("Continue", action: () => Page = PreBridge) };
            },
            onEnter: () => {
                pc.Selections[Selection.MERCY].Add(new Flee(PreBridge));
                (new Petrify()).Cast(pc, Page.GetEnemies(pc)[0]);
            },
            onExit: () => pc.Selections[Selection.MERCY].Remove(new Flee(PreBridge))
            );
    }

    public override void Init() {

    }
}
