using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PagePresenter {
    public Page Page { get; private set; }
    public IList<CharacterPresenter> CharacterPresenters { get; private set; }

    public PagePresenter() {
        this.Page = new ReadPage();
        this.CharacterPresenters = new List<CharacterPresenter>();
    }

    public void SetPage(Page page) {
        page.OnEnter();
        this.Page.OnExit();
        this.Page = page;
        Util.KillAllChildren(Game.Instance.TextBoxHolder.gameObject);
        Game.Instance.TextBoxHolder.AddTextBoxView(new TextBox(Page.Text, Color.white, TextEffect.FADE_IN));
    }

    void SetCharacterPresenters(IList<Character> characters, PortraitHolderView portraitHolder) {
        portraitHolder.AddPortraits(characters.Select(c => c.Name).ToArray()); //Pass in characters' Names as parameter
        foreach (Character c in characters) {
            c.Presenter = new CharacterPresenter(c, portraitHolder.CharacterViews[c.Name].portraitView);
        }
    }

    void TickCharacterPresenters(IList<Character> characters) {
        foreach (Character c in characters) {
            c.Presenter.Tick();
        }
    }

    public void Tick() {
        Game.Instance.ActionGrid.ClearAll();
        Game.Instance.ActionGrid.SetButtonAttributes(Page.ActionGrid);
        Game.Instance.Tooltip.Text = Page.Tooltip;
        SetCharacterPresenters(Page.LeftCharacters, Game.Instance.LeftPortraits);
        SetCharacterPresenters(Page.RightCharacters, Game.Instance.RightPortraits);
        TickCharacterPresenters(Page.LeftCharacters);
        TickCharacterPresenters(Page.RightCharacters);
        Page.Tick();
    }
}
