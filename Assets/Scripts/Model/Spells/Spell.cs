using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Spell {
    public SpellFactory SpellFactory { get; set; }
    public string CastText { get; set; }
    public SpellResult.Type Result { get; set; }
    public Character Caster { get; set; }
    public Character Target { get; set; }
    public IDictionary<AttributeType, PairedInt> Attributes { get; set; }
    public IDictionary<ResourceType, PairedInt> Resources { get; set; }
    public IUndoableProcess Process { get; set; }

    public Spell(SpellFactory spellFactory, Character caster, Character target) {
        SpellFactory = spellFactory;
        CastText = "";
        Result = SpellResult.Type.UNDEFINED;
        Caster = caster;
        Target = target;

        Attributes = new SortedDictionary<AttributeType, PairedInt>();
        foreach (AttributeType val in AttributeType.ALL) {
            Attributes.Add(val, new PairedInt(0));
        }

        Resources = new SortedDictionary<ResourceType, PairedInt>();
        foreach (ResourceType val in ResourceType.ALL) {
            Resources.Add(val, new PairedInt(0));
        }


        Process = new UndoableProcess(spellFactory.Name, spellFactory.Description);
    }

    public void Invoke() {
        Process.Play();
    }

    public void NumericPlay() {
        MultipliedAction(1);
    }

    public void NumericUndo() {
        MultipliedAction(-1);
    }

    void MultipliedAction(int multiplier) {
        foreach (KeyValuePair<AttributeType, PairedInt> pair in this.Attributes) {
            this.Target.AddToAttribute(pair.Key, true, multiplier * pair.Value.True, true);
            this.Target.AddToAttribute(pair.Key, false, multiplier * pair.Value.False, true);
        }
        foreach (KeyValuePair<ResourceType, PairedInt> pair in this.Resources) {
            this.Target.AddToResource(pair.Key, true, multiplier * pair.Value.True, true);
            this.Target.AddToResource(pair.Key, false, multiplier * pair.Value.False, true);
        }
    }
}
