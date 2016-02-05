using System.Collections.Generic;
using UnityEngine;

public class TextBoxHolderManager : MonoBehaviour {
    public static TextBoxHolderManager Instance { get; private set; }

    void Awake() { Instance = this; }

    public TextBoxManager AddTextBox() {
        GameObject g = (GameObject)GameObject.Instantiate(Resources.Load("TextBox"));
        Util.Parent(g, gameObject);
        return g.GetComponent<TextBoxManager>();
    }

    public void AddTextBox(string fullText, float lettersPerSecond, Color color) {
        GameObject g = (GameObject)GameObject.Instantiate(Resources.Load("TextBox"));
        TextBoxManager textBox = g.GetComponent<TextBoxManager>();
        Util.Parent(g, gameObject);
        textBox.SetText(fullText, lettersPerSecond, color);
        textBox.Post();
    }

    void Update() { }
}
