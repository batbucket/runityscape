using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text.RegularExpressions;

/**
 * This class manages a TextBox prefab
 */
public class TextBoxView : MonoBehaviour {

    [SerializeField]
    Text text;
    [SerializeField]
    AudioSource characterSound;
    float timePerLetter; //Speed at which the letters appear

    int index; //Which letter to make visible
    float timer; //Needs to be >= timePerLetter for a letter to appear

    public const int BLIP_INTERVAL = 1; //Letters needed for a sound to occur
    public const int CHARS_PER_LINE = 44; //Needed for word wrapping function

    public void WriteText(TextBox textBox) {
        StartCoroutine(WriteText(text, textBox));
    }

    IEnumerator WriteText(Text text, TextBox textBox) {
        text.color = textBox.Color;
        switch (textBox.Effect) {
            case TextEffect.NONE:
                text.text = textBox.RawText;
                Game.Instance.Sound.Play(textBox.SoundLocation);
                yield break;
            case TextEffect.FADE_IN:
                text.text = textBox.RawText;
                CanvasRenderer cr = gameObject.GetComponent<CanvasRenderer>();
                Color c = cr.GetColor();
                c.a = 0;
                cr.SetColor(c);
                while (cr.GetColor().a < 1) {
                    c = cr.GetColor();
                    c.a += Time.deltaTime * 3;
                    cr.SetColor(c);
                    yield return null;
                }
                yield break;
            case TextEffect.TYPE:
                string[] currentTextArray = new string[textBox.TextArray.Length];
                bool[] taggedText = new bool[currentTextArray.Length];

                //Prescreen and enable all html tags
                for (int i = 0; i < currentTextArray.Length; i++) {
                    if (Regex.IsMatch(textBox.TextArray[i], "(<.*?>)")) {
                        currentTextArray[i] = textBox.TextArray[i];
                        taggedText[i] = true;
                    }
                }
                float timer = 0;
                int index = 0;
                while (text.text.Length < textBox.RawText.Length) {
                    text.text = string.Join("", currentTextArray);
                    timer += Time.deltaTime;
                    if (timer >= textBox.TimePerLetter) {

                        //Don't reset timer or make sound on taggedText and spaces
                        if (!taggedText[index] && !Regex.IsMatch(textBox.TextArray[index], " ")) {
                            timer = 0;
                            Game.Instance.Sound.Play(characterSound);
                        }
                        if (!taggedText[index]) {
                            currentTextArray[index] = textBox.TextArray[index];
                        }
                        index++;
                    }
                    yield return null;
                }
                yield break;
        }
    }
}
