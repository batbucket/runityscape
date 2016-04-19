using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class Spell {
    public SpellFactory SpellFactory { get; set; }
    public Character Caster { get; set; }
    public Character Target { get; set; }

    public SpellComponent Current { get { return Components[CurrentString]; } }
    string _currentString;
    public string CurrentString { get { return _currentString; } set { _currentString = value; } }

    IDictionary<string, SpellComponent> _components;
    public IDictionary<string, SpellComponent> Components {
        get { return _components; }
        set {
            foreach (KeyValuePair<string, SpellComponent> pair in value) {
                pair.Value.Spell = this;
            }
            _components = value;
        }
    }

    static int count;
    int _id;
    public int Id { get { return _id; } }

    bool _isFinished;
    public bool IsFinished { get { return _isFinished; } set { _isFinished = value; } }

    public Spell(SpellFactory spellFactory, Character caster, Character target) {
        this.SpellFactory = spellFactory;
        this.Caster = caster;
        this.Target = target;
        this._currentString = SpellFactory.PRIMARY;

        _id = count++;
    }

    public void Tick() {
        if (!Current.IsTimedOut) {
            Current.Tick();
        }
    }
}
