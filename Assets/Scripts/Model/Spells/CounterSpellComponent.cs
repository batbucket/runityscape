using UnityEngine;
using System.Collections;
using System;

public class CounterSpellComponent : TimedSpellComponent {
    CounterSpellFactory counterSpellFactory;
    Func<Spell, Result, Calculation, bool> isReact;
    Func<Spell, Result, bool> isReactOverride;

    Func<Spell, Result, Calculation, bool> isWitness;
    Func<Spell, Result, bool> isWitnessOverride;

    bool isUnlimited;
    int count;
    int counter;

    public bool HasCount { get { return count > 0; } }

    void Init(CounterSpellFactory counterSpellFactory, Func<Spell, Result, Calculation, bool> isReact, Func<Spell, Result, bool> isReactOverride, Func<Spell, Result, Calculation, bool> isWitness, Func<Spell, Result, bool> isWitnessOverride) {
        this.counterSpellFactory = counterSpellFactory;
        this.isReact = isReact;
        this.isReactOverride = isReactOverride ?? ((s, r) => { return false; });
        this.isWitness = isWitness ?? ((s, r, c) => { return false; });
        this.isWitnessOverride = isWitnessOverride ?? ((s, r) => { return false; });
    }

    public CounterSpellComponent(Sprite sprite,
                               bool isGood,
                               CounterSpellFactory counterSpellFactory,
                               int count,
                               Func<Spell, Result, Calculation, bool> isReact,
                               float totalDuration,
                               Result hit,
                               Result critical = null,
                               Result miss = null,
                               Func<Spell, Result, Calculation, bool> isWitness = null,
                               Func<Spell, Result, bool> isReactOverride = null,
                               Func<Spell, Result, bool> isWitnessOverride = null)
                               : base(sprite, isGood, totalDuration, hit, critical, miss) {
        Init(counterSpellFactory, isReact, isReactOverride, isWitness, isWitnessOverride);
        this.count = count;
        this.counter = count;
        this.isUnlimited = false;
    }

    public CounterSpellComponent(Sprite sprite,
                           bool isGood,
                           CounterSpellFactory counterSpellFactory,
                           Func<Spell, Result, Calculation, bool> isReact,
                           float totalDuration,
                           Result hit,
                           Result critical = null,
                           Result miss = null,
                           Func<Spell, Result, Calculation, bool> isWitness = null)
                           : base(sprite, isGood, totalDuration, hit, critical, miss) {
        Init(counterSpellFactory, isReact, isReactOverride, isWitness, isWitnessOverride);
        this.isUnlimited = true;
    }

    public CounterSpellComponent(Sprite sprite,
                           bool isGood,
                           CounterSpellFactory counterSpellFactory,
                           Func<Spell, Result, Calculation, bool> isReact,
                           int count,
                           Result hit,
                           Result critical = null,
                           Result miss = null,
                           Func<Spell, Result, Calculation, bool> isWitness = null)
                           : base(sprite, isGood, hit, critical, miss) {
        Init(counterSpellFactory, isReact, isReactOverride, isWitness, isWitnessOverride);
        this.count = count;
        this.counter = count;
        this.isUnlimited = false;
    }

    public CounterSpellComponent(Sprite sprite,
                       bool isGood,
                       CounterSpellFactory counterSpellFactory,
                       Func<Spell, Result, Calculation, bool> isReact,
                       Result hit,
                       Result critical = null,
                       Result miss = null,
                       Func<Spell, Result, Calculation, bool> isWitness = null)
                       : base(sprite, isGood, hit, critical, miss) {
        Init(counterSpellFactory, isReact, isReactOverride, isWitness, isWitnessOverride);
        this.isUnlimited = true;
    }

    public override bool IsReactOverride(Spell spell, Result res) {
        return isReactOverride(spell, res);
    }

    public override void React(Spell s, Result r, Calculation c) {
        if (isReact(s, r, c) && HasCount) {
            counterSpellFactory.TryCast(Spell.Caster, s.Caster, new SpellDetails(s, r, c));
            counter--;
        }
    }

    public override bool IsWitnessOverride(Spell spell, Result res) {
        return isWitnessOverride(spell, res);
    }

    public override void Witness(Spell s, Result r, Calculation c) {
        if (isWitness(s, r, c) && HasCount) {
            counterSpellFactory.TryCast(Spell.Caster, s.Caster, new SpellDetails(s, r, c));
            counter--;
        }
    }

    public override void Tick() {
        if (counter <= 0 && !isUnlimited || !(IsIndefinite || (durationTimer -= Time.deltaTime) > 0)) {
            Spell.Target.Buffs.Remove(Spell);
        }
    }
}