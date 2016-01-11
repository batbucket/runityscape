using System.Collections.Generic;
using UnityEngine;

public class TextBoxHolderManager : MonoBehaviour {
    Queue<TextBoxManager> queue;
    TextBoxManager current;

    void Start() {
        queue = new Queue<TextBoxManager>();
    }

    public TextBoxManager addTextBox() {
        GameObject g = (GameObject)GameObject.Instantiate(Resources.Load("TextBox"));
        Util.parent(g, gameObject);
        return g.GetComponent<TextBoxManager>();
    }

    public void addTextBox(string fullText, float lettersPerSecond, Color color) {
        GameObject g = (GameObject)GameObject.Instantiate(Resources.Load("TextBox"));
        TextBoxManager textBox = g.GetComponent<TextBoxManager>();
        textBox.setText(fullText, lettersPerSecond, color);
        queue.Enqueue(textBox);
    }

    void Update() {
        if ((current == null || current.isDonePosting()) && queue.Count > 0) {
            current = queue.Dequeue();
            Util.parent(current.gameObject, gameObject);
            current.post();
        }
    }
}
