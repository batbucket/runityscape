using UnityEngine;
using UnityEngine.UI;

public class TooltipManager : MonoBehaviour {
    Text text;

    // Use this for initialization
    void Awake() {
        this.text = gameObject.GetComponent<Text>();
    }

    public void set(string s) {
        text.text = s;
    }

    public void clear() {
        text.text = "";
    }
}
