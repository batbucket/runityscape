using System;

[System.Serializable]
public struct PartySave : IRestorable<Party> {
    public CharacterSave Leader;
    public CharacterSave[] Characters;
    public InventorySave Inventory;

    public PartySave(Party p) {
        Leader = new CharacterSave(p.Main);
        Characters = new CharacterSave[p.Members.Count];
        int index = 0;
        foreach (Character c in p.Members) {
            Characters[index++] = new CharacterSave(c);
        }
        Inventory = new InventorySave(p.Inventory);
    }

    public Party Restore() {
        Party p = new Party(Leader.Restore());
        foreach (CharacterSave c in Characters) {
            p.Add(c.Restore());
        }
        p.Inventory = Inventory.Restore();
        return p;
    }
}