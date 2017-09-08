using Scripts.Game.Dungeons;
using Scripts.Game.Serialized;
using Scripts.Model.Characters;
using Scripts.Model.Interfaces;
using Scripts.Model.Pages;
using Scripts.Model.Processes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Scripts.Game.Pages {
    public class WorldPages : PageGroup {
        public WorldPages(Page previous, Flags flags, Party party) : base(new Page("Areas")) {
            Root.Body = "Which area would you like to go to?";

            IList<IButtonable> buttons = new List<IButtonable>();
            buttons.Add(PageUtil.GenerateBack(previous));

            Root.AddCharacters(Side.LEFT, party.Collection);

            foreach (AreaType type in AreaList.ALL_AREAS.Keys) {
                Util.Log(type.ToString());
                if (flags.IsAreaUnlocked(type) && flags.CurrentArea != type) {
                    buttons.Add(GetAreaChangeProcess(type, flags, party, previous));
                }
            }

            Root.Actions = buttons;
        }

        private Process GetAreaChangeProcess(AreaType type, Flags flags, Party party, Page previous) {
            return new Process(
                    type.GetDescription(),
                    "Move to this area.",
                    () => {
                        flags.CurrentArea = type;
                        previous.Invoke();
                        previous.AddText(string.Format("Moved to {0}.", type.GetDescription()));
                    }
                );
        }
    }
}