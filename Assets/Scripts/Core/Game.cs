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

    public PagePresenter PagePresenter { get; private set; }
    public Character MainCharacter { get; private set; }

    Dictionary<string, bool> boolFlags;
    Dictionary<string, int> intFlags;

    public const float NORMAL_TEXT_SPEED = 0.001f;
    public const string DERP = "Damn, I never really interacted with the fandom and stumbled on this channel by chance, could someone  tell me what was the problem with Granberia? I mean, the guy in the video is making some reasonable points all around and then when it comes to how granberia was handled in part 3 he is like \"nothing of value was lost fellas, move along\" and I dont really get that. She has this really nice build up for part 1 and 2 as a nemesis and her boss fight was probably one of my favourite parts of the series, it had good theming, wasnt too over the top and it estabilishes her as the strongest knight; and then part 3 just dumpsters her, she probably ends up being the weakest knight (Tamamo is revealed to be a god; Alma Elma is suddendly the best fighter in the universe for no reason; Erubetie is arguably on the same level but she has so many neat gimmicks she does with her slime that she just feels more useful in a fight all-around), and all we get in term of character \"development\" about her in part 3 is that: 1. she is the only one who cant even into technology haha so funny 2. she's secretly Alma Elma's super submissive bitch (this fact alone makes her ending so much more dissapointing, takes meaning away from her actions because then it feels like she is just emulating what Alma Elma did to her onto Luka) Talk about sucking all the intimidation out of a character. Her ending was alright, I felt it could have used an aftermath, but so could a lot of other endings (of course only Alice gets one, GOTTA MAKE ALICE LOOK BETTER THAN EVERYONE DAMNIT). Im gonna guess Granberia must have had the worst fanbase, because this review of what happened to her feels like a knee-jerk reaction to fangays that wanted to picture her as this perfect waifu or something.?";

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
        PagePresenter = new PagePresenter();
        MainCharacter = new Amit();

        Page p2 = new BattlePage(text: "Hello world!", mainCharacter: MainCharacter, left: new Character[] { new Amit(), new Amit() }, right: new Character[] { new Steve() });
        Page p1 = new ReadPage("What", "Hello world", MainCharacter, new Character[] { new Amit() },
            processes: new Process[] {
                new Process("Hello", "Say Hello world",
                    () => TextBoxHolderView.Instance.AddTextBoxView(
                        new TextBox(DERP, Color.white, TextEffect.TYPE, "Sounds/Blip_0", .1f))),
                new Process("Test Hitsplat", "Test the thing!",
                    () => {
                        GameObject hitsplat = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Hitsplat"));
                        hitsplat.GetComponent<HitsplatView>().GrowAndFade("999!", Color.blue);
                        Util.Parent(hitsplat, TextBoxHolder.gameObject);
                    }
                ),
                new Process("Test Battle", "You only <i>LOOK</i> human, don't you?", () => PagePresenter.SetPage(p2))
        });
        PagePresenter.SetPage(p1);
    }

    // Update is called once per frame
    void Update() {
        PagePresenter.Tick();
    }

    public PortraitHolderView GetPortraitHolder(bool isRightSide) {
        if (!isRightSide) {
            return LeftPortraits;
        } else {
            return RightPortraits;
        }
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
