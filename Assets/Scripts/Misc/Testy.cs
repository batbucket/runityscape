using UnityEngine;
using UnityEngine.UI;
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
        header.setBlurb("KILL");
        header.setChapter("LA");
        header.setLocation("DERP");
        time.setDay(99);
        time.setTime(12);
        PortraitManager fuckObama = leftPortraits.addPortrait();
        //fuckObama.setIconColor(Color.yellow);
        fuckObama.setIconImage(GameObject.Find("Amit").GetComponent<Image>().sprite);
        fuckObama.setPortraitName("Fuck Obama");
        ResourceManager resource = fuckObama.addResource();
        resource.setOverBarColor(Color.green);
        resource.setUnderBarColor(Color.red);
        resource.setFraction(87, 99);
        resource.setResourceName("HP");
        resource.setBarScale(87.0f / 99.0f);

        PortraitManager fuckObama3 = leftPortraits.addPortrait();
        fuckObama3.setIconColor(Color.green);
        fuckObama3.setIconImage(GameObject.Find("Amit").GetComponent<Image>().sprite);
        fuckObama3.setPortraitName("Fuck Obama3");
        ResourceManager resource3 = fuckObama3.addResource();
        resource3.setOverBarColor(Color.green);
        resource3.setUnderBarColor(Color.red);
        resource3.setFraction(87, 99);
        resource3.setResourceName("HP");
        resource3.setBarScale(87.0f / 99.0f);

        PortraitManager fuckObama2 = rightPortraits.addPortrait();
        fuckObama2.setIconColor(Color.blue);
        fuckObama2.setIconImage(GameObject.Find("Amit").GetComponent<Image>().sprite);
        fuckObama2.setPortraitName("Fuck Obama2");
        ResourceManager resource2 = fuckObama2.addResource();
        resource2.setOverBarColor(Color.green);
        resource2.setUnderBarColor(Color.red);
        resource2.setFraction(87, 99);
        resource2.setResourceName("HP");
        resource2.setBarScale(87.0f / 99.0f);

        PortraitManager fuckObama4 = rightPortraits.addPortrait();
        fuckObama4.setIconColor(Color.blue);
        fuckObama4.setIconImage(GameObject.Find("Amit").GetComponent<Image>().sprite);
        fuckObama4.setPortraitName("Fuck Obama4");
        ResourceManager resource4 = fuckObama4.addResource();
        resource4.setOverBarColor(Color.green);
        resource4.setUnderBarColor(Color.red);
        resource4.setFraction(87, 99);
        resource4.setResourceName("HP");
        resource4.setBarScale(87.0f / 99.0f);
    }

    // Update is called once per frame
    void Update() {

    }
}
