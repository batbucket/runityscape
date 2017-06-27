using Scripts.Game.Defined.Serialized.Statistics;
using Scripts.Model.Characters;
using Scripts.Model.Interfaces;
using Scripts.Model.Pages;
using Scripts.Model.Processes;
using Scripts.Model.Stats;
using Scripts.Model.TextBoxes;
using System.Collections.Generic;

namespace Scripts.Game.Pages {
    public class LevelUp : PageGroup {
        private const int LEVEL_UP_STAT_SELECTION = 1;

        public LevelUp(Page previous, Character c) : base(new Page(c.Look.DisplayName)) {
            Register(LEVEL_UP_STAT_SELECTION, new Page(c.Look.DisplayName));
            SetupRoot(previous, c);

            foreach (Page p in AllPages) {
                p.AddCharacters(Side.LEFT, c);
            }
        }

        private void SetupRoot(Page previous, Character c) {
            Page p = Get(ROOT_INDEX);
            p.OnEnter = () => {
                List<IButtonable> actions = new List<IButtonable>();
                actions.Add(previous);
                actions.Add(GetSendPlayerToPointAllocationPageProcess(c));
                actions.Add(ExpHack(c));
                DisplayStats(p, c);
                LevelUpPage(c);
                p.Actions = actions.ToArray();
            };
        }

        private void DisplayStats(Page p, Character c) {
            int level = c.Stats.Level;
            int diffToLevelUp = c.Stats.GetStatCount(Stats.Get.MAX, StatType.EXPERIENCE) - c.Stats.GetStatCount(Stats.Get.MOD, StatType.EXPERIENCE);
            int statPoints = c.Stats.StatPoints;
            p.AddText(c.Stats.LongAttributeDistribution);
            p.AddText(string.Format("{0} Experience until level up.", diffToLevelUp));
            p.AddText(string.Format("{0} stat points to allocate.", statPoints));
        }

        private Process ExpHack(Character c) {
            return new Process(
                "Exp HACK!",
                () => c.Stats.AddToStat(StatType.EXPERIENCE, Stats.Set.MOD_UNBOUND, 10)
                );
        }

        private Process GetSendPlayerToPointAllocationPageProcess(Character c) {
            return new Process(
                    string.Format("Allocate ({0})",
                        c.Stats.StatPoints),
                    "Allocate stat points.",
                    () => Get(LEVEL_UP_STAT_SELECTION).Invoke()
                    );
        }

        private void LevelUpPage(Character c) {
            Page p = Get(LEVEL_UP_STAT_SELECTION);
            p.AddCharacters(Side.LEFT, c);
            p.OnEnter = () => {
                LetUserSelectAStatToIncrease(p, c);
            };
        }

        private void LetUserSelectAStatToIncrease(Page p, Character c) {
            List<IButtonable> actions = new List<IButtonable>();
            actions.Add(Process.GetBackProcess(Get(ROOT_INDEX)));

            foreach (StatType st in StatType.ASSIGNABLES) {
                if (c.Stats.HasStat(st)) {
                    actions.Add(
                        AddStatProcess(p, c.Stats, st)
                        );
                }
            }

            if (c.Stats.StatPoints > 0) {
                p.AddText(string.Format("Select a stat to increase.\nPoints remaining: {0}.", c.Stats.StatPoints));
            } else {
                p.AddText(string.Format("{0} has no stat points to spend.", c.Look.DisplayName));
            }
            p.Actions = actions.ToArray();
        }

        private Process AddStatProcess(Page p, Stats stats, StatType st) {
            return new Process(
                st.Name,
                st.Sprite,
                string.Format("Add a point in\n{0}\n{1}",
                    st.Name,
                    st.Description),
                () => {
                    stats.StatPoints--;
                    stats.AddToStat(st, Stats.Set.MAX, 1);
                    p.AddText(string.Format("Maximum {0} was increased to {1}.\nPoints remaining: {2}.", st.Name, stats.GetStatCount(Stats.Get.MAX, st), stats.StatPoints));
                },
                () => stats.StatPoints > 0
                );
        }
    }
}