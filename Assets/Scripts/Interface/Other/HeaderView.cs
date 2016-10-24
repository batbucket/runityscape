using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/**
 * This class represents the text on the top of the screen
 * You can set/show/hide the location, chapter, and main quest blurb here
 */
public class HeaderView : MonoBehaviour {

    [SerializeField]
    private Text location;
    public string Location { set { location.text = value; } }
}
