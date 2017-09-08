using Scripts.Game.Defined.Serialized.Statistics;
using Scripts.Model.Characters;
using Scripts.Model.Interfaces;
using Scripts.Model.Pages;
using Scripts.Model.Processes;
using Scripts.Model.Stats;
using Scripts.Model.TextBoxes;
using System.Collections.Generic;

namespace Scripts.Game.Pages {
    public class LevelUpPages : PageGroup {

        public LevelUpPages(Page previous, Party party) : base(new Page("Party")) {
            SetupRoot(previous, party);
        }

        private void SetupRoot(Page previous, Party party) {
            Page p = Get(ROOT_INDEX);
            p.AddCharacters(Side.LEFT, party.Collection);
            p.Icon = Util.GetSprite("person");

            List<IButtonable> buttons = new List<IButtonable>();
            buttons.Add(PageUtil.GenerateBack(previous));
            foreach (Character partyMember in party) {
                buttons.Add(GetSpecificCharacterStats(partyMember, p));
            }
            p.Actions = buttons;
        }

        private Page GetSpecificCharacterStats(Character c, Page previous) {
            Page p = new Page(string.Format("{0}", c.Look.DisplayName));
            p.AddCharacters(Side.LEFT, c);
            p.Icon = c.Look.Sprite;
            p.OnEnter = () => {
                List<IButtonable> actions = new List<IButtonable>();
                actions.Add(PageUtil.GenerateBack(previous));
                actions.Add(GetSendPlayerToPointAllocationPageProcess(c, p));
                if (Util.IS_DEBUG) {
                    actions.Add(ExpHack(c));
                }
                DisplayStats(p, c);
                LevelUpPage(c, p);
                p.Actions = actions.ToArray();
            };
            return p;
        }

        private void DisplayStats(Page p, Character c) {
            int level = c.Stats.Level;
            int diffToLevelUp = c.Stats.GetStatCount(Stats.Get.MAX, StatType.EXPERIENCE) - c.Stats.GetStatCount(Stats.Get.MOD, StatType.EXPERIENCE);
            int statPoints = c.Stats.StatPoints;
            p.AddText(c.Stats.LongAttributeDistribution);
            p.AddText(string.Format("{0} Experience until level up.", diffToLevelUp));
            p.AddText(string.Format("{0} Stat Point(s) to allocate.", statPoints));
        }

        private Process ExpHack(Character c) {
            return new Process(
                "Exp HACK!",
                () => c.Stats.AddToStat(StatType.EXPERIENCE, Stats.Set.MOD_UNBOUND, 10)
                );
        }

        private Process GetSendPlayerToPointAllocationPageProcess(Character c, Page previous) {
            return new Process(
                    string.Format("Allocate ({0})",
                        c.Stats.StatPoints),
                    "Allocate stat points.",
                    () => LevelUpPage(c, previous).Invoke()
                    );
        }

        private Page LevelUpPage(Character c, Page previous) {
            Page p = new Page(string.Format("{0}'s stat allocation", c.Look.DisplayName));
            p.AddCharacters(Side.LEFT, c);
            p.OnEnter = () => {
                LetUserSelectAStatToIncrease(previous, p, c);
            };
            return p;
        }

        private void LetUserSelectAStatToIncrease(Page previous, Page current, Character c) {
            List<IButtonable> actions = new List<IButtonable>();
            actions.Add(PageUtil.GenerateBack(previous));

            foreach (StatType st in StatType.ASSIGNABLES) {
                if (c.Stats.HasStat(st)) {
                    actions.Add(
                        AddStatProcess(current, c.Stats, st)
                        );
                }
            }

            if (c.Stats.StatPoints > 0) {
                current.AddText(string.Format("Select a stat to increase.\nPoints remaining: {0}.", c.Stats.StatPoints));
            } else {
                current.AddText(string.Format("{0} has no stat points to spend.", c.Look.DisplayName));
            }
            current.Actions = actions.ToArray();
        }

        private Process AddStatProcess(Page p, Stats stats, StatType st) {
            return new Process(
                st.ColoredName,
                st.Sprite,
                string.Format("Add {0} point(s) in\n{1}\n{2}",
                    st.StatPointIncreaseAmount,
                    st.ColoredName,
                    st.Description),
                () => {
                    stats.StatPoints--;
                    stats.AddToStat(st, Stats.Set.MAX, st.StatPointIncreaseAmount);
                    p.AddText(string.Format("Maximum {0} was increased to {1}.\nPoints remaining: {2}.", st.Name, stats.GetStatCount(Stats.Get.MAX, st), stats.StatPoints));
                },
                () => stats.StatPoints > 0
                );
        }
    }
}