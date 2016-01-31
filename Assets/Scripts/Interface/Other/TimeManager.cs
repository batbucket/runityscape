using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/**
 * This manages the Time display on the bottom left corner
 * of the screen
 */
public class TimeManager : MonoBehaviour {
    Text dayText; //Days elapsed
    Text timeText; //Current time of day

    // Use this for initialization
    void Start() {
        dayText = GameObject.Find("Day").GetComponent<Text>();
        timeText = GameObject.Find("Time").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update() {

    }

    void SetDay(int dayNum) {
        dayText.text = string.Format("Day {0}", dayNum);
    }

    void SetTime(int newTime) {
        timeText.text = string.Format("{0}:00", newTime);
    }

    public void Enable(bool enable) {
        dayText.enabled = enable;
        timeText.enabled = enable;
    }
}
