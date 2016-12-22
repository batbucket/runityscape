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

    private string tooltip;

    public string Text { set { text.text = value; } }
    public string Duration { set { duration.text = value; } }

    public string Tooltip {
        set {
            tooltip = value;
        }
    }

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

    private void OnMouseOver() {
        Game.Instance.Tooltip.MouseText = tooltip;
    }

    private void OnMouseExit() {
        Game.Instance.Tooltip.MouseText = "";
    }
}
