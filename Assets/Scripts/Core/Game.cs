using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour {
    public TimeManager Time { get; private set; }
    public HeaderManager Header { get; private set; }
    public TextBoxHolderManager TextBoxHolder { get; private set; }
    public LeftPortraitHolderManager LeftPortraits { get; private set; }
    public RightPortraitHolderManager RightPortraits { get; private set; }
    public ActionGridManager ActionGrid { get; private set; }
    public TooltipManager Tooltip { get; private set; }

    public Page CurrentPage { get; private set; }
    bool initializedPage;

    Dictionary<string, bool> boolFlags;
    Dictionary<string, int> intFlags;

    public const float NORMAL_TEXT_SPEED = 0.001f;

    // Use this for initialization
    void Awake() {
        Time = gameObject.GetComponentInChildren<TimeManager>();
        Header = gameObject.GetComponentInChildren<HeaderManager>();
        TextBoxHolder = gameObject.GetComponentInChildren<TextBoxHolderManager>();
        LeftPortraits = gameObject.GetComponentInChildren<LeftPortraitHolderManager>();
        RightPortraits = gameObject.GetComponentInChildren<RightPortraitHolderManager>();
        ActionGrid = gameObject.GetComponentInChildren<ActionGridManager>();
        Tooltip = gameObject.GetComponentInChildren<TooltipManager>();
        boolFlags = new Dictionary<string, bool>();

        CurrentPage = new ReadPage();
        Page p1 = new ReadPage(text: "What", tooltip: "Hello world", actionGrid: new List<Process>() { new Process("Hello", "Say Hello world", () => PostText("Hello world")) });
        SetPage(p1);
        //p1 = new Page(text: "Hello world!", pageType: PageType.BATTLE, left: new List<Character>() { new Amit(), new Amit() }, right: new List<Character>() { new Amit(), new Steve(), new Steve() },
        //    processes: new Process("Kill yourself", "", () => { Debug.Log("Oh dear you are dead!"); }));
    }

    // Update is called once per frame
    void Update() {
        Util.KillAllChildren(LeftPortraits.gameObject);
        Util.KillAllChildren(RightPortraits.gameObject);
        ActionGrid.SetButtonAttributes(CurrentPage.ActionGrid);
        UpdatePortraits(CurrentPage.LeftCharacters, LeftPortraits);
        UpdatePortraits(CurrentPage.RightCharacters, RightPortraits);
        Tooltip.Set(CurrentPage.Tooltip);
        CurrentPage.Tick();
    }

    public void UpdatePortraits(IList<Character> characters, PortraitHolderManager portraitHolder) {
        foreach (Character c in characters) {
            PortraitManager portrait = portraitHolder.AddPortrait(c.Name, c.GetSprite());
            foreach (KeyValuePair<ResourceType, Resource> r in c.Resources) {
                portrait.AddResource(r.Value.ShortName, r.Value.OverColor, r.Value.UnderColor, r.Value.False, r.Value.True);
            }
        }
    }

    public void PostText(string s) {
        PostText(s, Color.grey);
    }

    public void PostText(string s, Color color) {
        if (s == null) {
            return;
        }
        TextBoxHolder.AddTextBox(s, NORMAL_TEXT_SPEED, color);
    }

    public PortraitHolderManager GetPortraitHolder(bool isRightSide) {
        if (!isRightSide) {
            return LeftPortraits;
        } else {
            return RightPortraits;
        }
    }

    void SetPage(Page page) {
        page.OnEnter();
        CurrentPage.OnExit();
        this.CurrentPage = page;
        TextBoxHolder.AddTextBox(CurrentPage.Text, 0, Color.white);
    }

    void SetFlag(string key, bool value) {
        boolFlags.Add(key, value);
    }

    bool GetFlag(string key) {
        bool res;
        boolFlags.TryGetValue(key, out res);
        return res;
    }

    void CreateGraph() {

    }
}
