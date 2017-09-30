using Scripts.Game.Defined.Characters;
using Scripts.Game.Defined.Serialized.Statistics;
using Scripts.Game.Dungeons;
using Scripts.Game.Serialized;
using Scripts.Game.Undefined.Characters;
using Scripts.Model.Acts;
using Scripts.Model.Buffs;
using Scripts.Model.Characters;
using Scripts.Model.Interfaces;
using Scripts.Model.Pages;
using Scripts.Model.Processes;
using Scripts.Model.Spells;
using Scripts.Model.Stats;
using Scripts.Model.TextBoxes;
using Scripts.Presenter;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.Game.Pages {

    /// <summary>
    /// The hub of the game, from which all other parts can be visited.
    /// </summary>
    public class Camp : Model.Pages.PageGroup {

        /// <summary>
        /// Missing percentage to restore when resting
        /// </summary>
        private const float MISSING_REST_RESTORE_PERCENTAGE = .2f;

        private Flags flags;
        private Party party;

        /// <summary>
        /// Main
        /// </summary>
        /// <param name="party">Party for this particular game.</param>
        /// <param name="flags">Flags for this particular game.</param>
        public Camp(Party party, Flags flags) : base(new Page("Campsite")) {
            this.party = party;
            this.flags = flags;
            SetupCamp();
        }

        /// <summary>
        /// Setup on enter events.
        /// </summary>
        private void SetupCamp() {
            Page root = Get(ROOT_INDEX);
            root.OnEnter = () => {
                root.Location = flags.CurrentArea.GetDescription();

                // If this isn't first Resting will advance to wrong time
                if (flags.ShouldAdvanceTimeInCamp) {
                    AdvanceTime(root);
                    flags.ShouldAdvanceTimeInCamp = false;
                }

                foreach (Character partyMember in party.Collection) {
                    if (partyMember.Stats.HasUnassignedStatPoints) {
                        root.AddText(
                            string.Format(
                                "<color=cyan>{0}</color> has unallocated stat points. Points can be allocated in the <color=yellow>Party</color> page.",
                                partyMember.Look.DisplayName));
                    }
                }

                Model.Pages.PageGroup dungeonSelectionPage = new StagePages(root, party, flags);

                root.AddCharacters(Side.LEFT, party.Collection);
                root.Actions = new IButtonable[] {
                dungeonSelectionPage,
                new PlacePages(root, flags, party),
                new WorldPages(root, flags, party),
                new LevelUpPages(Root, party),
                new InventoryPages(root, party),
                new EquipmentPages(root, party),
                RestProcess(root),
                new SavePages(root, party, flags)
                };

                PostTime(root);
            };
        }

        /// <summary>
        /// Posts the time onto the textholder.
        /// </summary>
        /// <param name="current"></param>
        private void PostTime(Page current) {
            current.AddText(string.Format("{0} of day {1}.", flags.Time.GetDescription(), flags.DayCount));
            if (flags.Time == TimeOfDay.NIGHT) {
                current.AddText("It is too dark to leave camp.");
            }
        }

        /// <summary>
        /// Creates the process for resting.
        /// </summary>
        /// <param name="current">Current page</param>
        /// <returns>A rest process.</returns>
        private Process RestProcess(Page current) {
            TimeOfDay[] times = Util.EnumAsArray<TimeOfDay>();
            int currentIndex = (int)flags.Time;
            int newIndex = (currentIndex + 1) % times.Length;
            bool isLastTime = (currentIndex == (times.Length - 1));
            return new Process(
                isLastTime ? "Sleep" : "Rest",
                isLastTime ? Util.GetSprite("bed") : Util.GetSprite("health-normal"),
                isLastTime ? string.Format("Sleep to the next day ({0}).\nFully restores most stats and removes most status conditions.", flags.DayCount + 1)
                    : string.Format("Take a short break, advancing the time of day to {0}.\nSomewhat restores most stats.", times[newIndex].GetDescription()),
                () => {
                    foreach (Character c in party) {
                        foreach (StatType type in StatType.RESTORED) {
                            c.Stats.RestoreResourcesByMissingPercentage(isLastTime ? 1 : MISSING_REST_RESTORE_PERCENTAGE);
                        }
                        if (isLastTime) {
                            c.Buffs.DispelAllBuffs();
                        }
                    }
                    if (isLastTime) {
                        flags.DayCount %= int.MaxValue;
                        flags.DayCount++;
                    }
                    flags.Time = times[newIndex];
                    current.AddText(string.Format("The party {0}s.", isLastTime ? "sleep" : "rest"));
                    current.OnEnter();
                }
                );
        }

        /// <summary>
        /// Makes time go forward in camp. From visiting places.
        /// </summary>
        /// <param name="current">The current page.</param>
        private void AdvanceTime(Page current) {
            TimeOfDay[] times = Util.EnumAsArray<TimeOfDay>();
            int currentIndex = (int)flags.Time;
            int newIndex = (currentIndex + 1) % times.Length;
            flags.Time = times[newIndex];
            current.AddText("Some time has passed.");
        }
    }
}