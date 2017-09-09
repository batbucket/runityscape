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
    /// Details needed to add a hitsplat
    /// </summary>
    public struct SplatDetails {
        public readonly Sprite Sprite;
        public readonly Color Color;
        public readonly string Text;

        /// <summary>
        /// Initializes a new instance of the <see cref="SplatDetails"/> struct.
        /// </summary>
        /// <param name="color">The color of the hitsplat.</param>
        /// <param name="text">The text of the splat.</param>
        /// <param name="sprite">The sprite of the plat.</param>
        public SplatDetails(Color color, string text, Sprite sprite = null) {
            this.Color = color;
            this.Text = text;
            this.Sprite = sprite;
        }
    }


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

        /// <summary>
        /// Setups functions in character.
        /// </summary>
        private void SetupFuncs() {
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
                PortraitView.StartCoroutine(SFX.DoHitSplat(PortraitView.EffectsHolder, details));
            }
        }
    }
}