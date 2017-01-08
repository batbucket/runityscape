using UnityEngine;
using System.Collections;
using Scripts.Model.Pages;
using Scripts.Presenter;
using Scripts.Model.TextBoxes;

namespace Scripts.Model.World.Pages {

    public class CreditsPage : ReadPage {
        private const string PROGRAMMING = "programming - andrewx, https://bitbucket.org/eternitylabs/runityscape";
        private const string VISUALS = "visuals - game-icons.net";
        private const string MUSIC = "music from opengameart.org:\nWind - IgnasD\nEnchanted Tiki 86 - cynicmusic.com\nHero Immortal - Trevor Lentz";
        private const string SOUND_EFFECTS = "sound effects - soundbible.com";
        private const string TESTERS = "testers - simon, vishal, rohan";
        private const string SPECIAL_THANKS = "special thanks - unity, stackoverflow.com, and bitbucket";

        public CreditsPage(Page back) : base("", "", "Credits", false) {
            OnEnterAction += () => {
                Game.Instance.TextBoxes.AddTextBox(new TextBox(PROGRAMMING));
                Game.Instance.TextBoxes.AddTextBox(new TextBox(VISUALS));
                Game.Instance.TextBoxes.AddTextBox(new TextBox(MUSIC));
                Game.Instance.TextBoxes.AddTextBox(new TextBox(SOUND_EFFECTS));
                Game.Instance.TextBoxes.AddTextBox(new TextBox(TESTERS));
                Game.Instance.TextBoxes.AddTextBox(new TextBox(SPECIAL_THANKS));

                ActionGrid[0] = back;
            };
        }
    }
}