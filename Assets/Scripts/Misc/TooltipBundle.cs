using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TooltipBundle {
    public Sprite Sprite;
    public string Title;
    public string Text;

    public TooltipBundle(Sprite sprite, string title, string text) {
        this.Sprite = sprite;
        this.Title = title;
        this.Text = text;
    }
}
