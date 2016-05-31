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

        //Attempt to set ResourceViews
        PortraitView.SetResources(Character.Resources.Keys.Where(k => Character.Resources[k].IsVisible).ToArray());

        foreach (KeyValuePair<AttributeType, Attribute> pair in Character.Attributes) {
            PortraitView.SetAttributes(pair.Key, new PortraitView.AttributeBundle { falseValue = (int)pair.Value.False, trueValue = pair.Value.True });
        }

        //Update ResourceViews' values
        foreach (KeyValuePair<ResourceType, Resource> pair in Character.Resources) {
            if (pair.Value.IsVisible) {
                ResourceView rv = PortraitView.ResourceViews[pair.Key].resourceView;
                ResourceType resType = pair.Key;
                Resource res = pair.Value;

                rv.Text = resType.DisplayFunction((int)res.False, res.True);
                rv.SetBarScale(res.False, res.True);
            }
        }

        //Set buffs and durations
        PortraitView.SetBuffs(Character.Buffs.Select(s => s.Current).Where(b => b is TimedSpellComponent).Cast<TimedSpellComponent>().Select(b => new PortraitView.BuffParams { id = b.Spell.Id, sprite = b.Sprite, color = b.Color, duration = b.DurationText }).ToArray());

        this.PortraitView.Sprite = Util.GetSprite(Character.SpriteLoc);
    }
}
