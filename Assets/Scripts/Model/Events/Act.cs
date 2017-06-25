using Scripts.Model.Characters;
using Scripts.Model.Interfaces;
using Scripts.Model.Pages;
using Scripts.Model.Processes;
using Scripts.Model.TextBoxes;
using Scripts.Presenter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Scripts.Model.Acts {
    public static class Util {
        private const char TEXTBOX_SYMBOL = 't';
        private const char AVATARBOX_SYMBOL = 'a';
        private static readonly string REGEX = string.Format("((<({0}|{1})>)(?:(?!(<({0}|{1})>)).)*)", TEXTBOX_SYMBOL, AVATARBOX_SYMBOL); //"((<.>)(?:(?!(<.>)).)*)";
        private const int IDENTIFIER_POSITION = 1;
        private const int TAG_LENGTH = 3;

        public static IEnumerator SetupScene(Page page, params Act[] acts) {
            IButtonable[] originalArray = page.Actions;

            bool isSkipAll = false;

            int lastIndex = 0;

            for (int i = 0; i < acts.Length && !isSkipAll; i++) {
                lastIndex = i;
                bool isStepped = false;
                Act act = acts[i];
                page.Actions = new IButtonable[] {
                    new Process("Skip", "Skip the current event.", () => act.SkipAct())
                };
                yield return act.Play();
                page.Actions = new IButtonable[] {
                    new Process("Step", "Advance to the next event.", () => isStepped = true),
                    new Process("Skip All", "Skip the current scene.", () => isSkipAll = true)
                };
                yield return new WaitUntil(() => isStepped || i == (acts.Length - 1) || isSkipAll);
            }
            if (isSkipAll) {
                for (int i = lastIndex + 1; i < acts.Length; i++) {
                    Act act = acts[i];
                    act.SkipAct();
                    yield return act.Play();
                }
            }

            page.Actions = originalArray;
        }
        public static Act[] LongTalk(Page p, Character c, string big) {
            List<Act> events = new List<Act>();
            IList<TextBox> boxes = SplitBigDialogue(p, c, big);

            for (int i = 0; i < boxes.Count; i++) {
                events.Add(new TextAct(boxes[i]));
            }
            return events.ToArray();
        }

        /// <summary>
        /// Use regex to split up a long string into an Emotes
        /// and Talks cutscene by a specific character
        /// </summary>
        private static IList<TextBox> SplitBigDialogue(Page p, Character c, string big) {
            List<string> matches = new List<string>();
            foreach (Match m in Regex.Matches(big, REGEX)) {
                matches.Add(m.Value);
            }
            IList<TextBox> boxes = new List<TextBox>();

            foreach (string match in matches) {
                char identifier = match[IDENTIFIER_POSITION];
                string text = match.Substring(TAG_LENGTH);

                TextBox box = null;
                if (identifier.Equals(TEXTBOX_SYMBOL)) {
                    box = new TextBox(c.Look.TextColor, text);
                } else if (identifier.Equals(AVATARBOX_SYMBOL)) {
                    box = new AvatarBox(p.GetSide(c), c.Look.Sprite, c.Look.TextColor, text);
                } else {
                    throw new UnityException("Unknown tag identifier: " + identifier);
                }
                boxes.Add(box);
            }
            return boxes;
        }
    }

    public abstract class Act {
        private bool hasEnded;
        private bool isSkip;

        public Act() { }

        public IEnumerator Play() {
            Coroutine co = Main.Instance.StartCoroutine(PlayTemplate());
            bool wasSkipped = false;
            while (!hasEnded && !wasSkipped) {
                if (isSkip) {
                    wasSkipped = true;
                    Skip();
                }
                yield return null;
            }
        }

        public void SkipAct() {
            this.isSkip = true;
        }

        protected abstract void Skip();

        protected abstract IEnumerator PlayHelper();

        private IEnumerator PlayTemplate() {
            yield return PlayHelper();
            hasEnded = true;
        }
    }

    public class TextAct : Act {
        private TextBox box;

        public TextAct(TextBox box) {
            this.box = box;
        }

        protected override IEnumerator PlayHelper() {
            Page.TypeText(box);
            while (!box.IsDone) {
                yield return null;
            }
        }

        protected override void Skip() {
            box.Skip();
        }
    }
}