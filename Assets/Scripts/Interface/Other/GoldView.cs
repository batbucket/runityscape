using UnityEngine;
using UnityEngine.UI;

public class GoldView : MonoBehaviour {
    [SerializeField]
    private Text text;
    [SerializeField]
    private Text count;

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
