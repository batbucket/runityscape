using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextBoxManager : MonoBehaviour {

    Text text;
    string fullText; //Whole text to be typed
    float lettersPerSecond; //Speed at which the letters appear

    int index;
    float timer;

    public const int BLIP_INTERVAL = 5;
    public const int CHARS_PER_LINE = 51;

	// Use this for initialization
	void Start () {
        text = gameObject.GetComponent<Text>();
	}

	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;

        if (text.text.Length < fullText.Length && timer >= lettersPerSecond) {
            char c = fullText[index++];
            if (c != ' ' && c != '*') {
                timer = 0;
                if (index % BLIP_INTERVAL == 0) {
                    GameObject.Find("Blip_0").GetComponent<AudioSource>().Play();
                }
            }
            text.text += c;
        }
	}

    public void post(string fullText, float lettersPerSecond) {
        if (lettersPerSecond <= 0) {
            throw new UnityException("Bad input:" + lettersPerSecond + " is either 0 or less than that.");
        }
        this.fullText += Util.wordWrap(fullText, CHARS_PER_LINE);
        this.lettersPerSecond = lettersPerSecond;
    }


}
