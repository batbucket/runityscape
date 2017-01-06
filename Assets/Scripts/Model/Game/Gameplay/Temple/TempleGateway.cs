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

    public class TempleGateway : PlacePage {
        private Kitsune fox;

        public TempleGateway(Page defeatPage, EventFlags flags, Party party)
            : base(party, "Starlight Temple/Gateway", "",
              "A large gateway lies in the center of the room, giving the room a violet glow."
              + " The black slick ends at the gateway.") {
            fox = new Kitsune();
            if (flags.Ints[Flag.TEMPLE_STATUS] < Flag.TEMPLE_BOSS_CLEARED) {
                fox.Side = true;
                AddCharacters(true, fox);
            }
            OnEnterAction += () => {
                if (flags.Ints[Flag.TEMPLE_STATUS] < Flag.TEMPLE_BOSS_CLEARED) {
                    Game.Instance.TextBoxes.AddTextBox(
                        new TextBox(
                            "A kitsune wearing a red robe stands right in front of the gateway, gazing into it."
                            + " Her tails seem to be steeped in the black slick, giving them a dark oily appearance."
                            + " The fox-like ears on her head twitch as she turns around to face you."
                            + " Her skin is dark gray. Her eyes are red, with black scleras."
                            + " Despite the circumstances, her face holds a calm expression."
                            )
                        );

                    ActionGrid[1] = new Process(
                        "Approach",
                        "Approach the fox-like creature.",
                        () => {
                            Act[] intro = null;
                            if (flags.Ints[Flag.TEMPLE_STATUS] < Flag.TEMPLE_BOSS_MET) {
                                flags.Ints[Flag.TEMPLE_STATUS] = Flag.TEMPLE_BOSS_MET;
                                if (flags.Ints[Flag.SHOPKEEPER_STATUS] == Flag.SHOPKEEPER_DEAD) {
                                    intro = Act.LongTalk(fox,
                                    "/I see all, human./That shopkeeper, Maple?/I know that you killed her./She was scouting me out for the Keepers./I'm sure she was told multiple times about how dangerous I am./And who does she get killed by? A lowly mugger./To be honest I wouldn't have even attacked her./What does that say about you?/Well, whatever./You're not getting out of here alive.>{0} charges at you!"
                                    );
                                } else {
                                    intro = Act.LongTalk(fox,
                                    "/The Celestials don't care, human./They just want resources from this world./Soldiers, metals, all of that./Just to extend a war whose outcome won't affect us in the slightest./The Keepers think they can stall this out for a little longer./That's where they're wrong./We don't need humans, or even all of monsterkind on our side./One drop of corruption is all it'll take to remove Alestre, and I'm a whole payload./But you don't care, do you?/You'll follow them to the bitter end./In that case...>{0} charges at you!/Fight for your gods, human, and die for them!"
                                    );
                                }
                            } else {
                                intro = Act.LongTalk(fox,
                                        "/How many times is it going to take?"
                                    );
                            }
                            Game.Instance.Cutscene(false,
                                    intro,
                                    new Act("fox-head", fox.Name),
                                    new Act(new BattlePage(party,
                                        new BattleResult(new TemplePostBoss(defeatPage, flags, party, fox), () => flags.Ints[Flag.TEMPLE_STATUS] = Flag.TEMPLE_BOSS_CLEARED),
                                        new BattleResult(defeatPage),
                                        "enchanted tiki 86",
                                        "Temple/Gateway",
                                        "Kitsune attacks!",
                                        new Kitsune()))
                                );
                        }
                        );
                } else {
                    Game.Instance.TextBoxes.AddTextBox(
                        new TextBox("A group of powerful tentacles are blocking the entrance to the gateway.")
                        );
                    ActionGrid[1] = new Process(
                        "Gateway",
                        "Attempt to enter the gateway.",
                        () =>
                        Game.Instance.CurrentPage = new BattlePage(
                            party,
                            new BattleResult(this),
                            new BattleResult(this),
                            "Hero Immortal",
                            "Temple/Gateway",
                            "A group of Immortals block the way!",
                        new Immortal(), new Immortal(), new Immortal()));
                }
                ActionGrid[0] = new Process(
                    "Hall",
                    "Return to the hall.",
                    () => {
                        Game.Instance.CurrentPage = new TempleHall(defeatPage, flags, party);
                    }
                    );
            };
        }
    }
}