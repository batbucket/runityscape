using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HitsplatView : MonoBehaviour {

    public void GrowAndFade(string s, Color c) {
        Text text = gameObject.GetComponent<Text>();
        text.text = s;
        text.color = c;
        StartCoroutine(GrowAndFade(text));
    }

    public static IEnumerator GrowAndFade(Text text) {
        float timer = .2f;
        while ((timer -= Time.deltaTime) > 0) {
            //Wait
            yield return null;
        }
        while (text.color.a > 0) {
            Color c = text.color;
            c.a -= Time.deltaTime * 3;
            text.color = c;
            text.transform.localScale = new Vector3(text.transform.localScale.x + Time.deltaTime, text.transform.localScale.y + Time.deltaTime, text.transform.localScale.z);
            yield return null;
        }
        Destroy(text.gameObject);
        yield break;
    }
}
