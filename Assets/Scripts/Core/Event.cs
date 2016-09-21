using System;
using UnityEngine;

public struct Event {
    public readonly Action action;
    public float delay;
    public readonly Func<bool> hasEnded;
    public readonly bool isOneShot;
    public bool HasPlayed { get; set; }
    public readonly TextBox textBox;

    public Event(Action action, float delay = 0, bool isOneShot = true, Func<bool> endCondition = null) {
        this.action = action;
        this.hasEnded = endCondition ?? (() => true);
        this.delay = delay;
        this.isOneShot = isOneShot;
        this.HasPlayed = false;
        this.textBox = null;
    }

    //Special Text-Event builders

    public Event(TextBox t, float delay = 0.1f, bool isOneShot = true) {
        this.action = () => Game.Instance.AddTextBox(t);
        this.delay = delay;
        this.hasEnded = () => true;
        this.isOneShot = isOneShot;
        this.HasPlayed = false;
        this.textBox = t;
    }
}