using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour {
    public static Game Instance { get; private set; }

    public TimeView Time { get; private set; }
    public HeaderView Header { get; private set; }
    public TextBoxHolderView TextBoxHolder { get; private set; }
    public PortraitHolderView LeftPortraits { get; private set; }
    public PortraitHolderView RightPortraits { get; private set; }
    public ActionGridView ActionGrid { get; private set; }
    public TooltipManager Tooltip { get; private set; }

    public Page CurrentPage { get; private set; }
    public Character MainCharacter { get; private set; }
    bool initializedPage;

    Dictionary<string, bool> boolFlags;
    Dictionary<string, int> intFlags;

    public const float NORMAL_TEXT_SPEED = 0.001f;

    // Use this for initialization
    void Start() {
        Instance = this;
        Time = TimeView.Instance;
        Header = HeaderView.Instance;
        TextBoxHolder = TextBoxHolderView.Instance;
        LeftPortraits = LeftPortraitHolderView.Instance;
        RightPortraits = RightPortraitHolderView.Instance;
        ActionGrid = ActionGridView.Instance;
        Tooltip = TooltipManager.Instance;
        boolFlags = new Dictionary<string, bool>();

        CurrentPage = new ReadPage();
        Page p1 = new ReadPage(text: "What", tooltip: "Hello world", left: new List<Character>() { new Amit() },
            actionGrid:
            new List<Process>() {
                new Process("Hello", "Say Hello world",
                    () => TextBoxHolderView.Instance.AddTextBox(
                        new TextBox("When the player runs out of <color=blue>time in a game and is holding down any number</color> of colorButtons, the game will submit the last button held down as an answer and act accordingly",
                        Color.green, TextEffect.TYPE, "Sounds/Blip_0", .1f))),
                new Process("Test Hitsplat", "Test the thing!",
                    () => {
                        GameObject hitsplat = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Hitsplat"));
                        hitsplat.GetComponent<HitsplatView>().GrowAndFade("999!");
                        Util.Parent(hitsplat, TextBoxHolder.gameObject);
                    }
                )

            });
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

    public void UpdatePortraits(IList<Character> characters, PortraitHolderView portraitHolder) {
        foreach (Character c in characters) {
            PortraitView portrait = portraitHolder.AddPortrait(c.Name, c.GetSprite());
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

    public PortraitHolderView GetPortraitHolder(bool isRightSide) {
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
