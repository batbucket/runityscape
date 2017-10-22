using Scripts.Model.Characters;
using Scripts.Model.Processes;
using Scripts.Model.Spells;
using System;

namespace Scripts.Model.Interfaces {
    /// <summary>
    /// Has a spellbook associated with it
    /// </summary>
    public interface ISpellable {

        /// <summary>
        /// Gets the spell book.
        /// </summary>
        /// <returns></returns>
        SpellBook GetSpellBook();
    }
}