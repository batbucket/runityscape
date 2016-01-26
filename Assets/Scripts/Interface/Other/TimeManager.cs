using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/**
 * This manages the Time display on the bottom left corner
 * of the screen
 */
public class TimeManager : MonoBehaviour {
    Text day; //Days elapsed
    Text time; //Current time of day

    // Use this for initialization
    void Start() {
        day = GameObject.Find("Day").GetComponent<Text>();
        time = GameObject.Find("Time").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update() {

    }

    public void SetDay(int dayNum) {
        day.text = string.Format("Day {0}", dayNum);
    }

    public void SetTime(string newTime) {
        time.text = newTime;
    }

    public void Enable(bool enable) {
        day.enabled = enable;
        time.enabled = enable;
    }
}
