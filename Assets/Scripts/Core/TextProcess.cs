using System;
using UnityEngine;

public class TextProcess : Process {

    private TextBox textBox;
    private GameObject go;
    public TextProcess(string name = "",
                   string description = "",
                   TextBox textBox = null,
                   Action action = null,
                   Func<bool> condition = null)
        : base(name, description, action, condition) {
        this.textBox = textBox;
    }

    public override void Play() {
        if (go != null) {
            GameObject.Destroy(go);
        }
        go = Game.Instance.AddTextBox(textBox);
        base.Play();
    }
}
