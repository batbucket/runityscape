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

            //Attempt to set ResourceViews
            PortraitView.SetResources(Character.Stats.Dict.Keys.Where(k => StatType.RESOURCES.Contains(k)).ToArray());

            //Update ResourceViews' values
            foreach (KeyValuePair<StatType, Stat> pair in Character.Stats.Dict) {
                if (PortraitView != null && PortraitView.ResourceViews.ContainsKey(pair.Key)) {
                    ResourceView rv = PortraitView.ResourceViews[pair.Key].resourceView;
                    StatType resType = pair.Key;
                    Stat res = pair.Value;
                    rv.SetBarScale(res.Mod, res.Max);
                    rv.Text = "" + res.Mod;
                }
            }

            //TODO Buff stuff
            PortraitView.SetBuffs(Character.Buffs.Collection
                .Select(b =>
                new PortraitView.BuffParams {
                    id = b.GetHashCode(),
                    name = b.Name,
                    color = Color.white,
                    description = b.Description,
                    sprite = b.Sprite,
                    duration = b.DurationText
                })
                .ToArray()
                );

            this.PortraitView.Sprite = Character.Look.Sprite;
        }

        private void SetupFuncs() {
            Character.Stats.AddSplat = (sd => AddHitsplat(sd));
            Character.Buffs.AddSplat = (sd => AddHitsplat(sd));
        }

        private void AddHitsplat(SplatDetails sd) {
            PortraitView.StartCoroutine(SFXList.HitSplat(PortraitView.EffectsHolder, sd));
        }
    }
}