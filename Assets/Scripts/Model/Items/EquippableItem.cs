using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public abstract class EquippableItem : Item {
    readonly EquipmentType _equipmentType;
    public EquipmentType EquipmentType { get { return _equipmentType; } }
    const string SELF_EQUIP_TEXT = "{1} equipped <color=yellow>{2}</color>!";
    const string OTHER_EQUIP_TEXT = "{0} equipped {1} with <color=yellow>{2}</color>.";

    public readonly IDictionary<AttributeType, PairedInt> bonuses;

    public EquippableItem(string name, string description, EquipmentType equipmentType, IDictionary<AttributeType, PairedInt> bonuses) : base(name, description) {
        this._equipmentType = equipmentType;
        this.bonuses = bonuses;
    }

    public override Hit CreateHit() {
        return new Hit(
            isState: (c, t, o) => {
                return true;
            },
            perform: (c, t, calc, o) => {
                Equip(t);
            },
            createText: (c, t, calc, o) => {
                return string.Format((c == t) ? SELF_EQUIP_TEXT : OTHER_EQUIP_TEXT, c.DisplayName, t.DisplayName, this.Name);
            }
        );
    }

    public void Equip(Character c) {
        //Remove buffs of previous equipped item
        Equipment e = c.Equipment;
        if (e.ContainsEquipment(EquipmentType)) {
            UnequipItemInSlot(c);
        }

        //Add buffs of this equipped item
        this.ApplyBonus(c);

        //Item management
        c.Items.Remove(this);
        c.Equipment.Add(this);
    }

    public void UnequipItemInSlot(Character c) {
        EquippableItem current = c.Equipment.Get(EquipmentType);
        if (current != null) {
            current.CancelBonus(c);
            c.Items.Add(this);
            c.Equipment.Remove(this);
        }
    }

    private void CancelBonus(Character wielder) {
        foreach (KeyValuePair<AttributeType, PairedInt> pair in bonuses) {
            wielder.Attributes[pair.Key].FlatBonus -= pair.Value.FlatBonus;
            wielder.Attributes[pair.Key].PercentBonus /= pair.Value.PercentBonus;
        }
    }

    private void ApplyBonus(Character wielder) {
        foreach (KeyValuePair<AttributeType, PairedInt> pair in bonuses) {
            wielder.Attributes[pair.Key].FlatBonus += pair.Value.FlatBonus;
            wielder.Attributes[pair.Key].PercentBonus *= pair.Value.PercentBonus;
        }
    }
}
