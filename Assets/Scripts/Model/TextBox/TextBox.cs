using Scripts.Model.Interfaces;
using Scripts.Model.Tooltips;
using Scripts.View.TextBoxes;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Scripts.Model.TextBoxes {

    /// <summary>
    /// Represents a box with text inside it.
    /// </summary>
    public class TextBox {
        private TextBoxView view;
        private Color color;
        private TextEffect effect;
        private string rawText;
        private string soundLoc;
        private string[] textArray;
        private float timePerLetter;
        private TooltipBundle tooltip;
        private bool isSkip;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextBox"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="tooltip">The tooltip.</param>
        public TextBox(string text, TooltipBundle tooltip) : this(text) {
            this.tooltip = tooltip;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextBox"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        public TextBox(string text) : this(text, Color.white, TextEffect.FADE_IN, "Blip_0", 0) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextBox"/> class.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <param name="text">The text.</param>
        public TextBox(Color c, string text) : this(text, c, TextEffect.FADE_IN, "Blip_0", 0) {
        }

        public TooltipBundle Tooltip {
            get {
                return tooltip;
            }
        }

        protected TextBox(string text, Color color, TextEffect effect, string soundLocation, float timePerLetter) {
            this.textArray = Regex.Matches(text, "(<.*?>)|\\.|.").Cast<Match>().Select(m => m.Value).ToArray(); //Splits by rich text, then letters
            this.rawText = text;
            this.color = color;
            this.timePerLetter = timePerLetter;
            this.soundLoc = soundLocation;
            this.effect = effect;
            this.tooltip = new TooltipBundle();
        }

        public Color Color { get { return color; } set { color = value; } }
        public TextEffect Effect { get { return effect; } set { effect = value; } }
        public bool IsDone { get; set; }
        public string RawText { get { return rawText; } }
        public string SoundLoc { get { return soundLoc; } }
        public string[] TextArray { get { return textArray; } }
        public float TimePerLetter { get { return timePerLetter; } }
        public virtual TextBoxType Type { get { return TextBoxType.TEXT; } }

        public void SetupPrefab(GameObject textBoxPrefab) {
            this.view = textBoxPrefab.GetComponent<TextBoxView>();
            SetupHelper(textBoxPrefab);
            view.IsSkip = isSkip;
        }

        protected virtual void SetupHelper(GameObject textBoxPrefab) {
        }

        public void Write() {
            view.WriteText(this);
        }

        public void Skip() {
            this.isSkip = true;
            if (view != null) {
                view.IsSkip = true;
            }
        }
    }
}