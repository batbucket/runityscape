using System;
using System.Collections.Generic;

public struct SpellListSave<T> : IRestorable<IList<T>> where T : SpellFactory {
    public SpellSave<T>[] SpellSaves;

    public SpellListSave(IList<T> list) {
        SpellSaves = new SpellSave<T>[list.Count];

        int index = 0;
        foreach (T s in list) {
            SpellSaves[index++] = new SpellSave<T>(s);
        }
    }

    public IList<T> Restore() {
        IList<T> spells = new List<T>();
        foreach (SpellSave<T> s in SpellSaves) {
            spells.Add(s.Restore());
        }
        return spells;
    }
}