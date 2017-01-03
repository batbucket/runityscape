using Scripts.View.ObjectPool;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.View.Effects {

    public class FadeAnimation : PooledBehaviour {

        [SerializeField]
        private float fadeDuration;

        [SerializeField]
        private Image image;

        public override void Reset() {
        }

        private IEnumerator FadeEffect() {
            float fadePerTick = (1.0f * Time.deltaTime) / fadeDuration;
            while (image != null && !image.IsDestroyed() && image.color.a > 0) {
                Util.SetImageAlpha(image, image.color.a - fadePerTick);
                yield return null;
            }
            ObjectPoolManager.Instance.Return(this);
            yield break;
        }

        private void Start() {
            StartCoroutine(FadeEffect());
        }
    }
}