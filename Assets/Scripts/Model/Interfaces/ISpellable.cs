using Scripts.Model.Characters;
using Scripts.Model.Processes;
using Scripts.Model.Spells;
using System;

namespace Scripts.Model.Interfaces {
    public interface ISpellable {
        SpellBook GetSpellBook();
    }
}