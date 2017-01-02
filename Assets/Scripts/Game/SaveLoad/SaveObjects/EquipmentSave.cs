using System.Linq;

[System.Serializable]
public struct EquipmentSave : IRestorable<Equipment> {
    public EquippedItemSave[] Equips;

    public EquipmentSave(Equipment equips) {
        Equips = new EquippedItemSave[equips.Count(e => e != null)];
        int index = 0;
        foreach (EquippableItem e in equips) {
            if (e != null) {
                Equips[index++] = new EquippedItemSave(e);
            }
        }
    }

    public Equipment Restore() {
        Equipment restoredEquips = new Equipment();
        foreach (EquippedItemSave e in Equips) {
            restoredEquips.Add(e.Restore());
        }
        return restoredEquips;
    }
}