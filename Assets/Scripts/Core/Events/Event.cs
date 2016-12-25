using System;

public class Event {
    public readonly Action Action;
    public readonly float Delay;
    public readonly Func<bool> HasEnded;

    private Event(Action action, Func<bool> endCondition, float delay) {
        this.Action = action;
        this.HasEnded = endCondition ?? (() => true);
        this.Delay = delay;
    }

    public Event(TextBox t) : this(() => Game.Instance.TextBoxes.AddTextBox(t), () => t.IsDone, 0.5f) { }

    public Event(string imageLoc, string text) : this(() => Game.Instance.Title.Play(imageLoc, text), () => Game.Instance.Title.IsDone, 0.5f) { }

    public Event(Page nextPage) : this(() => Game.Instance.CurrentPage = nextPage, () => true, 0.5f) { }

    public Event(Action action) {
        this.Action = action;
        this.HasEnded = () => true;
        this.Delay = 0;
    }
}