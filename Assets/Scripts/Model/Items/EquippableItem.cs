using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public abstract class EquippableItem : Item {
    readonly EquipmentType _equipmentType;
    public EquipmentType EquipmentType { get { return _equipmentType; } }
    const string SELF_EQUIP_TEXT = "{1} equipped <color=yellow>{2}</color>!";
    const string OTHER_EQUIP_TEXT = "{0} equipped {1} with <color=yellow>{2}</color>.";

    readonly IDictionary<AttributeType, PairedInt> bonuses;

    public EquippableItem(string name, string description, int count, EquipmentType equipmentType, IDictionary<AttributeType, PairedInt> bonuses) : base(name, description, count) {
        this._equipmentType = equipmentType;
        this.bonuses = bonuses;
    }

    public override Hit CreateHit() {
        return new Hit(
            isState: (c, t, o) => {
                return true;
            },
            perform: (c, t, calc, o) => {
                //Remove buffs of previous equipped item
                Equipment e = (Equipment)t.Selections[Selection.EQUIP];
                if (e.ContainsEquipment(EquipmentType)) {
                    EquippableItem current = e.Get(EquipmentType);
                    current.CancelBonus(t);
                }

                //Add buffs of this equipped item
                this.ApplyBonus(t);

                //Item management
                c.Selections[Selection.ITEM].Remove(this);
                t.Selections[Selection.EQUIP].Add(this);
            },
            createText: (c, t, calc, o) => {
                return string.Format((c == t) ? SELF_EQUIP_TEXT : OTHER_EQUIP_TEXT, c.DisplayName, t.DisplayName, this.Name);
            }
        );
    }

    public void CancelBonus(Character wielder) {
        foreach (KeyValuePair<AttributeType, PairedInt> pair in bonuses) {
            wielder.Attributes[pair.Key].Flat -= pair.Value.Flat;
            wielder.Attributes[pair.Key].Percent /= pair.Value.Percent;
        }
    }

    void ApplyBonus(Character wielder) {
        foreach (KeyValuePair<AttributeType, PairedInt> pair in bonuses) {
            wielder.Attributes[pair.Key].Flat += pair.Value.Flat;
            wielder.Attributes[pair.Key].Percent *= pair.Value.Percent;
        }
    }
}
