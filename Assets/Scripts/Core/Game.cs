using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour {
    static Game _instance;
    public static Game Instance { get { return _instance; } }

    [SerializeField]
    TimeView _time;
    public TimeView Time { get { return _time; } }

    [SerializeField]
    HeaderView _header;
    public HeaderView Header { get { return _header; } }

    [SerializeField]
    TextBoxHolderView _textBoxHolder;
    public TextBoxHolderView TextBoxHolder { get { return _textBoxHolder; } }

    [SerializeField]
    PortraitHolderView _leftPortraits;
    public PortraitHolderView LeftPortraits { get { return _leftPortraits; } }

    [SerializeField]
    PortraitHolderView _rightPortraits;
    public PortraitHolderView RightPortraits { get { return _rightPortraits; } }

    [SerializeField]
    ActionGridView _actionGrid;
    public ActionGridView ActionGrid { get { return _actionGrid; } }

    [SerializeField]
    TooltipManager _tooltip;
    public TooltipManager Tooltip { get { return _tooltip; } }

    [SerializeField]
    EffectsManager _effects;
    public EffectsManager Effect { get { return _effects; } }

    [SerializeField]
    SoundView _sound;
    public SoundView Sound { get { return _sound; } }

    public PagePresenter PagePresenter { get; private set; }
    public Character MainCharacter { get; private set; }

    Dictionary<string, bool> boolFlags;
    Dictionary<string, int> intFlags;

    public const float NORMAL_TEXT_SPEED = 0.001f;
    public const string DERP = "because I'm not paying a bunch of money to take shitty humanity classes";

    IDictionary<string, Page> pages;

    void Start() {
        _instance = this;
        boolFlags = new Dictionary<string, bool>();
        PagePresenter = new PagePresenter();
        MainCharacter = new Amit();
        pages = new Dictionary<string, Page>();
        Time.Enable(false);

        pages["primary"] = new ReadPage("Welcome to RunityScape.",
            processes: new Process[] {
                new Process("New Game", "Start a new game."),
                new Process("Load Game", "Load a saved game."),
                new Process("Debug", "Visit the Debug page.", () => PagePresenter.SetPage(pages["debug"]))
        });

        pages["debug"] = new ReadPage("What", "Hello world", mainCharacter: MainCharacter, left: new Character[] { MainCharacter }, right: new Character[] { new Steve(), new Steve() },
            processes: new Process[] {
                        new Process("Normal TextBox", "Say Hello world",
                            () => TextBoxHolder.AddTextBoxView(
                                new TextBox(DERP, Color.white, TextEffect.TYPE, "Sounds/Blip_0", .1f))),
                        new Process("LeftBox", "Say Hello world",
                            () => TextBoxHolder.AddAvatarBoxView(false, "crying_mudkip",
                                new TextBox(DERP, Color.white, TextEffect.TYPE, "Sounds/Blip_0", .1f))),
                        new Process("RightBox", "Say Hello world",
                            () => TextBoxHolder.AddAvatarBoxView(true, "crying_mudkip",
                                new TextBox(DERP, Color.white, TextEffect.TYPE, "Sounds/Blip_0", .1f))),
                        new Process("Test Battle", "You only <i>LOOK</i> human, don't you?", () => PagePresenter.SetPage(pages["debug1"])),
                        new Process("Steve Massacre", "Steve. It was nice to meet you. Goodbye.", () => { PagePresenter.SetPage(pages["debug2"]); Sound.Play("Music/99P"); }),
                        new Process("Shake yourself", "Literally U******E", () => { Effect.ShakeEffect(MainCharacter, .05f); }),
                        new Process("Fade yourself", "Literally U******E x2", () => { Effect.FadeAwayEffect(MainCharacter, 1.0f); }),
                        new Process(),
                        new Process(),
                        new Process(),
                        new Process(),
                        new Process("Back", "Go back to the main menu.", () => { PagePresenter.SetPage(pages["primary"]); })
        });
        pages["debug1"] = new BattlePage(text: "Hello world!", mainCharacter: MainCharacter, left: new Character[] { MainCharacter, new Amit() }, right: new Character[] { new Steve(), new Steve() });
        pages["debug2"] = new BattlePage(mainCharacter: new Steve(), left: new Character[] { new Steve(), new Steve(), new Steve(), new Steve(), new Steve() }, right: new Character[] { new Steve(), new Steve(), new Steve(), new Steve(), new Steve() });

        PagePresenter.SetPage(pages["primary"]);
    }

    void SetPage(string pageLoc) {
        Util.Assert(pages.ContainsKey(pageLoc), string.Format("Page: {0} does not exist!", pageLoc));
        PagePresenter.SetPage(pages[pageLoc]);
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
