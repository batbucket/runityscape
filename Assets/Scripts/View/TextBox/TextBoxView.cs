using Scripts.Model.TextBoxes;
using Scripts.Presenter;
using Scripts.View.ObjectPool;
using Scripts.View.Tooltip;
using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.View.TextBoxes {

    /// <summary>
    /// This class manages a TextBox prefab.
    /// </summary>
    public class TextBoxView : PooledBehaviour {
        public const int CHARS_FOR_SOUND = 1;

        /// <summary>
        /// Used to fast forward through the text writing.
        /// </summary>
        public bool IsSkip {
            private get {
                return isSkip;
            }
            set {
                isSkip = value;
            }
        }

        [SerializeField]
        private Image background;

        [SerializeField]
        private Outline outline;

        [SerializeField]
        private Text text;

        [SerializeField]
        private Tip tip;

        private float BackgroundAlpha {
            set {
                Util.SetImageAlpha(background, value);
            }
        }

        private float OutlineAlpha {
            set {
                Util.SetOutlineAlpha(outline, value);
            }
        }

        private float TextAlpha { set { Util.SetTextAlpha(text, value); } }

        private bool isSkip;

        public override void Reset() {
            text.text = "";
            text.color = Color.white;
            background.color = Color.black;
            outline.effectColor = Color.white;
            isSkip = false;

            if (tip != null) {
                tip.Reset();
            }
        }

        /// <summary>
        /// Type out a the characters in a TextBox object one by one
        /// </summary>
        /// <param name="textBox">Textbox to type out</param>
        /// <param name="callBack">Action to be called after textbox finishes typing</param>
        public virtual void WriteText(TextBox textBox) {
            StartCoroutine(TypeWriter(text, textBox));
            this.tip.Setup(textBox.Tooltip);
        }

        private IEnumerator TypeWriter(Text text, TextBox textBox) {
            text.color = textBox.Color;

            switch (textBox.Effect) {
                case TextEffect.OLD:
                    text.text = textBox.RawText;
                    break;

                case TextEffect.FADE_IN:
                    yield return FadeIn(textBox);
                    break;

                case TextEffect.TYPE:
                    yield return TypeWriter(textBox);
                    break;
            }

            textBox.IsDone = true;
        }

        private IEnumerator FadeIn(TextBox textBox) {
            text.text = textBox.RawText;
            float alpha = 0;
            while (alpha < 1) {
                alpha += Time.deltaTime * 3;
                Util.SetOutlineAlpha(outline, alpha);
                Util.SetTextAlpha(text, alpha);
                yield return null;
            }
        }

        /// <summary>
        /// Types out a textbox's contents. one by one.
        /// </summary>
        /// <param name="textBox">The text box.</param>
        /// <returns></returns>
        private IEnumerator TypeWriter(TextBox textBox) {
            string[] currentTextArray = new string[textBox.TextArray.Length];
            bool[] taggedText = new bool[currentTextArray.Length];

            // Prescreen and enable all html tags and spaces
            for (int i = 0; i < currentTextArray.Length; i++) {
                if (Regex.IsMatch(textBox.TextArray[i], "(<.*?>)")) {
                    currentTextArray[i] = textBox.TextArray[i];
                    taggedText[i] = true;
                }
            }
            float timer = 0;
            int index = 0;
            int charsPassedWithoutSound = 0;
            text.text = string.Join("", currentTextArray);
            string wrapper = "_";//"\u2007"; // Wordwrapped space

            bool hasSkipOccurred = false;
            while (index < textBox.TextArray.Length && !hasSkipOccurred) {
                // Type out letters every time we reach the timePerLetter time.
                if ((timer += Time.deltaTime) >= textBox.TimePerLetter) {
                    if (!taggedText[index]) {
                        //Preset forward characters to ensure wrapping
                        if (!string.Equals(currentTextArray[index], wrapper)) {
                            int start = index;

                            //Don't overshoot, only make one word, and don't replace any tagged text
                            while (start < textBox.TextArray.Length && !textBox.TextArray[start].Equals(" ") && !taggedText[start]) {
                                currentTextArray[start++] = wrapper;
                            }
                        }

                        // Update the currently showing text
                        currentTextArray[index] = textBox.TextArray[index];

                        //Don't reset timer or make sound on spaces
                        if (!Regex.IsMatch(textBox.TextArray[index], " ")) {
                            timer = 0;
                            if (--charsPassedWithoutSound <= 0) {
                                charsPassedWithoutSound = CHARS_FOR_SOUND;
                                Presenter.Main.Instance.Sound.PlaySound(textBox.SoundLoc);
                            }
                        }
                    }

                    // Update the text that is visible to the user
                    text.text = string.Join("", currentTextArray);
                    index++;

                    if (IsSkip) {
                        hasSkipOccurred = true;
                        yield return FadeIn(textBox);
                    }
                }
                yield return null;
            }
        }
    }
}