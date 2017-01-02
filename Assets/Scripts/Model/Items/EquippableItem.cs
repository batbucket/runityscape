using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public abstract class EquippableItem : Item {
    readonly EquipmentType _equipmentType;
    public EquipmentType EquipmentType { get { return _equipmentType; } }
    const string SELF_EQUIP_TEXT = "{1} equipped <color=yellow>{2}</color>!";
    const string OTHER_EQUIP_TEXT = "{0} equipped {1} with <color=yellow>{2}</color>.";

    public readonly IDictionary<AttributeType, int> Bonuses;

    public EquippableItem(string name, string description, EquipmentType equipmentType, IDictionary<AttributeType, int> bonuses) : base(name, string.Format("{0} ({1})", description, BonusText(bonuses))) {
        this._equipmentType = equipmentType;
        this.Bonuses = bonuses;
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
        Equipment e = c.Equipment;
        if (e.ContainsEquipment(EquipmentType)) {
            UnequipItemInSlot(c);
        }

        c.Inventory.Remove(this);
        c.Equipment.Add(this);
    }

    public void UnequipItemInSlot(Character c) {
        EquippableItem current = c.Equipment.Get(EquipmentType);
        if (current != null) {
            c.Equipment.Remove(this);
            c.Inventory.Add(this);
        }
    }


    private static string BonusText(IDictionary<AttributeType, int> bonuses) {
        List<string> list = new List<string>();
        foreach (KeyValuePair<AttributeType, int> pair in bonuses) {
            int bonus = pair.Value;
            list.Add(string.Format("{0}{1} {2}", bonus >= 0 ? "+" : "-", bonus, pair.Key.ShortName));
        }
        return string.Join(",", list.ToArray());
    }
}
