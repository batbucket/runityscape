using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/**
 * This manages the Time display on the bottom left corner
 * of the screen
 */
public class TimeView : MonoBehaviour {

    [SerializeField]
    private Text _day;
    public int Day { set { _day.text = string.Format("Day {0}", value); } }
    public bool IsDayEnabled {
        set {
            _day.enabled = value;
        }
    }

    [SerializeField]
    private Text _time;
    public int Time { set { _time.text = string.Format("{0} light hours", value); } }
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
