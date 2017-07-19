using Scripts.Game.Defined.Characters;
using Scripts.Game.Serialized;
using Scripts.Model.Acts;
using Scripts.Model.Characters;
using Scripts.Model.Interfaces;
using Scripts.Model.Pages;
using Scripts.Model.Processes;
using Scripts.Model.TextBoxes;

namespace Scripts.Game.Pages {
    public class Cathedral : VisitablePlace {
        public const string DISCOVERED_PLACE = "{0} was discovered.\nThis place can be visited again in the Places menu back at camp.";

        private const int DUNGEON_END = 1;
        private const int BOSS_ROOM = 2;
        private const int BOSS_BATTLE = 3;
        private const int BOSS_VICTORY = 4;

        private Page camp;
        private Party party;
        private Flags flags;

        public Cathedral(Page camp, Party party, Flags flags) : base(Place.CATHEDRAL, new Page("Cathedral")) {
            this.camp = camp;
            this.party = party;
            this.flags = flags;

            Character fox = CharacterList.Ruins.Kitsune();

            Register(DUNGEON_END, new Page("Last Corridor"));
            Register(BOSS_ROOM, new Page("Gateway"));
            Register(BOSS_VICTORY, new Page("Gateway"));
            Register(BOSS_BATTLE, new Battle(camp, Get(BOSS_VICTORY), Music.BOSS, "Gateway", party, new Character[] { CharacterList.Ruins.Healer(), fox, CharacterList.Ruins.Healer() }));

            SetupRoot();
            SetupEnd();
            SetupBossBattle();
            SetupBossRoom(fox);
            SetupBossVictory(fox);
        }

        private void SetupBossBattle() {
            Get(BOSS_BATTLE).Body = "Two Healers float up from the floor!";
        }

        private void SetupRoot() {
            Root.Body = "Standing out in the ruins, the cathedral somehow stays upright despite all its cracks. The metal doors have been knocked down, lying in pieces in front of the entrance.";
            Root.AddCharacters(Side.LEFT, party);
            Root.OnEnter += () => {
                if (flags.Ruins == RuinsProgression.UNEXPLORED) {
                    flags.Ruins = RuinsProgression.FIRST_VISIT;
                    flags.UnlockedPlaces.Add(Place.CATHEDRAL);
                    PageUtil.GetFirstPlaceVisitMessage(this);
                }

                Root.Actions = new IButtonable[] {
                    ShopList.Ruins(Root, flags, party),
                    new CathedralDungeon(flags, party, camp, Root, Get(DUNGEON_END)),
                    null,
                    camp
                };
            };
        }

        private void SetupEnd() {
            Page p = Get(DUNGEON_END);
            p.AddCharacters(Side.LEFT, party);
            p.Body = "One final archway separates the halls from the cathedral center. The light fails to reach past it, leaving you clueless as to what lies further.";
            Grid grid = new Grid("Dungeon End");
            grid.List.Add(PageUtil.GetConfirmationGrid(p, grid,
                    new Process("Yes", "Leave this dungeon.", () => camp.Invoke()),
                    "Leave", "Leave this dungeon.", "Are you sure you want to leave this dungeon?"));
            grid.List.Add(Get(BOSS_ROOM));
            p.Actions = grid.List;
        }

        private void SetupBossRoom(Character fox) {
            Page p = Get(BOSS_ROOM);
            p.AddCharacters(Side.LEFT, party);
            p.Body = "A dimly lit room with a staircase at the end of it. The staircase leads to a gateway at the very top, devoid of any portal. A black liquid is splattered all over the room and its walls, filling the room with a vile scent.";
            p.Actions = new IButtonable[] { PageUtil.GenerateBack(Get(DUNGEON_END)) };
            p.OnEnter = () => {
                p.Right.Clear();
                if (flags.Ruins == RuinsProgression.ASK_ABOUT_FOX || flags.Ruins == RuinsProgression.BOSS_CONFRONTED) {
                    p.AddCharacters(Side.RIGHT, fox);

                    if (flags.Ruins == RuinsProgression.ASK_ABOUT_FOX) {
                        flags.Ruins = RuinsProgression.BOSS_CONFRONTED;
                        ActUtil.SetupScene(
                            p,
                            ActUtil.LongTalk(p, fox,
                            "<t>Your entry into the room draws the attention of an unknown female figure at the bottom of the staircase. Two furry, fox-like ears top her head, which share her long hair's black coloration. A black, writhing mass of what appears to be her tails is behind her. Her skin is a dusky grey, only slightly lighter than her hair color. A blood red robe covers her body. There's no doubt she's the fox-creature Maple was describing from earlier.<t>Her red, slitted eyes glare at you.<a>So, Alestre's bringing back a dead hero, huh? Is it because she can't find any living ones?<a>Truly a pathetic move by your so-called goddess.<a>I, {0}, will bring about your end once again, mayfly. And the heavens will follow.<t>{0} charges at you!",
                            new BossTransitionAct(Get(BOSS_BATTLE), fox.Look)));
                    } else {
                        ActUtil.SetupScene(
                            p,
                            ActUtil.LongTalk(p, fox,
                            "<a>I have nothing to say to you, mayfly.<t>{0} walks towards you.",
                            new BossTransitionAct(Get(BOSS_BATTLE), fox.Look)));
                    }
                }
            };
        }

        private void SetupBossVictory(Character fox) {
            Page p = Get(BOSS_VICTORY);
            p.AddCharacters(Side.LEFT, party);
            p.AddCharacters(Side.RIGHT, fox);
            p.OnEnter = () => {
                flags.Ruins = RuinsProgression.BOSS_VICTORY;
                ActUtil.SetupScene(
                    p,
                    ActUtil.LongTalk(p, fox,
                    @"<a>It seems I underestimated you. No matter, this is still my victory.
                      <a>If I cannot defeat you, then I am not ready to take down the Celestials.
                      <a>Without your interference I would have surely died in my attack.
                      <a>We will meet again.
                      <t>{0} dissolves into a pool of black liquid, vanishing into the floor.",
                    new PageChangeAct(Get(BOSS_ROOM))
                    )
                    );
            };
        }
    }
}