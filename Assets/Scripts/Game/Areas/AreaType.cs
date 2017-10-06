using System.ComponentModel;

namespace Scripts.Game.Areas {

    /// <summary>
    /// Each area is associated with an enum
    /// to make serialization easy.
    /// </summary>
    public enum AreaType {

        [Description("None")]
        NONE,

        [Description("Java Crypt")]
        RUINS,

        [Description("Objective Sea")]
        SEA_WORLD,

        [Description("MAT Labs")]
        LAB,
    }
}