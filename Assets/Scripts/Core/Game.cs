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
    public const string DERP = "What";

    // Use this for initialization
    void Start() {
        _instance = this;
        boolFlags = new Dictionary<string, bool>();
        PagePresenter = new PagePresenter();
        MainCharacter = new Amit();
        GameObject canvas = GameObject.Find("Canvas");

        Page p3 = new BattlePage(mainCharacter: new Steve(), left: new Character[] { new Steve(), new Steve(), new Steve(), new Steve(), new Steve() }, right: new Character[] { new Steve(), new Steve(), new Steve(), new Steve(), new Steve() });
        Page p2 = new BattlePage(text: "Hello world!", mainCharacter: MainCharacter, left: new Character[] { new Amit(), new Amit() }, right: new Character[] { new Steve(), new Steve() });
        Page p1 = new ReadPage("What", "Hello world", MainCharacter, new Character[] { MainCharacter }, right: new Character[] { new Steve(), new Steve() },
            processes: new Process[] {
                new Process("Hello", "Say Hello world",
                    () => TextBoxHolder.AddTextBoxView(
                        new TextBox(DERP, Color.white, TextEffect.TYPE, "Sounds/Blip_0", .1f))),
                new Process("Test Battle", "You only <i>LOOK</i> human, don't you?", () => PagePresenter.SetPage(p2)),
                new Process("Steve Massacre", "Steve. It was nice to meet you. Goodbye.", () => { PagePresenter.SetPage(p3); Sound.Play("Music/CleytonRX"); })
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
