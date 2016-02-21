using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/**
 * This class represents the text on the top of the screen
 * You can set/show/hide the location, chapter, and main quest blurb here
 */
public class HeaderView : MonoBehaviour {

    [SerializeField]
    Text location;
    string Location { set { location.text = value; } }

    [SerializeField]
    Text chapter;
    string Chapter { set { chapter.text = value; } }

    [SerializeField]
    Text mainQuestBlurb;
    string Blurb { set { mainQuestBlurb.text = value; } }

    // Update is called once per frame
    void Update() {

    }

    public void EnableLocation(bool enable) {
        location.enabled = enable;
    }

    public void EnableChapter(bool enable) {
        chapter.enabled = enable;
    }

    public void EnableBlurb(bool enable) {
        mainQuestBlurb.enabled = enable;
    }
}
