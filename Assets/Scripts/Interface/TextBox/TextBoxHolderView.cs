using System.Collections.Generic;
using UnityEngine;

public class TextBoxHolderView : MonoBehaviour {
    public static TextBoxHolderView Instance { get; private set; }

    void Awake() { Instance = this; }

    public TextBoxView AddTextBox() {
        GameObject g = (GameObject)GameObject.Instantiate(Resources.Load("TextBox"));
        Util.Parent(g, gameObject);
        return g.GetComponent<TextBoxView>();
    }

    public void AddTextBox(string fullText, float lettersPerSecond, Color color) {
        GameObject g = (GameObject)GameObject.Instantiate(Resources.Load("TextBox"));
        TextBoxView textBox = g.GetComponent<TextBoxView>();
        Util.Parent(g, gameObject);
        textBox.SetText(fullText, lettersPerSecond, color);
        textBox.Post();
    }

    public void AddTextBox(TextBox textBox) {
        GameObject g = (GameObject)GameObject.Instantiate(Resources.Load("TextBox"));
        TextBoxView textBoxView = g.GetComponent<TextBoxView>();
        Util.Parent(g, gameObject);
        textBoxView.WriteText(textBox);
    }

    void Update() { }
}
