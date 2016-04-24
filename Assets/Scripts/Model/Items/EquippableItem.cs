using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public abstract class EquippableItem : Item {
    readonly EquipmentType _equipmentType;
    public EquipmentType EquipmentType { get { return _equipmentType; } }
    const string SELF_EQUIP_TEXT = "* {1} equipped their {2}!";
    const string OTHER_EQUIP_TEXT = "* {0} equipped {1} with the {2}.";

    public EquippableItem(string name, string description, int count, EquipmentType equipmentType) : base(name, description, count) {
        this._equipmentType = equipmentType;
    }

    protected override void ConsumeResources(Character caster) {
        base.ConsumeResources(caster);
        caster.Selections[Selection.ITEM].Remove(this);
    }

    protected override IDictionary<string, SpellComponent> CreateComponents(Character caster, Character target, Spell spell) {
        return new Dictionary<string, SpellComponent>() {
            { SpellFactory.PRIMARY, new SpellComponent(
                hit: new Result(
                    isState: (c, t) => {
                        return true;
                    },
                    perform: (c, t, calc) => {
                        caster.Selections[Selection.EQUIP].Add(this);
                    },
                    createText: (c, t, calc) => {
                        return string.Format((c == t) ? SELF_EQUIP_TEXT : OTHER_EQUIP_TEXT, c.Name, t.Name, this.Name);
                    }
                )
            )}
        };
    }
}
