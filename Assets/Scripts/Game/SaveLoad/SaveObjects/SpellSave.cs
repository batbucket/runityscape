using System;

[System.Serializable]
public struct SpellSave<T> : IRestorable<T> where T : SpellFactory {
    public string Name;

    public SpellSave(T s) {
        Name = Util.GetClassName(s);
    }

    public T Restore() {
        return Util.StringToObject<T>(Name);
    }
}