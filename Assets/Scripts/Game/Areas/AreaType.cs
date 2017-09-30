using System.ComponentModel;

namespace Scripts.Game.Areas {

    /// <summary>
    /// Each area is associated with an enum
    /// to make serialization easy.
    /// </summary>
    public enum AreaType {

        [Description("None")]
        NONE,

        [Description("Tiny Woods")]
        TINY_WOODS,

        [Description("Sea World")]
        SEA_WORLD
    }
}