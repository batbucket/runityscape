using UnityEngine;
using System.Collections;

public class TickerSpellComponent : TimedSpellComponent {
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

    public TickerSpellComponent(Sprite sprite,
                                        bool isGood,
                                        float timePerTick,
                                        float totalDuration,
                                        Result hit,
                                        Result critical = null,
                                        Result miss = null)
                                        : base(sprite, isGood, totalDuration, hit, critical, miss) {
        this._timePerTick = timePerTick;
    }

    public TickerSpellComponent(Sprite sprite,
                                    bool isGood,
                                    float timePerTick,
                                    Result hit,
                                    Result critical = null,
                                    Result miss = null)
                                    : base(sprite, isGood, hit, critical, miss) {
        this._timePerTick = timePerTick;
    }

    public override void Tick() {
        if (IsIndefinite || (durationTimer -= Time.deltaTime) > 0) {
            if ((tickTimer -= Time.deltaTime) <= 0) {
                tickTimer = TimePerTick;
                Invoke();
            }
        } else {
            Spell.Target.Buffs.Remove(Spell);
        }
    }
}
