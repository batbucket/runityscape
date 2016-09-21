using System.Collections.Generic;
using System.Linq;

public class CharacterPresenter {
    public Character Character { get; private set; }
    public PortraitView PortraitView { get; private set; }

    public CharacterPresenter(Character character, PortraitView portraitView) {
        this.Character = character;
        this.PortraitView = portraitView;
    }

    public void Tick() {
        PortraitView.IsShowingBarCounts = Character.IsShowingBarCounts;

        //Attempt to set ResourceViews
        PortraitView.SetResources(Character.Resources.Keys.Where(k => Character.Resources[k].IsVisible).ToArray());

        foreach (KeyValuePair<AttributeType, Attribute> pair in Character.Attributes) {
            PortraitView.SetAttributes(pair.Key, new PortraitView.AttributeBundle { falseValue = (int)pair.Value.False, trueValue = pair.Value.True });
        }

        //Update ResourceViews' values
        foreach (KeyValuePair<ResourceType, Resource> pair in Character.Resources) {
            if (PortraitView != null && PortraitView.ResourceViews.ContainsKey(pair.Key) && pair.Value.IsVisible) {
                ResourceView rv = PortraitView.ResourceViews[pair.Key].resourceView;
                ResourceType resType = pair.Key;
                Resource res = pair.Value;

                if (PortraitView.IsShowingBarCounts) {
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
                abbreviation = b.SpellFactory.Abbreviation,
                color = b.SpellFactory.Color,
                duration = b.DurationText
            })
                .ToArray());

        this.PortraitView.Sprite = Util.GetSprite(Character.SpriteLoc);
    }
}
