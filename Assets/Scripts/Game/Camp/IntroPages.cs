using Scripts.Game.Defined.Characters;
using Scripts.Game.Serialized;
using Scripts.Model.Acts;
using Scripts.Model.Characters;
using Scripts.Model.Pages;
using Scripts.Model.TextBoxes;
using System;
using UnityEngine;

namespace Scripts.Game.Pages {
    public class IntroPages : PageGroup {
        public IntroPages(string name) : base(new Page("Unknown")) {
            SetupIntro(name);
        }

        private void SetupIntro(string name) {
            Page page = Root;
            Character you = CharacterList.Hero(name);
            Character partner = CharacterList.Partner("???");

            page.AddCharacters(Side.LEFT, you);
            page.AddCharacters(Side.RIGHT, partner);

            page.OnEnter = () => {
                ActUtil.SetupScene(
                         YourVoice(string.Format("My name is {0}.", name)),
                         PartnerVoice(string.Format("My name is...")),
                         new InputAct("What is their name?", (s) =>
                                ActUtil.SetupScene(
                                    PartnerVoice("My name is " + s),
                                    new ActionAct(() => partner.Look.Name = s),
                                    new ActionAct(() => GoToCamp(you, partner)
                                    )
                                )
                        )
                    );
            };
        }

        private void GoToCamp(Character you, Character partner) {
            Party party = new Party();
            Flags flags = new Flags();
            party.AddMember(you);
            party.AddMember(partner);
            Camp camp = new Camp(party, flags);
            camp.Root.Invoke();
        }

        private static readonly Sprite hero = CharacterList.Hero(string.Empty).Look.Sprite;
        private static readonly Sprite partner = CharacterList.Partner(string.Empty).Look.Sprite;

        private TextAct YourVoice(string message) {
            return new TextAct(new AvatarBox(Side.RIGHT, hero, Color.white, message));
        }

        private TextAct PartnerVoice(string message) {
            return new TextAct(new AvatarBox(Side.RIGHT, partner, Color.white, message));
        }

        private TextAct UnknownVoice(string message) {
            return new TextAct(new AvatarBox(Side.RIGHT, Util.GetSprite("question-mark"), Color.grey, message));
        }
    }
}