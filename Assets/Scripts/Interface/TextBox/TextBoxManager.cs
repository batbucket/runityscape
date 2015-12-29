using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextBoxManager : MonoBehaviour {

    Text text;
    string fullText; //Whole text to be typed
    float timePerLetter; //Speed at which the letters appear

    int index;
    float timer;

    public const int BLIP_INTERVAL = 5;
    public const int CHARS_PER_LINE = 44;

	// Use this for initialization
	void Awake() {
        text = gameObject.GetComponent<Text>();
	}

    // Update is called once per frame
    void Update() {

        if (timePerLetter > 0) {
            timer += Time.deltaTime;
        }

        if (fullText != null && text.text.Length < fullText.Length && timer >= timePerLetter) {
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

    public void post(string fullText, float lettersPerSecond, Color color) {
        if (lettersPerSecond <= 0) {
            throw new UnityException("Bad input:" + lettersPerSecond + " is either 0 or less than that.");
        }
        this.fullText += Util.wordWrap(fullText, CHARS_PER_LINE);
        this.timePerLetter = lettersPerSecond;
        this.text.color = color;
    }

    public void post(string fullText, float lettersPerSecond) {
        post(fullText, lettersPerSecond, Color.white);
    }


    }
