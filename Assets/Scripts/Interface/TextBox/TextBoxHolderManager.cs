using System.Collections.Generic;
using UnityEngine;

public class TextBoxHolderManager : MonoBehaviour {
    Queue<TextBoxManager> queue;
    TextBoxManager current;

    void Start() {
        queue = new Queue<TextBoxManager>();
    }

    public TextBoxManager AddTextBox() {
        GameObject g = (GameObject)GameObject.Instantiate(Resources.Load("TextBox"));
        Util.Parent(g, gameObject);
        return g.GetComponent<TextBoxManager>();
    }

    public void AddTextBox(string fullText, float lettersPerSecond, Color color) {
        GameObject g = (GameObject)GameObject.Instantiate(Resources.Load("TextBox"));
        TextBoxManager textBox = g.GetComponent<TextBoxManager>();
        textBox.SetText(fullText, lettersPerSecond, color);
        queue.Enqueue(textBox);
    }

    void Update() {

        //(current == null || current.isDonePosting()) &&
        if (queue.Count > 0) {
            current = queue.Dequeue();
            Util.Parent(current.gameObject, gameObject);
            current.Post();
        }
    }
}
