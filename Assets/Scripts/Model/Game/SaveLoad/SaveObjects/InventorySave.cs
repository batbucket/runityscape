using Scripts.Model.Items;
using Scripts.Model.Spells;
using System.Linq;

namespace Scripts.Model.World.Serialization.SaveObject {

    [System.Serializable]
    public struct InventorySave : IRestorable<Inventory> {
        public int Gold;
        public SpellListSave Items;

        public InventorySave(Inventory inv) {
            Gold = inv.Gold;
            Items = new SpellListSave(inv.Items.Cast<SpellFactory>().ToList());
        }

        public Inventory Restore() {
            Inventory inven = new Inventory();
            inven.Gold = this.Gold;
            foreach (SpellSave i in Items.SpellSaves) {
                inven.Add((Item)i.Restore());
            }
            return inven;
        }
    }
}