using UnityEngine;
using UnityEngine.UI;

public class GoldView : MonoBehaviour {
    [SerializeField]
    private Text text;
    [SerializeField]
    private Text count;
    private Inventory inventory;

    private int Count {
        set { count.text = "" + value; }
    }

    public Inventory Inventory {
        set { inventory = value; }
    }

    private void Update() {
        this.text.enabled = inventory != null;
        this.count.enabled = inventory != null;
        if (inventory != null) {
            Count = inventory.Gold;
        }
    }
}
