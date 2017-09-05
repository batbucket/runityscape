using Scripts.Game.Defined.Characters;
using Scripts.Game.Serialized;
using Scripts.Model.Acts;
using Scripts.Model.Characters;
using Scripts.Model.Pages;
using Scripts.Model.TextBoxes;
using UnityEngine;

namespace Scripts.Game.Pages {
    public class IntroPages : PageGroup {
        public IntroPages(string name) : base(new Page("Unknown")) {
            SetupIntro(name);
        }

        private void SetupIntro(string name) {
            Page page = Root;
            page.OnEnter = () => {
                Party party = new Party();
                Flags flags = new Flags();
                party.AddMember(CharacterList.Hero(name));
                Camp camp = new Camp(party, flags);
                camp.Root.Invoke();
            };
        }

        private TextAct IntroVoice(string message, object arg = null) {
            return new TextAct(new AvatarBox(Side.RIGHT, Util.GetSprite("holy-symbol"), Color.yellow, string.Format(message, arg)));
        }
    }
}