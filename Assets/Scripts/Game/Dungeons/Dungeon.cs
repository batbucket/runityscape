using Scripts.Game.Defined.Characters;
using Scripts.Model.Characters;
using Scripts.Model.Interfaces;
using Scripts.Model.Pages;
using Scripts.Model.Processes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Dungeon : PageGroup {
    public Dungeon(Party party, Page defeat, Page previous, Page destination, Music music, string name, string description) : base(new Page(name)) {
        Root.Body = description;
        Root.Actions = new IButtonable[] {
            PageUtil.GenerateBack(previous),
            GetDungeonEnterProcess(party, defeat, destination, music)
        };
    }

    protected abstract Character[][] GetEnemyEncounters();

    private Process GetDungeonEnterProcess(Party party, Page defeat, Page destination, Music music) {
        Character[][] enemyEncounters = GetEnemyEncounters();
        Battle[] battles = new Battle[enemyEncounters.Length];
        battles[battles.Length - 1] = new Battle(
            defeat,
            destination,
            music,
            string.Format("{0} - {1}", Root.Location, battles.Length - 1),
            party,
            enemyEncounters[enemyEncounters.Length - 1]);
        for (int i = battles.Length - 2; i >= 0; i--) {
            battles[i] = new Battle(defeat, battles[i + 1], music, string.Format("{0} - {1}", Root.Location, i), party, enemyEncounters[i].Shuffle());
        }
        return new Process(
                "Enter",
                Util.GetSprite("dungeon-gate"),
                "Enter this dungeon.",
                () => {
                    battles[0].Invoke();
                }
            );
    }
}
