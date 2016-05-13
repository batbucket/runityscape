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
    public string Location { set { location.text = value; } }
    public bool IsLocationEnabled {
        set {
            location.enabled = value;
        }
    }

    [SerializeField]
    Text chapter;
    public string Chapter { set { chapter.text = value; } }
    public bool IsChapterEnabled {
        set {
            chapter.enabled = value;
        }
    }

    [SerializeField]
    Text mainQuestBlurb;
    public string Blurb { set { mainQuestBlurb.text = value; } }
    public bool IsBlurbEnabled {
        set {
            mainQuestBlurb.enabled = value;
        }
    }
}
