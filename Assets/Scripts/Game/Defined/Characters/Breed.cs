using System.ComponentModel;

namespace Scripts.Game.Defined.Characters {
    public enum Breed {
        [Description("???")]
        UNKNOWN = 0,
        [Description("Monster")]
        MONSTER,
        [Description("Human")]
        HUMAN,

        // These are here for the credits
        [Description("Tester")]
        TESTER,
        [Description("Persister")]
        CREATOR,
        [Description("Commenter")]
        COMMENTER
    }
}