using UnityEngine;
using UnityEngine.UI;

namespace Scripts.View.Other {

    /// <summary>
    /// This class represents the text on the top of the screen
    /// You can set/show/hide the location, chapter, and main quest blurb here
    /// </summary>
    public class HeaderView : MonoBehaviour {

        [SerializeField]
        private Text location;

        public string Location { set { location.text = value; } }
    }
}