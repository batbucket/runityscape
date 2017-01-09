using Scripts.Model.Characters;
using Scripts.Model.World.Serialization;
using Scripts.Model.Interfaces;
using Scripts.Model.Pages;
using Scripts.Model.Processes;
using Scripts.Presenter;
using Scripts.Model.World.Flags;

namespace Scripts.Model.World.Pages {

    public class Camp : ReadPage {
        public EventFlags Flags;

        public Party Party;

        private const int DAYS_DISPLAY_LIMIT = 9999;

        private const float REST_RESTORE_PERCENT = 0.4f;

        private const float SLEEP_RESTORE_PERCENT = 0.8f;

        private CharactersPage character;

        private ExplorePage explore;

        private ItemManagePage itemMan;

        private PlacesPage places;

        private bool hasTraveled;

        public Camp(Party party, EventFlags flags)
            : base(
                "",
                "",
                "Camp",
                false
                ) {
            this.Party = party;
            this.Flags = flags;

            OnFirstEnterAction += () => {
                CreateCamp();
                Game.Instance.Time.IsTimeEnabled = true;
                Game.Instance.Time.IsDayEnabled = true;
            };
            OnEnterAction += () => {
                TimeUpdateCheck(Flags);
                PageUtil.EnterMessages(Flags);
                Game.Instance.Other.Camp = this;
                PageUtil.RestoreParty(Party);
            };
        }

        public bool HasTraveled {
            set {
                hasTraveled = value;
            }
        }

        public int Day {
            get {
                return Flags.Ints[Flag.DAYS];
            }
        }

        public int Gold {
            get {
                return Party.Inventory.Gold;
            }
        }

        public int Time {
            get {
                return Flags.Ints[Flag.TIME];
            }
        }

        private void CreateCamp() {
            this.explore = new ExplorePage(Flags, this, Party);
            this.places = new PlacesPage(Flags, this, Party);

            this.character = new CharactersPage(this, Party);
            this.itemMan = new ItemManagePage(this, Party);

            this.ActionGrid = new IButtonable[] {
                new Process("Explore", "Explore a location.", () => Game.Instance.CurrentPage = explore,
                () => !PageUtil.IsNight(Flags)),
                new Process("Places", "Visit a place.", () => Game.Instance.CurrentPage = places,
                () => !PageUtil.IsNight(Flags)),
                character,
                itemMan,

                null,
                null,
                PageUtil.CreateRest(Flags, Party, REST_RESTORE_PERCENT),
                PageUtil.CreateSleep(Flags, Party, SLEEP_RESTORE_PERCENT),

                null,
                null,
                null,
                new Process("Save & Exit", "", () => Game.Instance.CurrentPage = new SavePage(this))
        };
            this.LeftCharacters = Party.Members;
        }

        private void TimeUpdateCheck(EventFlags flags) {
            if (hasTraveled) {
                hasTraveled = false;
                PageUtil.AdvanceTime(flags);
            }
        }
    }
}