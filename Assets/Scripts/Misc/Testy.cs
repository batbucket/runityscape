using UnityEngine;
using System.Collections;

public class Testy : MonoBehaviour {

    HeaderManager header;
    TimeManager time;
    TextBoxHolderManager textBoxes;
    ActionGridManager actionButtons;
    PortraitHolderManager leftPortraits;
    PortraitHolderManager rightPortraits;

    // Use this for initialization
    void Start() {
        header = GameObject.Find("HeaderHolder").GetComponent<HeaderManager>();
        time = GameObject.Find("TimeHolder").GetComponent<TimeManager>();
        textBoxes = GameObject.Find("TextBoxHolder").GetComponent<TextBoxHolderManager>();
        actionButtons = GameObject.Find("ButtonHolder").GetComponent<ActionGridManager>();
        leftPortraits = GameObject.Find("LeftPortraitHolder").GetComponent<LeftPortraitHolderManager>();
        rightPortraits = GameObject.Find("RightPortraitHolder").GetComponent<RightPortraitHolderManager>();
        //actionButtons.setButtonAttributes
    }

    // Update is called once per frame
    void Update() {

    }
}
