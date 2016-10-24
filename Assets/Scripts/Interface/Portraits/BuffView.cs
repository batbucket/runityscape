using System;
using UnityEngine;
using UnityEngine.UI;

public class BuffView : PooledBehaviour {
    [SerializeField]
    private Outline outline;
    [SerializeField]
    private Text text;
    [SerializeField]
    private Text duration;
    [SerializeField]
    private Image image;

    public string Text { set { text.text = value; } }
    public string Duration { set { duration.text = value; } }

    public Color Color {
        set {
            text.color = value;
            outline.effectColor = value;
        }
    }

    public override void Reset() {
        Text = "";
        Duration = "";
        Color = Color.white;
        image.color = Color.black;
    }
}
