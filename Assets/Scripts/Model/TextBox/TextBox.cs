using UnityEngine;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System;

public class TextBox {
    public virtual TextBoxType Type { get { return TextBoxType.TEXT; } }

    string[] _textArray;
    public string[] TextArray { get { return _textArray; } }

    string _rawText;
    public string RawText { get { return _rawText; } }

    Color _color;
    public Color Color { get { return _color; } }

    float _timePerLetter;
    public float TimePerLetter { get { return _timePerLetter; } }

    string _soundLoc;
    public string SoundLoc { get { return _soundLoc; } }

    TextEffect _effect;
    public TextEffect Effect { get { return _effect; } }

    bool _isDone;
    public bool IsDone { get; set; }

    void Init(string text, Color color, TextEffect effect = TextEffect.FADE_IN, string soundLocation = "Sounds/Blip_0", float timePerLetter = 0) {
        this._textArray = Regex.Matches(text, "(<.*?>)|\\.|.").Cast<Match>().Select(m => m.Value).ToArray(); //Splits by rich text, then letters
        this._rawText = text;
        this._color = color;
        this._timePerLetter = timePerLetter;
        this._soundLoc = soundLocation;
        this._effect = effect;
    }

    public TextBox(string text, Color color, TextEffect effect = TextEffect.FADE_IN, string soundLocation = "Sounds/Blip_0", float timePerLetter = 0) {
        Init(text, color, effect, soundLocation, timePerLetter);
    }

    public TextBox(string text, TextEffect effect = TextEffect.FADE_IN, string soundLocation = "Sounds/Blip_0", float timePerLetter = 0) {
        Init(text, Color.white, effect, soundLocation, timePerLetter);
    }

    public virtual void Write(GameObject textBoxPrefab, Action callBack) {
        textBoxPrefab.GetComponent<TextBoxView>().WriteText(this, callBack);
    }
}
