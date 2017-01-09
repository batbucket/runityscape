using Scripts.Model.Characters;
using Scripts.Model.Pages;
using Scripts.Model.Processes;
using Scripts.Model.World.Flags;
using Scripts.Presenter;

namespace Scripts.Model.World.Pages {

    public class TempleHall : PlacePage {

        public TempleHall(Page defeatPage, EventFlags flags, Party party)
            : base(party, "Starlight Temple/Hall", "Wind",
              "A chilling wind blows through these giant halls."
              + " The walls and floor are a dull gold. Stained glass windows adorn the walls, all heavily damaged."
              + " The black slick continues inside, leading deeper into the temple.") {
            OnEnterAction += () => {
                ActionGrid[0] = new Process("Back", "Go back outside.", () => Game.Instance.CurrentPage = new TempleEntrance(defeatPage, flags, party));
                ActionGrid[1] = new Process("Proceed", "Delve deeper into the temple.", () => Game.Instance.CurrentPage = new TempleGateway(defeatPage, flags, party));
            };
        }
    }
}