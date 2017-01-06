using Scripts.Model.Acts;
using Scripts.Model.Characters;
using Scripts.Model.Characters.Named;
using Scripts.Model.Pages;
using Scripts.Model.Processes;
using Scripts.Model.TextBoxes;
using Scripts.Model.World.Pages;
using Scripts.Model.World.Utility;
using Scripts.Presenter;

namespace Scripts.Model.World.Pages {

    public class TemplePostBoss : PlacePage {

        public TemplePostBoss(Page defeatPage, EventFlags flags, Party party, Kitsune fox)
            : base(party, "Starlight Temple/Entrance", "",
                "") {
            OnEnterAction += () => {
                Game.Instance.Cutscene(false,
                new Act(() => AddCharacters(true, fox)),
                new Act(fox.Emote("{0} rushes into the gateway and disappears!")),
                new Act(() => GetCharacters(true).Remove(fox)),
                new Act(fox.Emote("A group of powerful tentacles erupt in front of the gateway, blocking it.")),
                new Act(new TempleGateway(defeatPage, flags, party)));
            };
        }
    }
}