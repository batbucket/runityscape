using UnityEngine;
using System.Collections;
using System;
using System.Linq;

public class Tutorial : Area {
    public Page Greeting;
    public Page FakeCamp;
    public Page FakePlaces;
    public Page FakeStash;

    public Page PreBridge;
    public BattlePage RegeneratorFight;
    public Page Bridge;
    public BattlePage LasherFight;
    public Page TempleEntrance;
    public BattlePage TentaclePartyFight;
    public Page TempleHall;
    public Page TempleThrone;

    public Page DungeonStart;
    public Page[] Dungeon;

    public PlayerCharacter pc;
    public Alestre alestre;

    public Tutorial(PlayerCharacter pc) {
        this.pc = pc;
        this.alestre = new Alestre();

        Greeting = Rp(
            text: "Pure darkness covers your surroundings, near and far. You can move your body but you cannot see it, or anything else.",
            location: "Unknown",
            tooltip: "Press space to advance dialog.",
            right: new Character[] { },
            processes: new Process[] {
                                new OneShotProcess("Call out",
                                "Speak into the darkness.",
                                () => {
                                    Game.Cutscene(
                                        new Event(new LeftBox(pc, "H...hello?")),
                                        new Event(new TextBox("A soft note hums in your ears.")),
                                        new Event(new RightBox(alestre, string.Format("Greetings, {0}. I am {1}.", pc.Name, alestre.Name))),
                                        new Event(new RightBox(alestre, "I am the observer assigned to this world.")),
                                        new Event(new RightBox(alestre, "You have been granted a second chance to serve your world.")),
                                        new Event(new RightBox(alestre, "Enter the place known as Last Temple. There you will learn your purpose.")),
                                        new Event(new RightBox(alestre, string.Format("Now, {0}, awaken from your slumber and begin your journey...", pc.Name))),
                                        new Event(() => Page = FakeCamp)
                                        );
                                })
            }
            );

        CreateFakeCamp();
        CreatePreBridge();
        CreateBridge();
        CreateTempleEntrance();

        TempleHall = Rp(
            "Sample text",
            location: "Temple - Hall",
            processes: new Process[] {
                        new Process("Exit", action: () => Page = TempleEntrance),
                        new Process("Throne", action: () => Page = TempleThrone)
            }
            );

        TempleThrone = Rp(
            "Sample text",
            location: "Temple - Throne",
            processes: new Process[] {
                new Process("Hall", action: () => Page = TempleHall),
                new Process("Talk")
            }
            );
    }

