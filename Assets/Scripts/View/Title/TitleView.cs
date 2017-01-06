using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.View.Title {

    /// <summary>
    /// Represents a title transition with an
    /// image as well as subtitles.
    /// </summary>
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

        public void Cancel() {
            this.IsDone = true;
            StopAllCoroutines();
            alpha = 0;
        }

        public void Play(string imageLoc, string text) {
            StartCoroutine(TitleTransition(imageLoc, text, DURATION));
        }

        private IEnumerator TitleTransition(string imageLoc, string text, float duration) {
            this.IsDone = false;
            image.sprite = Util.LoadIcon(imageLoc);
            this.text.text = text;
            float timer = 0;

            // Fade in
            while ((timer += Time.deltaTime) < FADE_TIME) {
                alpha = Mathf.Lerp(0, 1, timer / FADE_TIME);
                yield return null;
            }
            alpha = 1;
            yield return new WaitForSeconds(duration);
            timer = 0;

            // Fade out
            while ((timer += Time.deltaTime) < FADE_TIME) {
                alpha = Mathf.Lerp(1, 0, timer / FADE_TIME);
                yield return null;
            }
            alpha = 0;
            this.IsDone = true;
        }
    }
}