using Scripts.Model.World.Flags;
using Scripts.Model.World.Pages;
using Scripts.View.Other;
using UnityEngine.UI;

namespace Scripts.Presenter {

    public class OtherPresenter {

        public Camp Camp {
            set {
                camp = value;
            }
        }

        private GoldView gold;
        private TimeView time;
        private Text version;

        private Camp camp;

        public OtherPresenter(GoldView gold, TimeView time, Text version) {
            this.gold = gold;
            this.time = time;
            this.version = version;
        }

        public void Tick() {
            gold.IsEnabled = (camp != null);
            time.IsEnabled = (camp != null);
            this.version.text = Game.VERSION;
            if (camp != null) {
                gold.Count = camp.Gold;
                time.Day = camp.Day;
                TimeType type = TimeType.Get(camp.Time);
                time.Time = Util.Color(type.Name, type.Color);
            }
        }
    }
}