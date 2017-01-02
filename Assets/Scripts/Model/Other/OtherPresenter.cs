using UnityEngine;
using System.Collections;

public class OtherPresenter {
    public Camp Camp {
        set {
            camp = value;
        }
    }

    private GoldView gold;
    private TimeView time;

    private Camp camp;

    public OtherPresenter(GoldView gold, TimeView time) {
        this.gold = gold;
        this.time = time;
    }

    public void Tick() {
        gold.IsEnabled = (camp != null);
        time.IsEnabled = (camp != null);
        if (camp != null) {
            gold.Count = camp.Gold;
            time.Day = camp.Day;
            TimeType type = TimeType.Get(camp.Time);
            time.Time = Util.Color(type.Name, type.Color);
        }
    }
}
