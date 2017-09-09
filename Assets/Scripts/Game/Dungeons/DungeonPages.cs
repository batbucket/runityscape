using Scripts.Game.Serialized;
using Scripts.Model.Characters;
using Scripts.Model.Interfaces;
using Scripts.Model.Pages;
using Scripts.Model.Processes;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Game.Dungeons {

    /// <summary>
    /// Stage selection menu.
    /// </summary>
    /// <seealso cref="Scripts.Model.Pages.PageGroup" />
    public class DungeonPages : PageGroup {
        private readonly Party party;
        private readonly Flags flags;
        private readonly Page previous;

        /// <summary>
        /// Initializes a new instance of the <see cref="DungeonPages"/> class.
        /// </summary>
        /// <param name="previous">The previous page to back to.</param>
        /// <param name="party">The current party.</param>
        /// <param name="flags">The game's flags.</param>
        public DungeonPages(Page previous, Party party, Flags flags) : base(new Page("Quest")) {
            var buttons = new List<IButtonable>();
            this.party = party;
            this.flags = flags;
            this.previous = previous;

            Root.Icon = Util.GetSprite("dungeon-gate");
            Root.AddCharacters(Side.LEFT, party);
            Root.Condition = PageUtil.GetVisitProcessCondition(flags, party);

            buttons.Add(PageUtil.GenerateBack(previous));

            Area currentArea = GetCurrentArea(flags.CurrentArea);

            for (int i = 0; i < currentArea.Stages.Length; i++) {
                if (IsStagePlayable(i, currentArea)) {
                    buttons.Add(GetDungeonEntryProcess(i, currentArea));
                } else {
                    buttons.Add(new Process("<color=grey>???</color>", "Complete the previous stage to unlock."));
                }
            }

            Get(ROOT_INDEX).Actions = buttons;
            Get(ROOT_INDEX).OnEnter = () => {
                Get(ROOT_INDEX)
                .AddText(
                    "Select a stage."
                    );
            };
        }

        /// <summary>
        /// Gets the current area.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private Area GetCurrentArea(AreaType type) {
            return AreaList.ALL_AREAS[type](flags, party, previous, Root);
        }

        /// <summary>
        /// Determines whether the particular stage is unlocked.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="area">The area.</param>
        /// <returns>
        ///   <c>true</c> if the stage is unlocked; otherwise, <c>false</c>.
        /// </returns>
        private bool IsStagePlayable(int index, Area area) {
            return index <= 0 || flags.IsStageCleared(index - 1, area.Type);
        }

        /// <summary>
        /// Gets the dungeon confirmation buttonable.
        /// </summary>
        /// <param name="index">The index of this dungeon.</param>
        /// <param name="area">The area this dungeon is in.</param>
        /// <returns></returns>
        private Process GetDungeonEntryProcess(int index, Area area) {
            Dungeon dungeon = area.Stages[index];
            Color buttonColor = Color.white;
            if (index == AreaList.MINIBOSS_INDEX) {
                buttonColor = AreaList.MINIBOSS_STAGE_TEXT_COLOR;
            } else if (index == AreaList.BOSS_INDEX) {
                buttonColor = AreaList.BOSS_STAGE_TEXT_COLOR;
            }
            return new Process(
                    Util.ColorString(string.Format("{0}-{1}", (int)area.Type, index), buttonColor),
                    dungeon.Sprite,
                    dungeon.Root.Location,
                    () => {
                        dungeon.Invoke();
                        flags.ShouldAdvanceTimeInCamp = true;
                    }
                );
        }
    }
}