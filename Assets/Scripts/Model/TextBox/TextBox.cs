using Scripts.View.TextBoxes;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Scripts.Model.TextBoxes {

    /// <summary>
    /// Represents a box with text inside it.
    /// </summary>
    public class TextBox {
        private Color color;
        private TextEffect effect;
        private string rawText;
        private string soundLoc;
        private string[] textArray;
        private float timePerLetter;

        public TextBox(string text) : this(text, Color.white, TextEffect.FADE_IN, "Blip_0", 0) { }

        protected TextBox(string text, Color color, TextEffect effect, string soundLocation, float timePerLetter) {
            this.textArray = Regex.Matches(text, "(<.*?>)|\\.|.").Cast<Match>().Select(m => m.Value).ToArray(); //Splits by rich text, then letters
            this.rawText = text;
            this.color = color;
            this.timePerLetter = timePerLetter;
            this.soundLoc = soundLocation;
            this.effect = effect;
        }

        public Color Color { get { return color; } set { color = value; } }
        public TextEffect Effect { get { return effect; } set { effect = value; } }
        public bool IsDone { get; set; }
        public string RawText { get { return rawText; } }
        public string SoundLoc { get { return soundLoc; } }
        public string[] TextArray { get { return textArray; } }
        public float TimePerLetter { get { return timePerLetter; } }
        public virtual TextBoxType Type { get { return TextBoxType.TEXT; } }

        public virtual void Write(GameObject textBoxPrefab, Action callBack) {
            textBoxPrefab.GetComponent<TextBoxView>().WriteText(this, callBack);
        }
    }
}