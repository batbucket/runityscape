using Scripts.Presenter;
using System.Collections;
using UnityEngine;

namespace Scripts.View.Effects {

    /// <summary>
    /// This is an abstract class representing any effect in the game
    /// </summary>
    public abstract class Effect {
        protected static EffectsManager effects = EffectsManager.Instance;

        protected bool isDone;

        private Coroutine _coroutine;

        public Coroutine Coroutine { get { return _coroutine; } }

        public string ID {
            get {
                return this.GetType().ToString();
            }
        }

        public bool IsDone {
            get {
                return isDone;
            }
        }

        public void Play() {
        }

        public void StartCoroutine() {
            this._coroutine = Game.Instance.StartCoroutine(EffectRoutine());
        }

        public void StopCoroutine() {
            if (Coroutine != null) {
                Game.Instance.StopCoroutine(Coroutine);
            }
        }

        protected abstract IEnumerator EffectRoutine();
    }
}