using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour {
    TimeManager time;
    HeaderManager header;
    TextBoxHolderManager textBox;
    LeftPortraitHolderManager leftPortrait;
    RightPortraitHolderManager rightPortrait;
    ActionGridManager actionGrid;

    Page start;
    bool postedInitialText;


	// Use this for initialization
	void Awake () {
        time = gameObject.GetComponentInChildren<TimeManager>();
        header = gameObject.GetComponentInChildren<HeaderManager>();
        textBox = gameObject.GetComponentInChildren<TextBoxHolderManager>();
        leftPortrait = gameObject.GetComponentInChildren<LeftPortraitHolderManager>();
        rightPortrait = gameObject.GetComponentInChildren<RightPortraitHolderManager>();
        actionGrid = gameObject.GetComponentInChildren<ActionGridManager>();

        List<Process> derp = new List<Process>();
        derp.Add(ProcessFactory.createProcess("Human", () => TextBoxFactory.createTextBox(textBox, "Human.", 0.001f, Color.white), null));
        derp.Add(ProcessFactory.createProcess("Nice to meet", () => TextBoxFactory.createTextBox(textBox, "It was nice to meet you.", 0.001f, Color.white), null));
        derp.Add(ProcessFactory.createProcess("Byebye", () => TextBoxFactory.createTextBox(textBox, "Goodbye.", 0.001f, Color.white), null));
        start = PageFactory.createPage("Insert starting text here.", PageType.NORMAL, null, null, null, null, null, null, derp);
	}

	// Update is called once per frame
	void Update () {
        if (!postedInitialText) {
            TextBoxFactory.createTextBox(textBox, start.getText(), 0.001f, Color.white);

            for (int i = 0; i < start.getActions().GetLength(0); i++) {
                for (int j = 0; j < start.getActions().GetLength(1); j++) {
                    Debug.Log("I:" + i + " J: " + j);
                    if (start.getActions()[i, j] != null) {
                        actionGrid.setButtonAttributes(start.getActions()[i, j], start.getActions()[i, j].getDescription(), i, j);
                    }
                }
            }

            postedInitialText = true;
        }
	}

    public TimeManager getTimeManager() {
        return time;
    }

    public HeaderManager getHeader() {
        return header;
    }

    public TextBoxHolderManager getTextBoxHolder() {
        return textBox;
    }

    public PortraitHolderManager returnPortrait(bool left) {
        if (left) {
            return leftPortrait;
        } else {
            return rightPortrait;
        }
    }

    public ActionGridManager getActionGrid() {
        return actionGrid;
    }

    void createGraph() {

    }
}
