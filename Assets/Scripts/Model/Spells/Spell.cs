using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class Spell {
    public SpellFactory SpellFactory { get; set; }
    public Character Caster { get; set; }
    public Character Target { get; set; }
    public SpellDetails Other { get; set; }

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

    public Spell(SpellFactory spellFactory, Character caster, Character target, SpellDetails other = null) {
        this.SpellFactory = spellFactory;
        this.Caster = caster;
        this.Target = target;
        this.Other = other;
        this._currentString = SpellFactory.PRIMARY;

        _id = count++;
    }

    public void Tick() {
        if (!Current.IsFinished) {
            Current.Tick();
        } else {
            Target.Buffs.Remove(this);
        }
    }

    public override bool Equals(object obj) {
        if (obj == null) {
            return false;
        }
        Spell s = obj as Spell;
        if (s == null) {
            return false;
        }
        return this.Id.Equals(s.Id);
    }

    public override int GetHashCode() {
        return Id;
    }
}
