using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/**
 * This class represents the text on the top of the screen
 * You can set/show/hide the location, chapter, and main quest blurb here
 */
public class HeaderManager : MonoBehaviour {
    Text location;
    Text chapter;
    Text mainQuestBlurb;

    // Use this for initialization
    void Start() {
        location = GameObject.Find("Location").GetComponent<Text>();
        chapter = GameObject.Find("Chapter").GetComponent<Text>();
        mainQuestBlurb = GameObject.Find("MainQuestBlurb").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update() {

    }

    public void setLocation(string loc) {
        location.text = loc;
    }

    public void enableLocation(bool enable) {
        location.enabled = enable;
    }

    public void setChapter(string chapterText) {
        chapter.text = chapterText;
    }

    public void enableChapter(bool enable) {
        chapter.enabled = enable;
    }

    public void setBlurb(string blurb) {
        mainQuestBlurb.text = blurb;
    }

    public void enableBlurb(bool enable) {
        mainQuestBlurb.enabled = enable;
    }
}
