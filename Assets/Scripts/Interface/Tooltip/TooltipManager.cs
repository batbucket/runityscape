using UnityEngine;
using UnityEngine.UI;

public class TooltipManager : MonoBehaviour {
    Text UIText { get; set; }
    public string Text { get { return Text; } set { UIText.text = value; } }

    // Use this for initialization
    void Awake() {
        this.UIText = gameObject.GetComponent<Text>();
    }

    public void Clear() {
        UIText.text = "";
    }
}
