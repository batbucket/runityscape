using System;

public struct SpellResult : IComparable<SpellResult> {

    public enum Type {
        UNDEFINED, MISS, CRITICAL, HIT
    }

    public Type resultType;
    public Func<Spell, bool> isState;
    public Action<Spell> calculateEffect;
    public Action<Spell> doEffect;
    public Func<Spell, string> createText;
    public Action<Spell> undoEffect;
    public Action<Spell> sfx;

    public int CompareTo(SpellResult other) {
        return this.resultType.CompareTo(other.resultType);
    }
}
