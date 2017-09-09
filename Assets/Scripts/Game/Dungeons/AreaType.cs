using System.ComponentModel;

namespace Scripts.Game.Dungeons {

    /// <summary>
    /// Each area is associated with an enum
    /// to make serialization easy.
    /// </summary>
    public enum AreaType {
        [Description("None")]
        NONE,
        [Description("Ruins")]
        RUINS
    }
}