using System.Collections.Generic;

public struct Items {
    public Item[] Inventory;
    public IDictionary<EquipmentType, EquippableItem> Equips;

    public Items(Item[] items, EquippableItem[] equips) {
        this.Inventory = items;

        this.Equips = new Dictionary<EquipmentType, EquippableItem>();
        for (int i = 0; i < equips.Length; i++) {
            EquippableItem e = equips[i];
            this.Equips.Add(e.EquipmentType, e);
        }
    }
    public Items(Item[] items) : this(items, new EquippableItem[0]) { }

    public Items(EquippableItem[] equips) : this(new Item[0], equips) { }
}
