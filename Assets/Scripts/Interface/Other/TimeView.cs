using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/**
 * This manages the Time display on the bottom left corner
 * of the screen
 */
public class TimeView : MonoBehaviour {

    [SerializeField]
    Text _day;
    string Day { set { _day.text = value; } }
    public bool IsDayEnabled {
        set {
            _day.enabled = value;
        }
    }

    [SerializeField]
    Text _time;
    string Time { set { _time.text = value; } }
    public bool IsTimeEnabled {
        set {
            _time.enabled = value;
        }
    }

    public bool IsEnabled {
        set {
            IsDayEnabled = value;
            IsTimeEnabled = value;
        }
    }
}
