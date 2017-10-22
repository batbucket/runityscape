using UnityEngine;
using UnityEngine.UI;

namespace Scripts.View.Other {

    /// <summary>
    /// This class represents the text on the top of the screen
    /// You can set/show/hide the location
    /// </summary>
    public class HeaderView : MonoBehaviour {

        [SerializeField]
        private Text location;

        /// <summary>
        /// Sets the location.
        /// </summary>
        /// <value>
        /// The location.
        /// </value>
        public string Location { set { location.text = value; } }
    }
}