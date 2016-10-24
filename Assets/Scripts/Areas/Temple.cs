using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using Model.BattlePage;

public class Temple : Area {

    public Page PreBridge;
    public BattlePage RegeneratorFight;
    public Page Bridge;
    public BattlePage LasherFight;
    public Page TempleEntrance;
    public BattlePage TentaclePartyFight;
    public Page TempleHall;
    public Page TempleThrone;
    public BattlePage KitsFight;
    public Page KitsAftermath;

    public Page DungeonStart;
    public Page[] Dungeon;

    public PlayerCharacter pc;
    public Alestre alestre;
    public Kitsune kits;

    public Camp camp;

    public Temple(Camp camp) {
        this.camp = camp;
        this.pc = camp.pc;
        this.alestre = new Alestre();
        this.kits = new Kitsune();

        CreatePreBridge();
        CreateBridge();
        CreateTempleEntrance();
        CreateTemple();
    }

    private void CreatePreBridge() {
        Character[] PreBridgeEnemies = new Character[] { new Regenerator() };
        PreBridge = Rp(
            text: "Not a single column supports the bridge underneath."
            + " The blue sky shifts to a pure black across the horizon."
            + " You are truly at the edge of the world.",
            location: "Bridge Entrance",
            processes: new Process[] {
                new Process("Go back", action: () => Page = camp.Hub),
                new Process("Go to Bridge", action: () => Page = (RegeneratorFight.State == BattleState.VICTORY) ? Bridge : RegeneratorFight)
            },
            right: PreBridgeEnemies,
            onEnter:
            () => {
                if (PreBridgeEnemies.Any(c => c.State != CharacterState.KILLED)) {
                    Game.AddTextBox(new TextBox("A dark tentacle erupts from the muck at the entrance to the bridge."));
                } else {
                    Game.AddTextBox(new TextBox("The entrance to the bridge is clear."));
                }
            }
            );

        bool learnedSmite = false;
        RegeneratorFight = Bp(
            "Regenerator blocks the way!",
            location: "Bridge Entrance",
            musicLoc: "Hero Immortal",
            right: PreBridgeEnemies,
            flee: PreBridge,
            victory: PreBridge,
            onTick:
            () => {
                if (!learnedSmite && pc.Equipment.Contains(new OldArmor(1)) && pc.Equipment.Contains(new OldSword(1))) {
                    learnedSmite = true;
                    Game.Ordered(
                        new Event(
                            action: () => {
                                pc.AddResource(new NamedResource.Skill());
                                pc.Equipment.Add(new Smite());
                            }),
                        new Event(
                            t: new TextBox(string.Format("{0} recalled something.", pc.DisplayName))),
                        new Event(
                            t: new TextBox("<color=yellow>Spell Smite... Two Skill... Attack to gain...</color>", TextEffect.TYPE, timePerLetter: 0.05f), delay: 5.0f)
                        );
                }
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
                if (kits.State == CharacterState.ALIVE) {
                    Game.AddTextBox(new TextBox("The muck covers the entirety of the bridge path. It drips away from your boots the moment you lift up a foot."));
                }
                if (bridgeEnemies.Any(c => c.State != CharacterState.KILLED)) {
                    Game.AddTextBox(new TextBox("A dark tentacle adorned with spikes erupts in your way. It repeatedly lashes in your direction. You're too far away to be hit, for now..."));
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
                if (kits.State == CharacterState.ALIVE) {
                    Game.AddTextBox(new TextBox("The muck trail goes to the inside of the building..."));
                }

                if (tentacleParty.All(t => t.State != CharacterState.KILLED)) {
                    Game.AddTextBox(new TextBox("A Lasher and Regenerator erupt at the entrance..."));
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

    private void CreateTemple() {
        TempleHall = Rp(
            "The hall could not be in a poorer state. The stone walls and floor are all in a state of decay."
            + " Grasses and other plants shoot up from cracks in the once-solid stone floor."
            + " The colored-glass windows on the walls have shattered, streaming flecks of colored light onto the ground along with sunlight."
            + " The throne room is just up ahead, its entrance decorated by a cracked arch."
            + " You see no other doorways.",
            "Temple - Hall",
            new Process[] {
                                new Process("Exit Temple", action: () => Page = TempleEntrance),
                                new Process("Go to Throne", action: () => Page = TempleThrone)
            },
            onEnter: () => {
                Game.Instance.AddTextBox(
                    new TextBox(
                        "The muck trail widens the closer you go to the throne room, eventually covering the the whole floor."
                        + " The muck is much more active here, growing up the walls."
                        )
                        );
            }
            );
        this.kits.Name = "???";
        TempleThrone = Rp(
            "A throne is in a slightly better condition. The stairs begin in the middle of the room, ending at a platform with some sort of gateway.",
            "Temple - Throne",
            new Process[] {
                new Process("Hall", action: () => Page = TempleHall),
                new OneShotProcess("Approach", action: () => {
                    Game.Cutscene(
                            new Event(new TextBox("You manage to get one step in before it turns around.")),
                            new Event(new TextBox("Its hair matches its tail color, reaching down to its shoulders. Its skin is an ash-grey."
                            + " Its eyes are a glowing red, with the sclera being completely black.")),
                            new Event(new TextBox("Without a doubt, this creature is a kitsune fallen to corruption.")),
                            new Event(() => {
                                this.kits.Name = "Corrupt K.";
                                Game.Instance.Sound.Loop("FantasyWav");
                            }),
                            new Event(new RightBox(kits, "This is the human who's going to stop me?")),
                            new Event(new RightBox(kits, "I suppose it's better than getting <color=yellow>herself</color> corrupted, but still.")),
                            new Event(new TextBox("Kitsune takes a deep breath.")),
                            new Event(new RightBox(kits, "Let's get to the point.")),
                            new Event(new TextBox("The kitsune starts walking towards you.")),
                            new Event(new TextBox("Tentacles sprout behind you, sealing off the door!")),
                            new Event(() => Page = KitsFight)
                        );
                }, condition: () => KitsFight.State != BattleState.VICTORY)
            },
            "What will you do?",
            new Character[] { kits },
            onEnter: () => {
                if (kits.State == CharacterState.ALIVE) {
                    Game.Instance.AddTextBox(new TextBox("The whole room is covered in muck."));
                    Game.Instance.AddTextBox(
                        new TextBox(
                            "Something stands at the stairs, facing the gateway."
                        + " An uncountable number of black tendril-like tails sprout from its back, tall enough to tower over the being's head."
                        + " Its tails seem to be overflowing in muck, dripping onto the floor and contributing to the muck in the room."
                        + " It wears a white robe. You can barely make out a set of fox-like ears at the top of its head."
                        + " There's no doubt that it's 'the agent of corruption' Alestre warned you about."
                        )
                        );
                }
            }
            );

        KitsFight = Bp(
            "You are fighting Corrupted Kitsune!",
            "Temple - Throne",
            musicLoc: "Flicker",
            right: new Character[] { kits },
            victory: KitsAftermath
            );

        KitsAftermath = Rp(
            "You're winner!",
            "Temple - Throne",
            new Process[0]
            );
    }

    private ReadPage Rp(
        string text,
        string location,
        Process[] processes,
        string tooltip = "What will you do?",
        Character[] right = null,
        Action onEnter = null,
        Action onExit = null,
        Action onTick = null) {
        right = right ?? new Character[0];
        return new ReadPage(text, tooltip, location, mainCharacter: pc, left: new Character[] { pc }, right: right.Where(c => c.State != CharacterState.KILLED).ToArray(), onEnter: onEnter, onExit: onExit, onTick: onTick, processes: processes);
    }

    private BattlePage Bp(string text,
        string location,
        string musicLoc,
        Character[] right,
        Page flee,
        Page victory,
        Action onEnter =
        null,
        Action onTick = null) {
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
                pc.Mercies.Add(new Flee(flee));
            },
            onExit: () => {
                pc.Mercies.Remove(new Flee(flee));
            },
            onTick: onTick
            );
    }

    private BattlePage Bp(string text,
    string location,
    string musicLoc,
    Character[] right,
    Page victory,
    Action onEnter =
    null,
    Action onTick = null,
    Func<bool> victoryCondition = null) {
        return new BattlePage(
            text: text,
            location: location,
            musicLoc: musicLoc,
            mainCharacter: pc,
            left: new Character[] { pc },
            right: right,
            isVictory: victoryCondition,
            onEnter: () => {
                if (onEnter != null) {
                    onEnter.Invoke();
                }
            },
            onVictory: () => this.Page.ActionGrid = new Process[] { new Process("Continue", action: () => Page = victory) },
            onTick: onTick
            );
    }

    public override void Init() {
    }
}
