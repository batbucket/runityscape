using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text.RegularExpressions;

/**
 * This class manages a TextBox prefab
 */
public class TextBoxView : MonoBehaviour {

    Text text;
    string partial; //Currently typed text
    string full; //Whole text to be typed
    float timePerLetter; //Speed at which the letters appear

    int index; //Which letter to make visible
    float timer; //Needs to be >= timePerLetter for a letter to appear

    public const int BLIP_INTERVAL = 1; //Letters needed for a sound to occur
    public const int CHARS_PER_LINE = 44; //Needed for word wrapping function

    bool start;

    // Use this for initialization
    void Awake() {
        text = gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update() {
        //if (this.start) {
        //    if (timePerLetter == 0) {
        //        text.text = full;
        //    } else {
        //        if (full != null && text.text.Length < full.Length && timePerLetter > 0) {
        //            timer += Time.deltaTime;
        //        }

        //        if (timer >= timePerLetter) {
        //            char c = full[index++];
        //            if (c != ' ' && c != '*') {
        //                timer = 0;
        //                if (index % BLIP_INTERVAL == 0 && !GameObject.Find("Blip_0").GetComponent<AudioSource>().isPlaying) {
        //                    GameObject.Find("Blip_0").GetComponent<AudioSource>().Play();
        //                }
        //            }
        //            text.text += c;
        //        }
        //    }
        //}
    }

    public void WriteText(TextBox textBox) {
        StartCoroutine(WriteText(text, textBox));
    }

    static IEnumerator WriteText(Text text, TextBox textBox) {
        text.color = textBox.Color;
        switch (textBox.Effect) {
            case TextEffect.NONE:
                text.text = textBox.RawText;
                SoundView.Instance.Play(textBox.SoundLocation);
                yield break;
            case TextEffect.FADE_IN:
                throw new UnityException("This aint a thing yet!");
                yield break;
            case TextEffect.TYPE:
                string[] currentTextArray = new string[textBox.TextArray.Length];
                bool[] taggedText = new bool[currentTextArray.Length];

                //Prescreen and enable all html tags
                for (int i = 0; i < currentTextArray.Length; i++) {
                    if (Regex.IsMatch(textBox.TextArray[i], "<.*?>")) {
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
                            SoundView.Instance.Play(textBox.SoundLocation);
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

    /**
     * Tells the textBox to type a certain string at a rate, with a certain color
     * If this is called before a previous post() finishes, the new text being posted
     * will be overwritten by the new fullText
     */
    public void Post(string fullText, float lettersPerSecond, Color color) {
        if (lettersPerSecond < 0) {
            throw new UnityException("Bad input:" + lettersPerSecond + " is less than 0.");
        }
        this.full = Util.WordWrap(fullText, CHARS_PER_LINE);
        index = 0;
        this.timePerLetter = lettersPerSecond;
        this.text.color = color;
        this.start = true;
    }

    public void SetText(string fullText, float lettersPerSecond, Color color) {
        if (lettersPerSecond < 0) {
            throw new UnityException("Bad input:" + lettersPerSecond + " is less than 0.");
        }
        this.full = Util.WordWrap(fullText, CHARS_PER_LINE);
        index = 0;
        this.timePerLetter = lettersPerSecond;
        this.text.color = color;
    }

    public void Post() {
        this.start = true;
    }

    /**
     * Tells the textBox to type a certain string at a rate. white colored
     */
    public void Post(string fullText, float lettersPerSecond) {
        Post(fullText, lettersPerSecond, Color.white);
    }

    /**
     * True when there is no more text to type
     */
    public bool IsDonePosting() {
        return text.text.Length == full.Length;
    }
}
