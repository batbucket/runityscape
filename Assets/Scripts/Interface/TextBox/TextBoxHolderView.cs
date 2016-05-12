using System.Collections.Generic;
using UnityEngine;

public class TextBoxHolderView : MonoBehaviour {

    [SerializeField]
    GameObject textBoxPrefab;
    IList<GameObject> children;

    const int TEXTBOX_LIMIT = 25;

    void Awake() {
        this.children = new List<GameObject>();
    }

    public void AddTextBoxView(TextBox textBox) {
        GameObject g = (GameObject)GameObject.Instantiate(textBoxPrefab);
        children.Add(g);
        TextBoxView textBoxView = g.GetComponent<TextBoxView>();
        Util.Parent(g, gameObject);
        textBoxView.WriteText(textBox);
    }

    void Update() {
        if (children.Count > TEXTBOX_LIMIT) {
            Destroy(children[TEXTBOX_LIMIT]);
            children.RemoveAt(TEXTBOX_LIMIT);
        }
    }
}
