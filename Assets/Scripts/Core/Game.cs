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

        currentPage = new ReadPage(text: "What", tooltip: "Hello world", actionGrid: new List<Process>() { new Process("Hello", "Hello world", () => postText("Hello world")) });


        //p1 = new Page(text: "Hello world!", pageType: PageType.BATTLE, left: new List<Character>() { new Amit(), new Amit() }, right: new List<Character>() { new Amit(), new Steve(), new Steve() },
        //    processes: new Process("Kill yourself", "", () => { Debug.Log("Oh dear you are dead!"); }));
    }

    // Update is called once per frame
    void Update() {
        Util.killChildren(leftPortraits.gameObject);
        Util.killChildren(rightPortraits.gameObject);
        actionGrid.setButtonAttributes(currentPage.getActions());
        updatePortraits(currentPage.getCharacters(false), leftPortraits);
        updatePortraits(currentPage.getCharacters(true), rightPortraits);
        tooltip.set(currentPage.getTooltip());
        currentPage.tick();
    }

    public void updatePortraits(List<Character> characters, PortraitHolderManager portraitHolder) {
        foreach (Character c in characters) {
            PortraitManager portrait = portraitHolder.addPortrait(c.getName(), c.getSprite());
            foreach (KeyValuePair<ResourceType, Resource> r in c.getResources()) {
                ResourceManager res = portrait.addResource(r.Value.getShortName(), r.Value.getOverColor(), r.Value.getUnderColor(), r.Value.getFalse(), r.Value.getTrue());
            }
        }
    }

    public TimeManager getTimeManager() {
        return time;
    }

    public HeaderManager getHeader() {
        return header;
    }

    public TextBoxHolderManager getTextBoxHolder() {
        return textBoxes;
    }

    public void postText(string s) {
        postText(s, Color.grey);
    }

    public void postText(string s, Color color) {
        if (s == null) {
            return;
        }
        getTextBoxHolder().addTextBox(s, NORMAL_TEXT_SPEED, color);
    }

    public PortraitHolderManager returnPortrait(bool left) {
        if (left) {
            return leftPortraits;
        } else {
            return rightPortraits;
        }
    }

    public ActionGridManager getActionGrid() {
        return actionGrid;
    }

    string format(string s, params object[] o) {
        return string.Format(s, o);
    }

    public void setPage(Page page) {
        page.onEnter();
        currentPage.onExit();
        Util.killChildren(textBoxes.gameObject);
        this.currentPage = page;
        textBoxes.addTextBox(currentPage.getText(), 0, Color.white);
    }

    public Page getPage() {
        return currentPage;
    }


    public List<Character> getCharacters(bool isRightSide) {
        return currentPage.getCharacters(isRightSide);
    }

    public List<Character> getAllies(bool side) {
        return getCharacters(side);
    }

    public List<Character> getEnemies(bool side) {
        return getCharacters(!side);
    }

    public Character getRandomAlly(bool side) {
        return getRandomChar(getCharacters(side));
    }

    public Character getRandomEnemy(bool side) {
        return getRandomChar(getCharacters(!side));
    }

    public List<Character> getAll() {
        List<Character> allChars = new List<Character>();
        allChars.AddRange(getCharacters(true));
        allChars.AddRange(getCharacters(false));
        return allChars;
    }

    public static Character getRandomChar(List<Character> characters) {
        return characters[UnityEngine.Random.Range(0, characters.Count)];
    }

    void setFlag(string key, bool value) {
        flags.Add(key, value);
    }

    bool getFlag(string key) {
        bool res;
        flags.TryGetValue(key, out res);
        return res;
    }

    public void setTooltip(string s) {
        tooltip.set(s);
    }

    void createGraph() {

    }
}
