using UnityEngine;

public abstract class TimedSpellComponent : SpellComponent {
    Sprite _sprite;
    public Sprite Sprite { get { return _sprite; } }
    Color _color;
    public Color Color { get { return _color; } }

    float _totalDuration;
    public float TotalDuration {
        get { return _totalDuration; }
        set {
            Util.Assert(value >= 0, "TotalDuration must be non-negative!");
            _totalDuration = value;
            durationTimer = value;
        }
    }
    protected float durationTimer;
    public int DurationTimer { get { return (int)durationTimer; } }
    public string DurationText { get { return !IsIndefinite ? "" + DurationTimer : "INF"; } }

    public bool IsIndefinite { get; set; }

    public override bool IsFinished { get { return !IsIndefinite && durationTimer <= 0; } }

    public TimedSpellComponent(Sprite sprite,
                               bool isGood,
                               float totalDuration,
                               Result hit,
                               Result critical,
                               Result miss)
                               : base(hit, critical, miss) {
        this._sprite = sprite;
        this._color = isGood ? Color.green : Color.red;
        this._totalDuration = totalDuration;
        this.durationTimer = totalDuration;
        this.IsIndefinite = false;
    }

    public TimedSpellComponent(Sprite sprite,
                           bool isGood,
                           Result hit,
                           Result critical,
                           Result miss)
                           : base(hit, critical, miss) {
        this._sprite = sprite;
        this._color = isGood ? Color.green : Color.red;
        this._totalDuration = 0;
        this.IsIndefinite = true;
    }
}
