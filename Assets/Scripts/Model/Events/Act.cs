using Scripts.Model.Characters;
using Scripts.Model.Pages;
using Scripts.Model.TextBoxes;
using Scripts.Presenter;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Scripts.Model.Acts {

    /// <summary>
    /// Represents an event occurring in the game.
    /// This would be named Event but that class already exists.
    /// Can be chained together using Game's Cutscene method.
    /// </summary>
    public class Act {
        public readonly Action Action;
        public readonly float Delay;
        public readonly Func<bool> HasEnded;
        public readonly bool RequiresUserAdvance;
        public readonly Action SkipEvent;

        private const char AVATARBOX_SYMBOL = '/';

        /// <summary>
        /// Splits up a string
        /// "/Hello;World" to "/Hello" and ";World"
        /// </summary>
        private const string REGEX = "(?=>|/)";

        private const char TEXTBOX_SYMBOL = '>';

        public Act(Action a) : this(a, () => true, 0, false, null) {
        }

        public Act(TextBox t) : this(() => Game.Instance.TextBoxes.AddTextBox(t), () => t.IsDone, 0.5f, true, () => t.Skip()) {
        }

        public Act(string imageLoc, string text) : this(() => Game.Instance.Title.Play(imageLoc, text), () => Game.Instance.Title.IsDone, 0.5f, false) {
        }

        public Act(Page nextPage) : this(() => Game.Instance.CurrentPage = nextPage, () => true, 0.5f, false) {
        }

        private Act(Action action, Func<bool> endCondition, float delay, bool reqUserAdv, Action skip = null) {
            this.Action = action;
            this.HasEnded = endCondition ?? (() => true);
            this.Delay = delay;
            this.RequiresUserAdvance = reqUserAdv;
            this.SkipEvent = skip;
        }

        public static Act[] LongTalk(Character c, string big) {
            List<Act> events = new List<Act>();
            IList<TextBox> boxes = SplitBigDialogue(c, big);

            for (int i = 0; i < boxes.Count; i++) {
                events.Add(new Act(boxes[i]));
            }
            return events.ToArray();
        }

        /// <summary>
        /// Use regex to split up a long string into an Emotes
        /// and Talks cutscene by a specific character
        /// </summary>
        private static IList<TextBox> SplitBigDialogue(Character c, string big) {
            string[] split = Regex.Split(big, REGEX);
            IList<TextBox> boxes = new List<TextBox>();

            // The 0th index element is "" so skip it
            for (int i = 1; i < split.Length; i++) {
                char front = split[i][0];
                string rest = split[i].Substring(1);
                switch (front) {
                    case TEXTBOX_SYMBOL:
                        boxes.Add(c.Emote(rest));
                        break;

                    case AVATARBOX_SYMBOL:
                        boxes.Add(c.Talk(rest));
                        break;

                    default:
                        Util.Assert(false, "Unknown symbol in front of " + split[i]);
                        break;
                }
            }
            return boxes;
        }
    }
}