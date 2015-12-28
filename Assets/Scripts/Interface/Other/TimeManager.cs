using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeManager : MonoBehaviour {
    Text day;
    Text time;

    // Use this for initialization
    void Start() {
        day = GameObject.Find("Day").GetComponent<Text>();
        time = GameObject.Find("Time").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update() {

    }

    public void setDay(int dayNum) {
        day.text = string.Format("Day {0}", dayNum);
    }

    public void setTime(int hours) {
        time.text = string.Format("{0}:00", hours);
    }

    public void enable(bool enable) {
        day.enabled = enable;
        time.enabled = enable;
    }
}
