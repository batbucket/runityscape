using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class TimedSpellComponent : SpellComponent {
    Sprite _sprite;
    public Sprite Sprite { get { return _sprite; } }
    Color _color;
    public Color Color { get { return _color; } }

    float _delay;
    public float Delay {
        get { return _delay; }
        set { _delay = value; }
    }

    float _timePerTick;
    public float TimePerTick {
        get { return _timePerTick; }
        set {
            Util.Assert(value >= 0, "TimePerTick must be non-negative!");
            _timePerTick = value;
            tickTimer = value;
        }
    }
    float tickTimer;

    float _totalDuration;
    public float TotalDuration {
        get { return _totalDuration; }
        set {
            Util.Assert(value >= 0, "TotalDuration must be non-negative!");
            _totalDuration = value;
            durationTimer = value;
        }
    }
    float durationTimer;
    public int DurationTimer { get { return (int)durationTimer; } }
    public string DurationText { get { return !IsIndefinite ? "" + DurationTimer : "INF"; } }

    public bool IsIndefinite { get; set; }

    bool _isTimedOut;
    public override bool IsTimedOut {
        get { return _isTimedOut; }
        set {
            Util.Assert(!IsIndefinite && value, "Cannot set IsTimedOut to true while IsIndefinite is true!");
            _isTimedOut = value;
        }
    }
    Func<bool> OtherEndCondition;
    Action onEnd;

    public TimedSpellComponent(Sprite sprite, bool isGood, Result hit, Result critical, Result miss, float timePerTick, float totalDuration, float delay = 0, Action onEnd = null, bool isIndefinite = false, Func<bool> otherEndCondition = null) : base(hit, critical, miss) {
        this._sprite = sprite;
        this._color = isGood ? Color.green : Color.red;
        this._timePerTick = timePerTick;
        this.tickTimer = timePerTick;
        this._totalDuration = totalDuration;
        this.durationTimer = totalDuration;
        this._delay = delay;
        this.OtherEndCondition = otherEndCondition ?? (() => false);
        this.onEnd = onEnd ?? (() => { });
        this.IsIndefinite = isIndefinite;
    }

    public override void Tick() {
        if (!this.IsTimedOut && !OtherEndCondition.Invoke()) {
            if ((Delay -= Time.deltaTime) <= 0 && (IsIndefinite || (durationTimer -= Time.deltaTime) > 0)) {
                if ((tickTimer -= Time.deltaTime) <= 0) {
                    tickTimer = TimePerTick;
                    SpellReactions();
                }
            } else {
                this.IsTimedOut = true;
                onEnd.Invoke();
            }
        }
    }
}
