using Scripts.Game.Defined.SFXs;
using Scripts.Model.Characters;
using Scripts.Model.Spells;
using Scripts.Model.Stats;
using Scripts.View.Portraits;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.Presenter {

    /// <summary>
    /// Updates the portraitview from the character.
    /// </summary>
    public class CharacterPresenter {
        public Character Character { get; private set; }
        public PortraitView PortraitView { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterPresenter"/> class.
        /// </summary>
        /// <param name="character">The character associated with the portrait.</param>
        /// <param name="portraitView">The portrait associated with the character.</param>
        public CharacterPresenter(Character character, PortraitView portraitView) {
            this.Character = character;
            this.PortraitView = portraitView;
        }

        /// <summary>
        /// Ticks this instance.
        /// </summary>
        public void Tick() {
            SetupFuncs();
        }

        private RectTransform IconRect {
            get {
                if (PortraitView == null) {
                    return null;
                }
                return PortraitView.Image.rectTransform;
            }
        }

        private void ParentToEffects(GameObject go) {
            if (PortraitView != null) {
                Util.Parent(go, PortraitView.EffectsHolder);
            }
        }

        /// <summary>
        /// Setups functions in character.
        /// </summary>
        private void SetupFuncs() {
            Character.GetIconRectFunc = () => IconRect;
            Character.ParentToEffectsFunc = (go) => ParentToEffects(go);

            Character.Stats.AddSplat = (sd => AddHitsplat(sd));
            Character.Buffs.AddSplat = (sd => AddHitsplat(sd));
            Character.Equipment.AddSplat = (sd => AddHitsplat(sd));
        }

        /// <summary>
        /// Function that adds a hitsplat to the portrait
        /// </summary>
        /// <param name="details">The details needed to customize the splat.</param>
        private void AddHitsplat(SplatDetails details) {
            if (PortraitView != null && PortraitView.isActiveAndEnabled) {
                PortraitView.StartCoroutine(SFX.DoHitSplat(Character, details));
            }
        }
    }
}