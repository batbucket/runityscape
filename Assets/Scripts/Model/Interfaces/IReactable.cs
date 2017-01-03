using Scripts.Model.Spells;

namespace Scripts.Model.Interfaces {

    /// <summary>
    /// Represents any object that can react and witness spells.
    /// </summary>
    public interface IReactable {

        /// <summary>
        /// Called when this Character is affected by a Spell.
        /// </summary>
        /// <param name="s">Spell Character hit by</param>
        void React(Spell s);

        /// <summary>
        /// Called when any Character in the Page is affected by a Spell.
        /// </summary>
        /// <param name="s">Spell Character hit by</param>
        void Witness(Spell s);
    }
}