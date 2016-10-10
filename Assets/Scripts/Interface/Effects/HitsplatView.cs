using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class HitsplatView : PooledBehaviour {

    public void Animation(string s, Color c) {
        Text text = gameObject.GetComponent<Text>();
        text.text = s;
        text.color = c;
        StartCoroutine(Animation(text));
    }

    const float TIME_UPSIZED = 1.0f;
    const float TIME_BEFORE_DECAY = .75f;
    const float ACCEL_RATE = 3.0f;
    static readonly Vector2 INITIAL_SIZE = new Vector2(1.5f, 1.5f);
    static readonly Vector2 FINAL_SIZE = new Vector2(1, 1);

    public IEnumerator Animation(Text text) {
        float timer = TIME_UPSIZED;
        float accel = 0;
        while ((timer -= Time.deltaTime * accel) > 0) {
            text.transform.localScale = Vector2.Lerp(INITIAL_SIZE, FINAL_SIZE, (TIME_UPSIZED - timer) / TIME_UPSIZED);
            accel += ACCEL_RATE;
            yield return null;
        }

        float timer2 = TIME_BEFORE_DECAY;
        while ((timer2 -= Time.deltaTime) > 0) {
            yield return null;
        }

        while (text.color.a > 0) {
            Color c = text.color;
            c.a -= Time.deltaTime * 3;
            text.color = c;
            yield return null;
        }
        ObjectPoolManager.Instance.Return(this);
        yield break;
    }

    public override void Reset() {
    }
}
