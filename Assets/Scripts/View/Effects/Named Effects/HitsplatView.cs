using Scripts.View.ObjectPool;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.View.Effects {

    public class HitsplatView : PooledBehaviour {

        private const float ACCEL_RATE = 3.0f;

        private const float TIME_BEFORE_DECAY = .75f;

        private const float FADE_OUT_TIME = 0.5f;

        private const float TIME_UPSIZED = 1.0f;

        private static readonly Vector2 FINAL_SIZE = new Vector2(1, 1);

        private static readonly Vector2 INITIAL_SIZE = new Vector2(3f, 3f);

        [SerializeField]
        private Text text;

        [SerializeField]
        private Image image;

        public IEnumerator Animation(string s, Color c, Sprite sprite) {
            Util.Log(string.Format("Hitsplat animation with string={0}, color={1}, sprite={2}", s, c, sprite));
            this.image.sprite = sprite;
            image.gameObject.SetActive(sprite != null);
            text.color = c;
            text.text = s;
            image.color = c;

            float timer = 0;
            float accel = 0;
            while ((timer += Time.deltaTime * accel) < TIME_UPSIZED) {
                Vector2 scale = Vector2.Lerp(INITIAL_SIZE, FINAL_SIZE, timer / TIME_UPSIZED);
                text.transform.localScale = scale;
                image.transform.localScale = scale;
                accel += ACCEL_RATE;
                yield return null;
            }

            float timer2 = 0;
            while ((timer2 += Time.deltaTime) < TIME_BEFORE_DECAY) {
                yield return null;
            }

            float timer3 = 0;
            while ((timer3 += Time.deltaTime) < FADE_OUT_TIME) {
                c.a -= Time.deltaTime * 3;
                float t = 1 - timer3 / FADE_OUT_TIME;
                Util.SetTextAlpha(text, t);
                Util.SetImageAlpha(image, t);
                yield return null;
            }
            ObjectPoolManager.Instance.Return(this);
            yield break;
        }

        public override void Reset() {
            text.text = string.Empty;
            text.transform.localScale = new Vector3(1, 1, 1);
            image.transform.localScale = new Vector3(1, 1, 1);
            text.color = Color.white;
            image.color = Color.white;
        }
    }
}