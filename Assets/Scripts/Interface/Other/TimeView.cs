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

    [SerializeField]
    Text _time;
    string Time { set { _time.text = value; } }

    public void Enable(bool enable) {
        _day.enabled = enable;
        _time.enabled = enable;
    }
}
