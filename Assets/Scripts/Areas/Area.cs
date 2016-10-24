using UnityEngine;
using System.Collections;

/// <summary>
/// Represents a group of related page in the game.
/// </summary>
public abstract class Area {

    /// <summary>
    /// Reference to the Game class.
    /// </summary>
    protected Game Game {
        get {
            return Game.Instance;
        }
    }

    /// <summary>
    /// Property for setting the current Page.
    /// </summary>
    protected Page Page {
        get {
            return Game.Instance.PagePresenter.Page;
        }
        set {
            Game.Instance.PagePresenter.SetPage(value);
        }
    }

    public Area() {
        Init();
    }

    /// <summary>
    /// Post constructor intitialization.
    /// </summary>
    public abstract void Init();
}
