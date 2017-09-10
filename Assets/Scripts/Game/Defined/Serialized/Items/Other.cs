using Scripts.Model.Items;

namespace Scripts.Game.Defined.Serialized.Items {
    public class Money : BasicItem {
        public Money()
            : base(
            Util.GetSprite("water-drop"),
            0,
            "Droplet",
            "A droplet of pure water. Used as currency."
            ) { }
    }
}