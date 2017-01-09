using Scripts.Model.Characters;
using Scripts.Model.Characters.Named;
using Scripts.Model.Pages;
using Scripts.Model.Processes;
using Scripts.Model.TextBoxes;
using Scripts.Model.World.Flags;
using Scripts.Presenter;

namespace Scripts.Model.World.Pages {

    public class TempleEntrance : PlacePage {

        public TempleEntrance(Page defeatPage, EventFlags flags, Party party)
            : base(party, "Starlight Temple/Entrance", "",
                "A lengthy bridge connects this plateau to the mainland."
              + " The center of the plateau holds a giant derelict temple."
              + " You can't see the top."
              + " The remains of a massive gate are spread across the ground in front of the equally massive entrance."
              + " A black slick trails into the temple.") {
            OnEnterAction += () => {
                if (flags.Ints[Flag.TEMPLE_STATUS] < Flag.TEMPLE_ENTRANCE_CLEARED) {
                    Game.Instance.TextBoxes.AddTextBox(new TextBox("Three Bishops stand at guard, blocking the entrance."));
                }
                ActionGrid[1] = new Process("Entrance", "Proceed into the temple.", () => {
                    if (flags.Ints[Flag.TEMPLE_STATUS] < Flag.TEMPLE_ENTRANCE_CLEARED) {
                        Game.Instance.CurrentPage = new BattlePage(party,
                            new BattleResult(new TempleHall(defeatPage, flags, party), () => flags.Ints[Flag.TEMPLE_STATUS] = Flag.TEMPLE_ENTRANCE_CLEARED),
                            new BattleResult(defeatPage),
                            "Hero Immortal",
                            "Temple/Entrance",
                            "Three Bishops block the way.",
                            new Character[] {
                                    new Regenerator(),
                                    new Regenerator(),
                                    new Regenerator()
                            }
                            );
                    } else {
                        Game.Instance.CurrentPage = new TempleHall(defeatPage, flags, party);
                    }
                });
                ActionGrid[0] = defeatPage;
            };
        }
    }
}