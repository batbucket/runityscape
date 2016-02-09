using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PagePresenter {
    public Page Page { get; private set; }

    public PagePresenter() {
        Page = new ReadPage();
    }

    public void SetPage(Page page) {
        page.OnEnter();
        this.Page.OnExit();
        this.Page = page;

        Game.Instance.TextBoxHolder.AddTextBoxView(new TextBox(Page.Text, Color.white, TextEffect.FADE_IN));
    }

    void SetPortraits(IList<Character> characters, PortraitHolderView portraitHolder) {
        foreach (Character c in characters) {
            PortraitView portrait = portraitHolder.AddPortrait(c.Name, c.GetSprite());
            foreach (KeyValuePair<ResourceType, Resource> r in c.Resources) {
                portrait.AddResource(r.Value.ShortName, r.Value.OverColor, r.Value.UnderColor, r.Value.False, r.Value.True);
            }
        }
    }

    public void Tick() {
        Util.KillAllChildren(Game.Instance.LeftPortraits.gameObject);
        Util.KillAllChildren(Game.Instance.RightPortraits.gameObject);
        Game.Instance.ActionGrid.SetButtonAttributes(Page.ActionGrid);
        SetPortraits(Page.LeftCharacters, Game.Instance.LeftPortraits);
        SetPortraits(Page.RightCharacters, Game.Instance.RightPortraits);
        Game.Instance.Tooltip.Set(Page.Tooltip);
        Page.Tick();
    }
}
