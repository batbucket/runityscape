using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Event {
    public readonly Action Action;
    public readonly float Delay;
    public readonly Func<bool> HasEnded;
    public readonly bool RequiresUserAdvance;
    public readonly Action SkipEvent;

    private Event(Action action, Func<bool> endCondition, float delay, bool reqUserAdv, Action skip = null) {
        this.Action = action;
        this.HasEnded = endCondition ?? (() => true);
        this.Delay = delay;
        this.RequiresUserAdvance = reqUserAdv;
        this.SkipEvent = skip;
    }

    public static Event[] LongTalk(Character c, string big) {
        List<Event> events = new List<Event>();
        IList<TextBox> boxes = SplitBigDialogue(c, big);

        for (int i = 0; i < boxes.Count; i++) {
            events.Add(new Event(boxes[i]));
        }
        return events.ToArray();
    }

    public Event(Action a) : this(a, () => true, 0, false, null) { }

    public Event(TextBox t) : this(() => Game.Instance.TextBoxes.AddTextBox(t), () => t.IsDone, 0.5f, true, () => t.Skip()) { }

    public Event(string imageLoc, string text) : this(() => Game.Instance.Title.Play(imageLoc, text), () => Game.Instance.Title.IsDone, 0.5f, false) { }

    public Event(Page nextPage) : this(() => Game.Instance.CurrentPage = nextPage, () => true, 0.5f, false) { }

    /// <summary>
    /// Splits up a string
    /// "/Hello;World" to "/Hello" and ";World"
    /// </summary>
    private const string REGEX = "(?=>|/)";
    private const char TEXTBOX_SYMBOL = '>';
    private const char AVATARBOX_SYMBOL = '/';

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