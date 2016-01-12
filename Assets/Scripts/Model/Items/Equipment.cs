using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Equipment {
    Dictionary<string, EquippableItem> equipment;

	public Equipment() {
        equipment = new Dictionary<string, EquippableItem>();
        equipment.Add("Weapon", null);
        equipment.Add("Armor", null);
        equipment.Add("Trinket", null);
	}
}
