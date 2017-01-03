using Scripts.View.Portraits;

namespace Scripts.View.Effects {

    /// <summary>
    /// This class represents an effect affecting a Character's portraitview.
    /// </summary>
    public abstract class CharacterEffect : Effect {
        protected PortraitView target;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="target">Portrait to affect.</param>
        public CharacterEffect(PortraitView target) : base() {
            this.target = target;
        }

        public abstract void CancelEffect();
    }
}