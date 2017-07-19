using System.ComponentModel;

namespace Scripts.Game.Defined.Characters {
    public enum Breed {
        [Description("???")]
        UNKNOWN = 0,
        [Description("Monster")]
        MONSTER,
        [Description("Human")]
        HUMAN,
        [Description("Ghost")]
        SPIRIT,
        [Description("Kitsune")]
        KITSUNE,

        // These are here for the credits
        [Description("Tester")]
        TESTER,
        [Description("Persister")]
        CREATOR,
        [Description("Commenter")]
        COMMENTER
    }
}