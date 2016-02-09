using System.Collections.Generic;
using UnityEngine;

public class TextBoxHolderView : MonoBehaviour {
    public static TextBoxHolderView Instance { get; private set; }

    void Awake() { Instance = this; }

    public void AddTextBoxView(TextBox textBox) {
        GameObject g = (GameObject)GameObject.Instantiate(Resources.Load("TextBox"));
        TextBoxView textBoxView = g.GetComponent<TextBoxView>();
        Util.Parent(g, gameObject);
        textBoxView.WriteText(textBox);
    }

    void Update() { }
}
