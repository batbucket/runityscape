using System.Linq;

[System.Serializable]
public struct InventorySave : IRestorable<Inventory> {
    public int Gold;
    public SpellListSave<Item> Items;

    public InventorySave(Inventory inv) {
        Gold = inv.Gold;
        Items = new SpellListSave<Item>(inv.Items);
    }

    public Inventory Restore() {
        Inventory inven = new Inventory();
        inven.Gold = this.Gold;
        foreach (SpellSave<Item> i in Items.SpellSaves) {
            inven.Add(i.Restore());
        }
        return inven;
    }
}