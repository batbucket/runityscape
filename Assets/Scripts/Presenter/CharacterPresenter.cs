using Scripts.Game.Defined.SFXs;
using Scripts.Model.Characters;
using Scripts.Model.Spells;
using Scripts.Model.Stats;
using Scripts.View.Portraits;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.Presenter {

    public struct SplatDetails {
        public readonly Sprite Sprite;
        public readonly Color Color;
        public readonly string Text;

        public SplatDetails(Color color, string text, Sprite sprite = null) {
            this.Color = color;
            this.Text = text;
            this.Sprite = sprite;
        }
    }

    public class CharacterPresenter {
        public Character Character { get; private set; }
        public PortraitView PortraitView { get; private set; }

        public CharacterPresenter(Character character, PortraitView portraitView) {
            this.Character = character;
            this.PortraitView = portraitView;
        }

        public void Tick() {
            SetupFuncs();
        }

        private void SetupFuncs() {
            Character.Stats.AddSplat = (sd => AddHitsplat(sd));
            Character.Buffs.AddSplat = (sd => AddHitsplat(sd));
            Character.Equipment.AddSplat = (sd => AddHitsplat(sd));
        }

        private void AddHitsplat(SplatDetails sd) {
            if (PortraitView != null && PortraitView.isActiveAndEnabled) {
                PortraitView.StartCoroutine(SFX.HitSplat(PortraitView.EffectsHolder, sd));
            }
        }
    }
}