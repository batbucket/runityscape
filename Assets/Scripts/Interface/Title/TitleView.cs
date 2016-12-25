using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TitleView : MonoBehaviour {
    [SerializeField]
    private Image background;
    [SerializeField]
    private Image image;
    [SerializeField]
    private Text text;

    private const float FADE_TIME = 0.5f;
    private const float DURATION = 4f;

    public bool IsDone;

    private float alpha {
        set {
            Util.SetImageAlpha(background, value);
            Util.SetImageAlpha(image, value);
            Util.SetTextAlpha(text, value);
        }
    }

    public void Play(string imageLoc, string text) {
        StartCoroutine(TitleTransition(imageLoc, text, DURATION));
    }

    private IEnumerator TitleTransition(string imageLoc, string text, float duration) {
        this.IsDone = false;
        image.sprite = Util.LoadIcon(imageLoc);
        this.text.text = text;
        float timer = 0;
        while ((timer += Time.deltaTime) < FADE_TIME) {
            alpha = Mathf.Lerp(0, 1, timer / FADE_TIME);
            yield return null;
        }
        yield return new WaitForSeconds(duration);
        timer = 0;
        while ((timer += Time.deltaTime) < FADE_TIME) {
            alpha = Mathf.Lerp(1, 0, timer / FADE_TIME);
            yield return null;
        }
        this.IsDone = true;
    }
}
