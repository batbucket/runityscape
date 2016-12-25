using UnityEngine;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System;

public class TextBox {
    public virtual TextBoxType Type { get { return TextBoxType.TEXT; } }

    string[] textArray;
    public string[] TextArray { get { return textArray; } }

    string rawText;
    public string RawText { get { return rawText; } }

    Color color;
    public Color Color { get { return color; } set { color = value; } }

    float timePerLetter;
    public float TimePerLetter { get { return timePerLetter; } }

    string soundLoc;
    public string SoundLoc { get { return soundLoc; } }

    TextEffect effect;
    public TextEffect Effect { get { return effect; } set { effect = value; } }

    bool _isDone;
    public bool IsDone { get; set; }

    private TextBox(string text, Color color, TextEffect effect, string soundLocation, float timePerLetter) {
        this.textArray = Regex.Matches(text, "(<.*?>)|\\.|.").Cast<Match>().Select(m => m.Value).ToArray(); //Splits by rich text, then letters
        this.rawText = text;
        this.color = color;
        this.timePerLetter = timePerLetter;
        this.soundLoc = soundLocation;
        this.effect = effect;
    }

    public TextBox(string text, TextEffect effect) : this(text, Color.white, effect, "Blip_0", 0) { }

    public virtual void Write(GameObject textBoxPrefab, Action callBack) {
        textBoxPrefab.GetComponent<TextBoxView>().WriteText(this, callBack);
    }
}
