using Scripts.Model.Items;

namespace Scripts.Game.Defined.Serialized.Items {

    public class Money : BasicItem {
        public const string NAME = "Droplet";

        public Money()
            : base(
            Util.GetSprite("water-drop"),
            0,
            NAME,
            "A droplet of pure water. Used as currency."
            ) { }
    }
}