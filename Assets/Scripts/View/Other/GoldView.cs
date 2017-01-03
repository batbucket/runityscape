using UnityEngine;
using UnityEngine.UI;

namespace Scripts.View.Other {

    /// <summary>
    /// This class shows off the party's gold on interface.
    /// </summary>
    public class GoldView : MonoBehaviour {

        [SerializeField]
        private Text count;

        [SerializeField]
        private Text text;

        public int Count {
            set { count.text = "" + value; }
        }

        public bool IsEnabled {
            set {
                text.enabled = value;
                count.enabled = value;
            }
        }
    }
}