using UnityEngine;
using System.Collections;

public abstract class Area {
    protected Game Game {
        get {
            return Game.Instance;
        }
    }

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

    public abstract void Init();
}
