using Scripts.Model.Characters;
using Scripts.Model.Spells;
using System;
using System.Collections;

public interface IPlayable : IComparable<IPlayable> {
    string Text {
        get;
    }
    Spell MySpell {
        get;
    }
    bool IsPlayable {
        get;
    }
    IEnumerator Play();
    IEnumerator Skip();
}