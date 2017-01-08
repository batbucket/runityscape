using Scripts.Model.Acts;
using Scripts.Model.Characters;
using Scripts.Model.Characters.Named;
using Scripts.Model.Pages;
using Scripts.Model.Spells.Named;
using Scripts.Model.TextBoxes;
using Scripts.Model.World.Utility;
using Scripts.Presenter;

namespace Scripts.Model.World.Pages {

    public class IntroScenePage : ReadPage {
        private Kitsune fox;
        private Knight knight;

        public IntroScenePage(Party party)
            : base("",
                   "",
                   "Temple - Starlight Gateway",
                   false,
                   null,
                   null) {
            // HACK because Talk checks your compile time side
            this.fox = new Kitsune();
            fox.Side = true;
            this.knight = new Knight();
            knight.Side = false;
            OnEnterAction += () => {
                Game.Instance.Cutscene(false,
                    new Act(() => AddCharacters(true, fox)),
                    new Act(new TextBox("The fallen bodies of countless knights litter the room.")),
                    new Act(() => AddCharacters(false, knight)),
                    new Act(knight.Talk("They had families and aspirations, you monster! How many more have to die?")),
                    new Act(fox.Talk("There was no need for any bloodshed. I only wanted to access the gateway.")),
                    new Act(fox.Talk("All they had to do was look the other way and let me through, but they didn't.")),
                    new Act(fox.Talk("This is the aftermath of their choice.")),
                    new Act(fox.Talk("Leave now little human, this olive-branch I extend to you. Whatever that means.")),
                    new Act(fox.Talk("Leave, and live another day.")),
                    new Act(knight.Talk("...")),
                    new Act(() => (new Attack(false)).Cast(knight, fox)),
                    new Act(fox.Talk("Have it your way, then.")),
                    new Act(() => (new BackTurn(true)).Cast(fox, fox)),
                    new Act(fox.Emote("The kitsune turns back around to face the gateway.")),
                    new Act(() => (new Attack(false)).Cast(knight, fox)),
                    new Act(fox.Talk("Now, back to important matters...")),
                    new Act(new Camp(party, new EventFlags()))
                    );
            };
        }
    }
}