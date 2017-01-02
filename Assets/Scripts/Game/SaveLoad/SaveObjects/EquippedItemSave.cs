using System;

[System.Serializable]
public struct EquippedItemSave : IRestorable<EquippableItem> {
    public string Item;
    public EquipmentType EquipSlot;

    public EquippedItemSave(EquippableItem e) {
        Item = Util.GetClassName(e);
        EquipSlot = e.EquipmentType;
    }

    public EquippableItem Restore() {
        return Util.StringToObject<EquippableItem>(Item);
    }
}