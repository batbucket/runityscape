using System;
using System.Collections.Generic;
using UnityEngine;

public class TextBoxHolderView : MonoBehaviour {

    [SerializeField]
    GameObject textBoxPrefab;
    [SerializeField]
    GameObject leftBoxPrefab;
    [SerializeField]
    GameObject rightBoxPrefab;
    [SerializeField]
    GameObject inputBoxPrefab;

    List<GameObject> children;
    IDictionary<TextBoxType, GameObject> textBoxes;

    const int TEXTBOX_LIMIT = 25;

    void Awake() {
        this.children = new List<GameObject>();
        textBoxes = new Dictionary<TextBoxType, GameObject>() {
            { TextBoxType.TEXT, textBoxPrefab },
            { TextBoxType.LEFT, leftBoxPrefab },
            { TextBoxType.RIGHT, rightBoxPrefab }
        };
    }

    public GameObject AddTextBoxView(TextBox textBox, Action callBack = null) {
        GameObject g = Instantiate(textBoxes[textBox.Type]);
        children.Add(g);
        Util.Parent(g, gameObject);
        textBox.Write(g, callBack);
        return g;
    }

    public InputBoxView AddInputBoxView() {
        GameObject g = Instantiate(inputBoxPrefab);
        children.Add(g);
        Util.Parent(g, gameObject);
        return g.GetComponent<InputBoxView>();
    }

    void Update() {
        children.RemoveAll(item => item == null);
        if (children.Count > TEXTBOX_LIMIT) {
            Destroy(children[0]);
            children.RemoveAt(0);
        }
    }
}
