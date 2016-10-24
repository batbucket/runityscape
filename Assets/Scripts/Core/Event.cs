using System;
using UnityEngine;

/// <summary>
/// Represents an Event.
/// Events can be strung together to create complex scripted game events.
/// </summary>
public struct Event {
    /// <summary>
    /// Action to occur.
    /// </summary>
    public readonly Action Action;

    /// <summary>
    /// Time in seconds before the action occurs.
    /// </summary>
    public float Delay;

    /// <summary>
    /// Returns true when this event has ended.
    /// </summary>
    public readonly Func<bool> HasEnded;

    /// <summary>
    /// True if this event can only occur once.
    /// </summary>
    public readonly bool IsOneShot;

    /// <summary>
    /// True if the event has occurred.
    /// </summary>
    public bool HasPlayed { get; set; }

    /// <summary>
    /// Textbox associated with this event.
    /// Used if the Text event constructor is used.
    /// </summary>
    public readonly TextBox TextBox;

    /// <summary>
    /// Normal constructor for a variety of events.
    /// </summary>
    /// <param name="action">Action this event will cause</param>
    /// <param name="delay">Time in seconds before this event will fire.</param>
    /// <param name="isOneShot">True if this event can only occur once.</param>
    /// <param name="endCondition">Returns true when this event is done.</param>
    public Event(Action action, float delay = 0, bool isOneShot = true, Func<bool> endCondition = null) {
        this.Action = action;
        this.HasEnded = endCondition ?? (() => true);
        this.Delay = delay;
        this.IsOneShot = isOneShot;
        this.HasPlayed = false;
        this.TextBox = null;
    }

    /// <summary>
    /// Special textbox constructor for events involving a textbox.
    /// </summary>
    /// <param name="t">TextBox to use.</param>
    /// <param name="delay">Time in seconds before the textbox will pop up.</param>
    /// <param name="isOneShot">True if this event can only occur once.</param>
    public Event(TextBox t, float delay = 0.1f, bool isOneShot = true) {
        this.Action = () => Game.Instance.AddTextBox(t);
        this.Delay = delay;
        this.HasEnded = () => true;
        this.IsOneShot = isOneShot;
        this.HasPlayed = false;
        this.TextBox = t;
    }
}