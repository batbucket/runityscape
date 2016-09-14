using UnityEngine;
using System.Collections;

public abstract class Effect {
    protected static EffectsManager effects = EffectsManager.Instance;

    protected bool _isDone;
    public bool IsDone {
        get {
            return _isDone;
        }
    }

    public string ID {
        get {
            return this.GetType().ToString();
        }
    }

    Coroutine _coroutine;
    public Coroutine Coroutine { get { return _coroutine; } }

    public void Play() {

    }

    protected abstract IEnumerator EffectRoutine();

    public void StartCoroutine() {
        this._coroutine = Game.Instance.StartCoroutine(EffectRoutine());
    }

    public void StopCoroutine() {
        Game.Instance.StopCoroutine(Coroutine);
    }
}
