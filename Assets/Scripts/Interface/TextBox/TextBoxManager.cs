using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/**
 * This class manages a TextBox prefab
 */
public class TextBoxManager : MonoBehaviour {

    Text text; //Visible text
    string fullText; //Whole text to be typed
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
        if (this.start) {
            if (timePerLetter == 0) {
                text.text = fullText;
            } else {
                if (fullText != null && text.text.Length < fullText.Length && timePerLetter > 0) {
                    timer += Time.deltaTime;
                }

                if (timer >= timePerLetter) {
                    char c = fullText[index++];
                    if (c != ' ' && c != '*') {
                        timer = 0;
                        if (index % BLIP_INTERVAL == 0 && !GameObject.Find("Blip_0").GetComponent<AudioSource>().isPlaying) {
                            GameObject.Find("Blip_0").GetComponent<AudioSource>().Play();
                        }
                    }
                    text.text += c;
                }
            }
        }
	}

    /**
     * Tells the textBox to type a certain string at a rate, with a certain color
     * If this is called before a previous post() finishes, the new text being posted
     * will be overwritten by the new fullText
     */
    public void post(string fullText, float lettersPerSecond, Color color) {
        if (lettersPerSecond < 0) {
            throw new UnityException("Bad input:" + lettersPerSecond + " is less than 0.");
        }
        this.fullText = Util.wordWrap(fullText, CHARS_PER_LINE);
        index = 0;
        this.timePerLetter = lettersPerSecond;
        this.text.color = color;
        this.start = true;
    }

    public void setText(string fullText, float lettersPerSecond, Color color) {
        if (lettersPerSecond < 0) {
            throw new UnityException("Bad input:" + lettersPerSecond + " is less than 0.");
        }
        this.fullText = Util.wordWrap(fullText, CHARS_PER_LINE);
        index = 0;
        this.timePerLetter = lettersPerSecond;
        this.text.color = color;
    }

    public void post() {
        this.start = true;
    }

    /**
     * Tells the textBox to type a certain string at a rate. white colored
     */
    public void post(string fullText, float lettersPerSecond) {
        post(fullText, lettersPerSecond, Color.white);
    }

    /**
     * True when there is no more text to type
     */
    public bool isDonePosting() {
        return text.text.Length == fullText.Length;
    }
}
