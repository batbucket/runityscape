using System;
using UnityEngine;
using UnityEngine.UI;

public class BuffView : PooledBehaviour {
    [SerializeField]
    private Outline _outline;

    [SerializeField]
    private Text _text;
    public string Text { set { _text.text = value; } }

    [SerializeField]
    private Text _duration;
    public string Duration { set { _duration.text = value; } }

    public Color Color {
        set {
            _text.color = value;
            _outline.effectColor = value;
        }
    }

    public override void Reset() {
        Text = "";
        Duration = "";
        Color = Color.white;
    }
}
