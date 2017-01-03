﻿using Scripts.Model.TextBoxes;
using Scripts.Presenter;
using Scripts.View.ObjectPool;
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
        public bool IsSkip;

        [SerializeField]
        private Image background;

        [SerializeField]
        private Outline outline;

        [SerializeField]
        private Text text;

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

        public override void Reset() {
            text.text = "";
            text.color = Color.white;
            background.color = Color.black;
            outline.effectColor = Color.white;
        }

        /// <summary>
        /// Type out a the characters in a TextBox object one by one
        /// </summary>
        /// <param name="textBox">Textbox to type out</param>
        /// <param name="callBack">Action to be called after textbox finishes typing</param>
        public virtual void WriteText(TextBox textBox, Action callBack = null) {
            StartCoroutine(TypeWriter(text, textBox, callBack));
        }

        private IEnumerator TypeWriter(Text text, TextBox textBox, Action callBack) {
            text.color = textBox.Color;

            switch (textBox.Effect) {
                case TextEffect.OLD:
                    text.text = textBox.RawText;
                    break;

                case TextEffect.FADE_IN:
                skipLoc:
                    text.text = textBox.RawText;
                    Color color = text.color;
                    color.a = 0;
                    text.color = color;
                    while (text.color.a < 1) {
                        Color c = text.color;
                        c.a += Time.deltaTime * 3;
                        text.color = c;
                        yield return null;
                    }
                    break;

                case TextEffect.TYPE:
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
                    string wrapper = "\u2007"; // Wordwrapped space
                    while (index < textBox.TextArray.Length) {
                        if (IsSkip) {
                            goto skipLoc;
                        }

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
                                        Game.Instance.Sound.PlaySound(textBox.SoundLoc);
                                    }
                                }
                            }

                            // Update the text that is visible to the user
                            text.text = string.Join("", currentTextArray);
                            index++;
                        }
                        yield return null;
                    }
                    break;
            }

            textBox.IsDone = true;
            if (callBack != null) {
                callBack.Invoke();
            }
        }
    }
}