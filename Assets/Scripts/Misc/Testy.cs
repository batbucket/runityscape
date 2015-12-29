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
        Process postText = new Process();
        TextBoxManager textBox = textBoxes.addTextBox();
        postText.setPlay(() => textBox.post("There are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration in some form, by injected humour, or randomised words which don't look even slightly believable. If you are going to use a passage of Lorem Ipsum, you need to be sure there isn't anything embarrassing hidden in the middle of text. All the Lorem Ipsum generators on the Internet tend to repeat predefined chunks as necessary, making this the first true generator on the Internet. It uses a dictionary of over 200 Latin words, combined with a handful of model sentence structures, to generate Lorem Ipsum which looks reasonable. The generated Lorem Ipsum is therefore always free from repetition, injected humour, or non-characteristic words etc.", .005f));
        Process killText = new Process();
        killText.setPlay(() => Destroy(textBox.gameObject));
        actionButtons.setButtonAttributes(postText, "Amit", 0, 0);
        actionButtons.setButtonAttributes(killText, "KILL", 0, 1);
    }

    // Update is called once per frame
    void Update() {

    }
}
