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

    // Use this for initialization
    void Awake() {
        time = gameObject.GetComponentInChildren<TimeManager>();
        header = gameObject.GetComponentInChildren<HeaderManager>();
        textBoxes = gameObject.GetComponentInChildren<TextBoxHolderManager>();
        leftPortraits = gameObject.GetComponentInChildren<LeftPortraitHolderManager>();
        rightPortraits = gameObject.GetComponentInChildren<RightPortraitHolderManager>();
        actionGrid = gameObject.GetComponentInChildren<ActionGridManager>();

        currentPage = PageFactory.createPage(onEnter: ProcessFactory.createProcess("Check fruit",
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
        p1 = PageFactory.createPage(text: "Hello world!", actions: ActionGridFactory.createProcesses(d0: "Kill yourself", a0: () => { Debug.Log("Oh dear you are dead!"); }));
	}

	// Update is called once per frame
	void Update () {
        if (!initializedPage) {
            currentPage.onEnter();
            TextBoxFactory.createTextBox(textBoxes, currentPage.getText(), 0.001f, Color.white);
            initializedPage = true;
        }
        switch(currentPage.getPageType()) {
            case PageType.NORMAL:
                actionGrid.clearAllButtonAttributes();
                actionGrid.setButtonAttributes(currentPage.getActions());
                break;
            case PageType.BATTLE:

                break;
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

    void createGraph() {

    }
}
