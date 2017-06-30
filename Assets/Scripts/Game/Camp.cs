
using Scripts.Game.Defined.Characters;
using Scripts.Game.Defined.Serialized.Statistics;
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

    public class Camp : PageGroup {
        private Flags flags;
        private Party party;

        private bool shouldAdvanceTime;

        public Camp(Party party, Flags flags) : base(new Page("Campsite")) {
            this.party = party;
            this.flags = flags;
            SetupCamp();
        }

        private void SetupCamp() {
            Page p = Get(ROOT_INDEX);
            p.OnEnter = () => {
                p.AddCharacters(Side.LEFT, party.Collection);
                p.Actions = new IButtonable[] {
                new ExplorePages(p, flags, ref shouldAdvanceTime),
                new LevelUpPages(Root, party.Default),
                new InventoryPages(p, party.Default, party.shared),
                new EquipmentPages(p, new SpellParams(party.Default)),
                RestProcess(p),
                new SavePages(p, party, flags)
                };
                if (shouldAdvanceTime) {
                    AdvanceTime(p);
                }
                PostTime(p);
            };
        }

        private void PostTime(Page current) {
            current.AddText(string.Format("{0} of day {1}.", flags.Time.GetDescription(), flags.DayCount));
            if (flags.Time == TimeOfDay.NIGHT) {
                current.AddText("It is too dark to leave camp.");
            }
        }

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
                            CampUtil.RestoreStats(type, c.Stats, isLastTime);
                        }
                        if (isLastTime) {
                            CampUtil.CureMostBuffs(c.Buffs);
                        }
                    }
                    if (isLastTime) {
                        flags.DayCount %= int.MaxValue;
                        flags.DayCount++;
                    }
                    flags.Time = times[newIndex];
                    current.AddText(string.Format("The party {0}s.", isLastTime ? "rest" : "sleep"));
                    current.OnEnter();
                }
                );
        }

        private void AdvanceTime(Page p) {
            TimeOfDay[] times = Util.EnumAsArray<TimeOfDay>();
            int currentIndex = (int)flags.Time;
            int newIndex = (currentIndex + 1) % times.Length;
            flags.Time = times[newIndex];
            p.AddText("Some time has passed.");
        }
    }

}