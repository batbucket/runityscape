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
        PortraitView.SetResources(Character.Resources.Keys.ToArray());

        //Update ResourceViews' values
        foreach (KeyValuePair<ResourceType, Resource> pair in Character.Resources) {
            ResourceView rv = PortraitView.ResourceViews[pair.Key].resourceView;
            ResourceType resType = pair.Key;
            Resource res = pair.Value;

            rv.Text = resType.DisplayFunction(res.False, res.True);
            rv.SetBarScale(res.False, res.True);
        }
    }
}
