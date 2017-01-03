using System;
using System.Collections.Generic;

[System.Serializable]
public struct SpellListSave : IRestorable<IList<SpellFactory>> {
    public SpellSave[] SpellSaves;

    public SpellListSave(IList<SpellFactory> list) {
        SpellSaves = new SpellSave[list.Count];

        int index = 0;
        foreach (SpellFactory s in list) {
            SpellSaves[index++] = new SpellSave(s);
        }
    }

    public IList<SpellFactory> Restore() {
        IList<SpellFactory> spells = new List<SpellFactory>();
        foreach (SpellSave s in SpellSaves) {
            spells.Add(s.Restore());
        }
        return spells;
    }
}