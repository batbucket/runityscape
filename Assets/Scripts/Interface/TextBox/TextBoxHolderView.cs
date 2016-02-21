using System.Collections.Generic;
using UnityEngine;

public class TextBoxHolderView : MonoBehaviour {

    [SerializeField]
    GameObject textBoxPrefab;

    public void AddTextBoxView(TextBox textBox) {
        GameObject g = (GameObject)GameObject.Instantiate(textBoxPrefab);
        TextBoxView textBoxView = g.GetComponent<TextBoxView>();
        Util.Parent(g, gameObject);
        textBoxView.WriteText(textBox);
    }

    void Update() { }
}
