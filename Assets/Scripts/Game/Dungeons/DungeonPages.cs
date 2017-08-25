using Scripts.Game.Serialized;
using Scripts.Model.Characters;
using Scripts.Model.Interfaces;
using Scripts.Model.Pages;
using Scripts.Model.Processes;
using System.Collections.Generic;

namespace Scripts.Game.Dungeons {
    public class DungeonPages : PageGroup {
        private readonly Party party;
        private readonly Flags flags;
        private readonly Page previous;

        public DungeonPages(Page previous, Party party, Flags flags) : base(new Page("Quest")) {
            var buttons = new List<IButtonable>();
            this.party = party;
            this.flags = flags;
            this.previous = previous;   

            Root.Icon = Util.GetSprite("dungeon-gate");
            Root.AddCharacters(Side.LEFT, party);
            Root.Condition = PageUtil.GetVisitProcessCondition(flags, party);

            buttons.Add(PageUtil.GenerateBack(previous));

            Area area = GetCurrentArea(flags);

            for (int i = 0; i < area.Dungeons.Length; i++) {
                if (IsStagePlayable(i, area)) {
                    buttons.Add(GetDungeonEntryProcess(i, area));
                } else {
                    buttons.Add(new Process("<color=grey>???</color>", "Complete the previous stage to unlock."));
                }
            }

            Get(ROOT_INDEX).Actions = buttons;
            Get(ROOT_INDEX).OnEnter = () => {
                Get(ROOT_INDEX)
                .AddText(
                    "Where would you like to go?"
                    );
            };
        }

        private Area GetCurrentArea(Flags flags) {
            return AreaList.AllAreas[flags.CurrentArea];
        }

        private bool IsStagePlayable(int index, Area area) {
            return index <= 0 || area.IsStageCleared(index - 1);
        }

        private Process GetDungeonEntryProcess(int index, Area area) {
            Dungeon dungeon = area.Dungeons[index];
            return new Process(
                    string.Format("{0}-{1}", area.Type.GetDescription(), index),
                    dungeon.Sprite,
                    string.Format("Enter stage {0}-{1}:\n<i>{2}</i>", (int)area.Type, index, dungeon.ButtonText),
                    () => dungeon.Invoke()
                );
        }
    }
}