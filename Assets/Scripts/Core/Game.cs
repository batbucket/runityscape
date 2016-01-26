using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour {
    TimeManager time;
    HeaderManager header;
    TextBoxHolderManager textBoxes;
    LeftPortraitHolderManager leftPortraits;
    RightPortraitHolderManager rightPortraits;
    ActionGridManager actionGrid;
    TooltipManager tooltip;

    Page currentPage;
    bool initializedPage;

    Dictionary<string, bool> flags;
    Queue<PlayerCharacter> playerCharQueue;

    Page p1;

    public const float NORMAL_TEXT_SPEED = 0.001f;

    // Use this for initialization
    void Awake() {
        time = gameObject.GetComponentInChildren<TimeManager>();
        header = gameObject.GetComponentInChildren<HeaderManager>();
        textBoxes = gameObject.GetComponentInChildren<TextBoxHolderManager>();
        leftPortraits = gameObject.GetComponentInChildren<LeftPortraitHolderManager>();
        rightPortraits = gameObject.GetComponentInChildren<RightPortraitHolderManager>();
        actionGrid = gameObject.GetComponentInChildren<ActionGridManager>();
        tooltip = gameObject.GetComponentInChildren<TooltipManager>();
        flags = new Dictionary<string, bool>();
        playerCharQueue = new Queue<PlayerCharacter>();

        currentPage = new ReadPage(text: "What", tooltip: "Hello world", actionGrid: new List<Process>() { new Process("Hello", "Hello world", () => PostText("Hello world")) });


        //p1 = new Page(text: "Hello world!", pageType: PageType.BATTLE, left: new List<Character>() { new Amit(), new Amit() }, right: new List<Character>() { new Amit(), new Steve(), new Steve() },
        //    processes: new Process("Kill yourself", "", () => { Debug.Log("Oh dear you are dead!"); }));
    }

    // Update is called once per frame
    void Update() {
        Util.KillAllChildren(leftPortraits.gameObject);
        Util.KillAllChildren(rightPortraits.gameObject);
        actionGrid.SetButtonAttributes(currentPage.GetActions());
        UpdatePortraits(currentPage.GetCharacters(false), leftPortraits);
        UpdatePortraits(currentPage.GetCharacters(true), rightPortraits);
        SetTooltip(currentPage.GetTooltip());
        currentPage.Tick();
    }

    public void UpdatePortraits(List<Character> characters, PortraitHolderManager portraitHolder) {
        foreach (Character c in characters) {
            PortraitManager portrait = portraitHolder.AddPortrait(c.Name, c.GetSprite());
            foreach (KeyValuePair<ResourceType, Resource> r in c.Resources) {
                ResourceManager res = portrait.AddResource(r.Value.ShortName, r.Value.OverColor, r.Value.UnderColor, r.Value.False, r.Value.True);
            }
        }
    }

    public TimeManager GetTimeManager() {
        return time;
    }

    public HeaderManager GetHeader() {
        return header;
    }

    public TextBoxHolderManager GetTextBoxHolder() {
        return textBoxes;
    }

    public void PostText(string s) {
        PostText(s, Color.grey);
    }

    public void PostText(string s, Color color) {
        if (s == null) {
            return;
        }
        GetTextBoxHolder().AddTextBox(s, NORMAL_TEXT_SPEED, color);
    }

    public PortraitHolderManager GetPortraits(bool left) {
        if (left) {
            return leftPortraits;
        } else {
            return rightPortraits;
        }
    }

    public ActionGridManager GetActionGrid() {
        return actionGrid;
    }

    string Format(string s, params object[] o) {
        return string.Format(s, o);
    }

    public void SetPage(Page page) {
        page.OnEnter();
        currentPage.OnExit();
        Util.KillAllChildren(textBoxes.gameObject);
        this.currentPage = page;
        textBoxes.AddTextBox(currentPage.GetText(), 0, Color.white);
    }

    public Page GetPage() {
        return currentPage;
    }


    public List<Character> GetCharacters(bool isRightSide) {
        return currentPage.GetCharacters(isRightSide);
    }

    public List<Character> GetAllies(bool side) {
        return GetCharacters(side);
    }

    public List<Character> GetEnemies(bool side) {
        return GetCharacters(!side);
    }

    public Character GetRandomAlly(bool side) {
        return GetRandomCharacter(GetCharacters(side));
    }

    public Character GetRandomEnemy(bool side) {
        return GetRandomCharacter(GetCharacters(!side));
    }

    public List<Character> GetAll() {
        List<Character> allChars = new List<Character>();
        allChars.AddRange(GetCharacters(true));
        allChars.AddRange(GetCharacters(false));
        return allChars;
    }

    public static Character GetRandomCharacter(List<Character> characters) {
        return characters[UnityEngine.Random.Range(0, characters.Count)];
    }

    void SetFlag(string key, bool value) {
        flags.Add(key, value);
    }

    bool GetFlag(string key) {
        bool res;
        flags.TryGetValue(key, out res);
        return res;
    }

    public void SetTooltip(string s) {
        tooltip.Text = s;
    }

    void CreateGraph() {

    }
}
