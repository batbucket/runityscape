using Scripts.Model.Items;

namespace Scripts.Model.World.Serialization.SaveObject {

    [System.Serializable]
    public struct EquippedItemSave : IRestorable<EquippableItem> {
        public EquipmentType EquipSlot;
        public string Item;

        public EquippedItemSave(EquippableItem e) {
            Item = Util.GetClassName(e);
            EquipSlot = e.EquipmentType;
        }

        public EquippableItem Restore() {
            return Util.StringToObject<EquippableItem>(Item);
        }
    }
}