    private void CreateFakeCamp() {
        FakeCamp = Rp(
            text: "You awaken in a grassy green field. "
            + "The warm, soft grass has served as your bed. The sun glows above you."
            + " You are on some sort of small peninsula. Two hulking metal walls as high as the sky guard the attachment to the mainland, connected by the ruins of a giant gate."
            + " The walls curve around the field, ending where the land ends. Aside from one end, the land is surrounded by a sea of nothingness.",
            location: "Camp",
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

        FakePlaces = Rp(
            text: "Behind you, you can barely make out what appears to be a temple, further away on an island. A lengthy stone bridge goes in the same direction...",
            location: "Camp",
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
    }

    private void CreatePreBridge() {
        Character[] PreBridgeEnemies = new Character[] { new Regenerator() };
        PreBridge = Rp(
            text: "Not a single column supports the bridge underneath."
            + " The blue sky shifts to a pure black across the horizon."
            + " You are truly at the edge of the world.",
            location: "Bridge Entrance",
            processes: new Process[] {
                new Process("Go back", action: () => Page = FakeCamp),
                new Process("Go to Bridge", action: () => Page = (RegeneratorFight.State == BattleState.VICTORY) ? Bridge : RegeneratorFight)
            },
            right: PreBridgeEnemies,
            onEnter:
            () => {
                if (PreBridgeEnemies.Any(c => c.State != CharacterState.KILLED)) {
                    Game.AddTextBox(new TextBox("A black-purple tentacle blocks the entrance to the bridge. It seems frozen in time."));
                } else {
                    Game.AddTextBox(new TextBox("The path to enter the bridge is clear."));
                }
            }
            );

        bool learnedSmite = false;
        RegeneratorFight = Bp(
            "Regenerator blocks the way!",
            location: "Bridge Entrance",
            musicLoc: "Hero Immortal",
            right: PreBridgeEnemies,
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
            flee: PreBridge,
            victory: PreBridge,
            onEnter: () => {
                (new Petrify()).Cast(pc, Page.GetEnemies(pc)[0]);
            }
            );
    }

    private void CreateBridge() {
        Character[] bridgeEnemies = new Character[] { new Lasher() };
        Bridge = Rp(
            "The bridge goes on for a seemingly infinite distance. Wind gusts into the bridge again and again, making it rumble.",
            location: "Bridge",
            processes: new Process[] {
                        new Process("Go towards Camp", action: () => Page = PreBridge),
                        new Process("Go towards Temple", action: () => Page = (LasherFight.State == BattleState.VICTORY) ? TempleEntrance : LasherFight)
            },
            right: bridgeEnemies,
            onEnter: () => {
                if (bridgeEnemies.Any(c => c.State != CharacterState.KILLED)) {
                    Game.AddTextBox(new TextBox("A pure black tentacle adorned with spikes is blocking your way. It repeatedly whips in your direction. Luckily, you're too far away, for now..."));
                } else {
                    Game.AddTextBox(new TextBox("The bridge path is clear."));
                }
            }
            );

        LasherFight = Bp(
            "Lasher blocks the way!",
            location: "Bridge",
            musicLoc: "Hero Immortal",
            right: bridgeEnemies,
            flee: Bridge,
            victory: Bridge
            );
    }

    private void CreateTempleEntrance() {
        Tentacle[] tentacleParty = new Tentacle[] { new Lasher(), new Regenerator() };
        TempleEntrance = Rp(
            "The temple seems to be in a poor condition. Rain has taken its toll on the material. Despite all this, the temple is still impressive. It towers to the skies. You can barely see the top.",
            location: "Temple - Entrance",
            processes: new Process[] {
                        new Process("Go to Bridge", action: () => Page = Bridge),
                        new Process("Enter Temple", action: () => Page = (TentaclePartyFight.State == BattleState.VICTORY) ? TempleHall : TentaclePartyFight)
            },
            right: tentacleParty,
            onEnter: () => {
                if (tentacleParty.All(t => t.State != CharacterState.KILLED)) {
                    Game.AddTextBox(new TextBox("A Lasher and Regenerator block the entrance..."));
                } else {
                    Game.AddTextBox(new TextBox("The temple entrance is clear."));
                }
            }
            );
        TentaclePartyFight = Bp(
            text: "A Lasher and Regenerator block the way!",
            location: "Temple - Entrance",
            musicLoc: "Hero Immortal",
            right: tentacleParty,
            flee: TempleEntrance,
            victory: TempleEntrance
            );
    }

    private ReadPage Rp(string text, string location, Process[] processes = null, string tooltip = "What will you do?", Character[] right = null, Action onEnter = null, Action onExit = null, Action onTick = null) {
        right = right ?? new Character[0];
        return new ReadPage(text, tooltip, location, mainCharacter: pc, left: new Character[] { pc }, right: right.Where(c => c.State != CharacterState.KILLED).ToArray(), onEnter: onEnter, onExit: onExit, onTick: onTick, processes: processes);
    }

    private BattlePage Bp(string text, string location, string musicLoc, Character[] right, Page flee, Page victory, Action onEnter = null, Action onTick = null) {
        return new BattlePage(
            text: text,
            location: location,
            musicLoc: musicLoc,
            mainCharacter: pc,
            left: new Character[] { pc },
            right: right,
            onVictory: () => this.Page.ActionGrid = new Process[] { new Process("Continue", action: () => Page = victory) },
            onEnter: () => {
                if (onEnter != null) {
                    onEnter.Invoke();
                }
                pc.Selections[Selection.MERCY].Add(new Flee(flee));
            },
            onExit: () => {
                pc.Selections[Selection.MERCY].Remove(new Flee(flee));
            },
            onTick: onTick
            );
    }

    public override void Init() {
    }
}
