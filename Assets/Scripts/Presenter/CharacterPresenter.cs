using Scripts.Model.Characters;
using Scripts.Model.Stats.Attributes;
using Scripts.Model.Stats.Resources;
using Scripts.View.Portraits;
using System.Collections.Generic;
using System.Linq;

namespace Scripts.Presenter {

    public class CharacterPresenter {
        public Character Character { get; private set; }
        public PortraitView PortraitView { get; private set; }

        public CharacterPresenter(Character character, PortraitView portraitView) {
            this.Character = character;
            this.PortraitView = portraitView;
        }

        public void Tick() {
            //Attempt to set ResourceViews
            PortraitView.SetResources(Character.Resources.Keys.Where(k => Character.Resources[k].IsVisible && (Character.IsShowingBarCounts || !k.Equals(ResourceType.CHARGE))).ToArray());

            foreach (KeyValuePair<AttributeType, Attribute> pair in Character.Attributes) {
                PortraitView.SetAttributeViews(pair.Key, new PortraitView.AttributeBundle { falseValue = (int)pair.Value.False, trueValue = pair.Value.True });
            }

            //Update ResourceViews' values
            foreach (KeyValuePair<ResourceType, Resource> pair in Character.Resources) {
                if (PortraitView != null && PortraitView.ResourceViews.ContainsKey(pair.Key) && pair.Value.IsVisible) {
                    ResourceView rv = PortraitView.ResourceViews[pair.Key].resourceView;
                    ResourceType resType = pair.Key;
                    Resource res = pair.Value;

                    if (Character.IsShowingBarCounts) {
                        rv.Text = resType.DisplayFunction(res.False, res.True);
                    } else {
                        rv.Text = "";
                    }
                    rv.SetBarScale(res.False, res.True);
                }
            }

            PortraitView
                .SetBuffs(Character.Buffs.Where(b => b.DurationTimer > 0 || b.IsIndefinite)
                .Select(b =>
                new PortraitView.BuffParams {
                    id = b.GetHashCode(),
                    name = b.SpellFactory.Name,
                    abbreviation = b.SpellFactory.Abbreviation,
                    description = b.SpellFactory.Description,
                    color = b.SpellFactory.Color,
                    duration = b.DurationText
                })
                    .ToArray());

            this.PortraitView.Sprite = Util.GetSprite(Character.SpriteLoc);
        }
    }
}