using Scripts.Model.Characters;
using Scripts.Model.Spells;
using System;
using System.Collections;

/// <summary>
/// Represents a turn-consuming action taken during a battle.
/// </summary>
/// <seealso cref="System.IComparable{IPlayable}" />
public interface IPlayable : IComparable<IPlayable> {
    /// <summary>
    /// Gets the text associated with the action.
    /// </summary>
    /// <value>
    /// The text.
    /// </value>
    string Text {
        get;
    }
    /// <summary>
    /// Gets the spell associated with the action.
    /// </summary>
    /// <value>
    /// My spell.
    /// </value>
    Spell MySpell {
        get;
    }
    /// <summary>
    /// Gets a value indicating whether this instance is playable.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is playable; otherwise, <c>false</c>.
    /// </value>
    bool IsPlayable {
        get;
    }
    /// <summary>
    /// Plays this instance.
    /// </summary>
    /// <returns>Coroutine</returns>
    IEnumerator Play();

    /// <summary>
    /// Skips this instance.
    /// </summary>
    /// <returns>Coroutine.</returns>
    IEnumerator Skip();
}