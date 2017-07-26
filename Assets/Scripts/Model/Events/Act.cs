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
using Scripts.Model.Spells;
using Scripts.Game.Defined.SFXs;

namespace Scripts.Model.Acts {
    public static class ActUtil {
        private const char TEXTBOX_SYMBOL = 't';
        private const char AVATARBOX_SYMBOL = 'a';
        private static readonly string SYMBOL_TAG_DETECTION = string.Format("((<({0}|{1})>)(?:(?!(<({0}|{1})>)).)*)", TEXTBOX_SYMBOL, AVATARBOX_SYMBOL); //"((<.>)(?:(?!(<.>)).)*)";
        private const int IDENTIFIER_POSITION = 1;
        private const int TAG_LENGTH = 3;

        public static void SetupScene(Page page, params Act[] acts) {
            SetupScene(page, acts, new Action[0]);
        }

        public static void SetupScene(Page page, Act[] acts, params Action[] postActions) {
            Main.Instance.StartCoroutine(SetupSceneRoutine(page, acts, postActions));
        }

        public static Act[] LongTalk(Page p, Character c, string big, params Act[] postActions) {
            List<Act> events = new List<Act>();
            IList<TextBox> boxes = SplitBigDialogue(p, c, big);

            for (int i = 0; i < boxes.Count; i++) {
                events.Add(new TextAct(boxes[i]));
            }

            events.AddRange(postActions);

            return events.ToArray();
        }

        public static IEnumerator SetupSceneRoutine(Page page, Act[] acts) {
            yield return SetupSceneRoutine(page, acts, new Action[0]);
        }

        public static IEnumerator SetupSceneRoutine(Page page, Act[] acts, Action[] postActions) {
            Grid grid = new Grid(string.Empty);
            Main.Instance.PagePresenter.Override = grid;

            bool isSkipAll = false;
            int lastIndex = 0;

            for (int i = 0; i < acts.Length && !isSkipAll; i++) {
                lastIndex = i;
                bool isStepped = false;
                Act act = acts[i];
                if (act.IsSkippable) {
                    grid.List = new IButtonable[] {
                    new Process("Skip", "Skip the current event.", () => act.SkipAct())
                };
                }
                yield return act.Play();
                if (i < acts.Length - 1) {
                    grid.List = new IButtonable[] {
                    new Process("Step", "Advance to the next event.", () => isStepped = true),
                    new Process("Skip All", "Skip the current scene.", () => isSkipAll = true)
                };
                }
                yield return new WaitUntil(() => isStepped || i == (acts.Length - 1) || isSkipAll);
            }
            if (isSkipAll) {
                for (int i = lastIndex + 1; i < acts.Length; i++) {
                    Act act = acts[i];
                    act.SkipAct();
                    yield return act.Play();
                }
            }

            foreach (Action action in postActions) {
                action();
            }

            // If grid we're using is still in use, wipe it
            if (Main.Instance.PagePresenter.Override == grid) {
                Main.Instance.PagePresenter.Override = null;
            }
        }

        /// <summary>
        /// Use regex to split up a long string into an Emotes
        /// and Talks cutscene by a specific character
        /// </summary>
        private static IList<TextBox> SplitBigDialogue(Page p, Character c, string big) {
            List<string> matches = new List<string>();
            foreach (Match m in Regex.Matches(big, SYMBOL_TAG_DETECTION)) {
                matches.Add(m.Value);
            }
            IList<TextBox> boxes = new List<TextBox>();

            foreach (string match in matches) {
                char identifier = match[IDENTIFIER_POSITION];
                string text = string.Format(match.Substring(TAG_LENGTH), c.Look.DisplayName);

                TextBox box = null;
                if (identifier.Equals(TEXTBOX_SYMBOL)) {
                    box = new TextBox(text);
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
        public readonly bool IsSkippable;
        private bool hasEnded;
        private bool isSkip;

        public Act(bool isSkippable = true) {
            this.IsSkippable = isSkippable;
        }

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

        public TextAct(string message) {
            this.box = new TextBox(message);
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

    public class PageChangeAct : Act {
        private Page destination;

        public PageChangeAct(Page destination) {
            this.destination = destination;
        }

        protected override IEnumerator PlayHelper() {
            destination.Invoke();
            yield break;
        }

        protected override void Skip() {

        }
    }

    public class ActionAct : Act {
        private Action action;

        public ActionAct(Action action) {
            this.action = action;
        }

        protected override IEnumerator PlayHelper() {
            action.Invoke();
            yield break;
        }

        protected override void Skip() {

        }
    }

    public class BossTransitionAct : Act {
        private Page destination;
        private Sprite sprite;
        private string text;

        public BossTransitionAct(Page destination, Characters.Look boss) : base(false) {
            this.destination = destination;
            this.sprite = boss.Sprite;
            this.text = boss.Name;
        }

        protected override IEnumerator PlayHelper() {
            yield return SFX.PageTransition(sprite, text);
            destination.Invoke();
        }

        protected override void Skip() {
            Main.Instance.Title.Cancel();
        }
    }
}