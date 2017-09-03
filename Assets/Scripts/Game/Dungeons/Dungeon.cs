using Scripts.Game.Defined.Characters;
using Scripts.Game.Serialized;
using Scripts.Model.Characters;
using Scripts.Model.Interfaces;
using Scripts.Model.Pages;
using Scripts.Model.Processes;
using Scripts.Model.TextBoxes;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Scripts.Game.Dungeons {
    public class Dungeon : PageGroup, IButtonable {
        private readonly Func<Encounter[]> encounterGenerator;
        private readonly Action onClear;

        public Dungeon(
            Party party, 
            Page defeat, 
            Page previous, 
            Page destination,
            string name,
            string description, 
            Func<Encounter[]> encounterGenerator,
            Action onClear) : base(new Page(name)) {
            Root.Body = description;
            this.encounterGenerator = encounterGenerator;
            this.onClear = onClear;
            Root.AddCharacters(Side.LEFT, party);

            Root.Actions = new IButtonable[] {
                PageUtil.GenerateBack(previous),
                GetDungeonEnterProcess(party, defeat, destination, onClear)
            };
        }

        private Process GetDungeonEnterProcess(Party party, Page defeat, Page destination, Action onClear) {
            Encounter[] encounters = encounterGenerator();
            Battle[] battles = new Battle[encounters.Length];

            for (int i = battles.Length - 1; i >= 0; i--) {
                Encounter encounter = encounters[i];

                encounter.Enemies.Shuffle();
                Page victoryDestination = null;
                if (i == battles.Length - 1) {
                    Page results = GetResults(destination, battles);
                    results.OnEnter += onClear;
                    victoryDestination = results;
                } else {
                    victoryDestination = battles[i + 1];
                }
                battles[i] = new Battle(defeat, victoryDestination, encounter.Music, string.Format("{0} - {1}", Root.Location, i), party, encounter.Enemies);
            }
            return new Process(
                    "Enter",
                    Util.GetSprite("dungeon-gate"),
                    "Enter this stage.",
                    () => {
                        battles[0].Invoke();
                    }
                );
        }

        private Page GetResults(Page destination, Battle[] battles) {
            Page results = new Page(string.Format("{0} - {1}", Root.Location, "Results"));
            results.Actions = new IButtonable[] { destination };
            results.OnEnter = () => {
                results.AddText(new TextBox(string.Format(
                    "{0} was cleared in {1} turns.\nTotal experience gained: {1}.",
                      Root.Location,
                      battles.Sum(b => b.TurnCount + 1),
                      battles.Sum(b => b.ExperienceGiven)
                      )));
            };
            return results;
        }
    }
}
