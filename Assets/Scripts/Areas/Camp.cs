using UnityEngine;
using System.Collections;
using System;
using System.Linq;

public class Camp : Area {
    public Page Hub;
    public Page Places;
    public Page Stash;

    public Page DungeonStart;
    public Page[] Dungeon;

    public PlayerCharacter pc;
    public Alestre alestre;
    public Kitsune kits;

    public Camp(PlayerCharacter pc) {
        this.pc = pc;
        this.alestre = new Alestre();
        this.kits = new Kitsune();

        CreateCamp();
    }

    private void CreateCamp() {
        Hub = Rp(
            text: "You awaken in a grassy green field.",
            location: "Camp",
            processes: new Process[] {
                        new Process("<color=red>Explore</color>", "not implemented yet :("),
                        new Process("Places", "Visit an area you know.", () => Page = Places),
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
                        }),
            },
            onEnter: () => {
                if (kits.State == CharacterState.ALIVE) {
                    Game.Instance.AddTextBox(new TextBox("A path of black muck cuts through the peninsula from the shattered gate to somewhere behind you. It writhes at the surface."));
                }
            }
            );

        Places = Rp(
            text: "Behind you is a lengthy stone bridge. In a far away distance, you see a temple.",
            location: "Camp",
            tooltip: "Where will you go?",
            processes: new Process[] {
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
                new Process(),
                new Process("Back", "Return to the camp.", () => Page = Hub)
            },
            onEnter: () => {
                if (kits.State == CharacterState.ALIVE) {
                    Game.Instance.AddTextBox(new TextBox("The trail of muck is on the bridge."));
                }
            }
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
    Action onEnter = null,
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
                pc.Selections[Selection.MERCY].Add(new Flee(flee));
            },
            onExit: () => {
                pc.Selections[Selection.MERCY].Remove(new Flee(flee));
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
