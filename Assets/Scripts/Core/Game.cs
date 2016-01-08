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

    Page currentPage;
    bool initializedPage;

    bool ateApple;
    bool ateOrange;
    bool ateBanana;

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

        currentPage = new Page(onEnter: ProcessFactory.createProcess("Check fruit",
            () => {
                string message = "In front of you is a table with the following: {0}, {1}, {2}. What will you do?";
                object a = "An apple" + (ateApple ? " core" : "");
                object o = "an orange" + (ateOrange ? " peel" : "");
                object b = "a banana" + (ateBanana ? " peel" : "");
                currentPage.setText(string.Format(message, a, o, b));
            }), onExit: ProcessFactory.createProcess("Debug statement", () => Debug.Log("You left the page.")),
            actions: ActionGridFactory.createProcesses(d0: "Eat apple",
                                                       a0: () => { ateApple = true; TextBoxFactory.createTextBox(textBoxes,  "* You ate the apple.", 0.01f);  currentPage.clearAction(0); initializedPage = false; },
                                                       d1: "Eat orange",
                                                       a1: () => { ateOrange = true; TextBoxFactory.createTextBox(textBoxes, "* You ate the orange.", 0.01f); currentPage.clearAction(1); initializedPage = false; },
                                                       d2: "Eat banana",
                                                       a2: () => { ateBanana = true; TextBoxFactory.createTextBox(textBoxes, "* You ate the banana.", 0.01f); currentPage.clearAction(2); initializedPage = false; },
                                                       d3: "Leave",
                                                       a3: () => { setPage(p1); }
                                                       ));
        p1 = new Page(text: "Hello world!", pageType: PageType.BATTLE, left: new List<Character>() { new Amit() }, right: new List<Character>() { new Steve(), new Steve() }, actions: ActionGridFactory.createProcesses(d0: "Kill yourself", a0: () => { Debug.Log("Oh dear you are dead!"); }));
	}

	// Update is called once per frame
	void Update () {
        Util.killChildren(leftPortraits.gameObject);
        Util.killChildren(rightPortraits.gameObject);
        actionGrid.clearAllButtonAttributes();

        switch (currentPage.getPageType()) {
            case PageType.NORMAL:
                actionGrid.setButtonAttributes(currentPage.getActions());
                break;
            case PageType.BATTLE:
                battleTick(currentPage.getCharacters(false), leftPortraits);
                battleTick(currentPage.getCharacters(true), rightPortraits);
                break;
        }

        if (!initializedPage) {
            currentPage.onEnter();
            TextBoxFactory.createTextBox(textBoxes, currentPage.getText(), 0.001f, Color.white);
            initializedPage = true;
        }
    }

    public void battleTick(List<Character> characters, PortraitHolderManager portraitHolder) {
        foreach (Character c in characters) {
            if (!initializedPage) {
                c.onStart(this);
            }
            int derp = (int) (100 * ((float) (new Amit().getAttribute(AttributeType.DEXTERITY).getFalse())) / c.getAttribute(AttributeType.DEXTERITY).getFalse());
            c.getResource(ResourceType.CHARGE).setTrue(derp);
            c.act(1, this);
            PortraitManager portrait = PortraitViewFactory.createPortrait(portraitHolder, c.getName(), c.getSprite());
            foreach (KeyValuePair<ResourceType, Resource> r in c.getResources()) {
                ResourceManager res = ResourceViewFactory.createResource(portrait, r.Value.getShortName(), r.Value.getOverColor(), r.Value.getUnderColor());
                res.setFraction(r.Value.getFalse(), r.Value.getTrue());
                res.setBarScale(r.Value.getFalse() / ((float)r.Value.getTrue()));
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
        postText(s, Color.white);
    }

    public void postText(string s, Color color) {
        if (s == null) {
            return;
        }
        TextBoxFactory.createTextBox(getTextBoxHolder(), s, NORMAL_TEXT_SPEED, color);
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
        currentPage.onExit();
        this.currentPage = page;
        initializedPage = false;
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
        return characters[UnityEngine.Random.Range(0, characters.Count - 1)];
    }

    void createGraph() {

    }
}
