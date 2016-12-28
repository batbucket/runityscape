using UnityEngine;
using System.Collections;

/// <summary>
/// Represents a group of related page in the game.
/// </summary>
public abstract class Area {

    public Area() {
        Init();
    }

    /// <summary>
    /// Post constructor intitialization.
    /// </summary>
    public abstract void Init();
}
