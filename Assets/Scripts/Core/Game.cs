using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text.RegularExpressions;

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
    SoundView _sound;
    public SoundView Sound { get { return _sound; } }

    [SerializeField]
    HotkeyButton _menuButton;
    public HotkeyButton MenuButton { get { return _menuButton; } }

    [SerializeField]
    EffectsManager _effect;
    public EffectsManager Effect { get { return _effect; } }

    public PagePresenter PagePresenter { get; private set; }
    public Character MainCharacter { get; private set; }

    public const float NORMAL_TEXT_SPEED = 0.001f;
    public static string DERP = "[8:33:40 AM] lanrete: did you know that you can get to the tutorial battle from the start by just spamming Q";

    IDictionary<string, Page> pages;
    IDictionary<string, string> pageLinks;

    static IDictionary<string, RightBox> FORBIDDEN_NAMES = new Dictionary<string, RightBox>() {
        { "Alestre", new RightBox("placeholder", "Redeemer. Please do not do that.", Color.yellow) },
    };

    string _pageIdentificationNumber;
    string PageID {
        set {
            Util.Assert(pages.ContainsKey(value), string.Format("Page: \"{0}\" does not exist!", value));
            ActionGrid.IsVisible = true;
            if (_pageIdentificationNumber != null && !pageLinks.ContainsKey(value)) {
                pageLinks.Add(value, _pageIdentificationNumber);
            }
            _pageIdentificationNumber = value;
            PagePresenter.SetPage(pages[value]);
        }
    }
    Page Page {
        get { return PagePresenter.Page; }
    }

    void Start() {
        _instance = this;
        PagePresenter = new PagePresenter();
        pages = new Dictionary<string, Page>();
        Time.IsEnabled = false;
        pageLinks = new Dictionary<string, string>();

        Header.Blurb = "";
        Header.Chapter = "";
        Header.Location = "";

        MenuButton.Hotkey = KeyCode.None;
        MenuButton.Process = new Process("Main Menu", "Return to the Main Menu.", () => { Start(); PageID = "primary"; });

        pages["primary"] = new ReadPage(
            tooltip: "Welcome to RunityScape.",
            processes: new Process[] {
                new Process("New Game", "Start a new game.", () => PageID = "newGame-Name"),
                new Process("Load Game", "Load a saved game.", condition: () => false),
                new Process("Debug", "Enter the debug page. ENTER AT YOUR OWN RISK.", () => PageID = "debug"),
                new Process(),

                new Process(),
                new Process(),
                new Process(),
                new Process(),

                new Process(),
                new Process(),
                new Process(),
                new Process("Exit", "Exit the application.", () => Application.Quit())
        });

        CreateDebug();
        PageID = "primary";
    }

    void CreateDebug() {
        pages["debug"] = new ReadPage("What", "Hello world", mainCharacter: new Amit(), left: new Character[] { new Amit() }, right: new Character[] { new Steve(), new Steve() },
            processes: new Process[] {
                new Process("Normal TextBox", "Say Hello world",
                    () => AddTextBox(
                        new TextBox(DERP, Color.white, TextEffect.FADE_IN, "Blip_0", .1f))),
                new Process("LeftBox", "Say Hello world",
                    () => AddTextBox(
                        new LeftBox("crying_mudkip", DERP, Color.white))),
                new Process("RightBox", "Say Hello world",
                    () => AddTextBox(
                        new RightBox("crying_mudkip", DERP, Color.white))),
                new Process("InputBox", "Type something",
                    () => TextBoxHolder.AddInputBoxView()),
                new Process("Test Battle", "You only <i>LOOK</i> human, don't you?", () => PagePresenter.SetPage(pages["debug1"])),
                new Process("Steve Massacre", "Steve. It was nice to meet you. Goodbye.", () => { PagePresenter.SetPage(pages["debug2"]); Sound.Loop("99P"); }),
                new Process("Shake yourself", "Literally U******E", () => { }),
                new OneShotProcess("Test OneShotProcess", action: () => Debug.Log("ya did it!")),
                new Process("deadm00se", "Text not displaying right.", () => AddTextBox(new RightBox("placeholder", "But this world is also inhabited by other creatures, who are not as light-touched as your kind. These abhorrent beings have begun sealing these purifiers, seeking to end my——humanity's presence here forever.", Color.white))),
                new Process(),
                new Process("Back", "Go back to the main menu.", () => { PagePresenter.SetPage(pages["primary"]); })
            }
        );
        pages["debug1"] = new BattlePage(text: "Hello world!", mainCharacter: new Amit(), left: new Character[] { new Amit(), new Amit() }, right: new Character[] { new Amit(), new Steve(), new Steve() });
        pages["debug2"] = new BattlePage(mainCharacter: new Steve(), left: new Character[] { new Steve(), new Steve(), new Steve(), new Steve(), new Steve() }, right: new Character[] { new Steve(), new Steve(), new Steve(), new Steve(), new Steve() });
    }

    public struct Event {
        public readonly Action action;
        public float delay;
        public readonly Func<bool> hasEnded;
        public readonly bool isOneShot;
        public bool HasPlayed { get; set; }
        public readonly TextBox textBox;

        public Event(Action action, float delay = 0, bool isOneShot = true, Func<bool> endCondition = null) {
            this.action = action;
            this.hasEnded = endCondition ?? (() => true);
            this.delay = delay;
            this.isOneShot = isOneShot;
            this.HasPlayed = false;
            this.textBox = null;
        }

        //Special Text-Event builders

        public Event(TextBox t, float delay = 1, bool isOneShot = true) {
            this.action = () => Instance.AddTextBox(t);
            this.delay = delay;
            this.hasEnded = () => t.IsDone;
            this.isOneShot = isOneShot;
            this.HasPlayed = false;
            this.textBox = t;
        }
    }

    public void Cutscene(params Event[] events) {
        StartCoroutine(Timeline(events, true));
    }

    public void Ordered(params Event[] events) {
        StartCoroutine(Timeline(events, false));
    }

    IEnumerator Timeline(Event[] events, bool hideGrid) {
        if (hideGrid) {
            ActionGrid.IsVisible = false;
            PagePresenter.Page.GetAll().ForEach(c => c.IsCharging = false);
        }
        if (events[0].textBox != null && events[0].IsExactly<TextBox>()) {
            events[0].delay = 0;
        }
        foreach (Event myEvent in events) {
            Event e = myEvent;
            if (!e.isOneShot || (e.isOneShot && !e.HasPlayed)) {
                float timer = 0;
                while (!Input.GetKey(KeyCode.LeftControl) && (timer += UnityEngine.Time.deltaTime) < e.delay) {
                    yield return null;
                }
                e.action.Invoke();
                while (!Input.GetKey(KeyCode.LeftControl) && !e.hasEnded.Invoke()) {
                    yield return null;
                }
                e.HasPlayed = true;
            }
        }
        if (hideGrid) {
            ActionGrid.IsVisible = true;
            PagePresenter.Page.GetAll().ForEach(c => c.IsCharging = true);
        }
        yield break;
    }

    public void Cutscene(params Event[][] events) {
        StartCoroutine(Timeline(events, true));
    }

    IEnumerator Timeline(Event[][] events, bool hideGrid) {
        if (hideGrid) {
            ActionGrid.IsVisible = false;
        }
        if (events[0][0].textBox != null && events[0].IsExactly<TextBox>()) {
            events[0][0].delay = 0;
        }
        foreach (Event[] myEventGroup in events) {
            Event[] eventGroup = myEventGroup;
            foreach (Event myEvent in eventGroup) {
                Event e = myEvent;
                if (!e.isOneShot || (e.isOneShot && !e.HasPlayed)) {
                    float timer = 0;
                    while (!Input.GetKey(KeyCode.LeftControl) && (timer += UnityEngine.Time.deltaTime) < e.delay) {
                        yield return null;
                    }
                    e.action.Invoke();
                    while (!Input.GetKey(KeyCode.LeftControl) && !e.hasEnded.Invoke()) {
                        yield return null;
                    }
                    e.HasPlayed = true;
                }
            }
        }
        if (hideGrid) {
            ActionGrid.IsVisible = true;
        }
        yield break;
    }


    BattlePage CreateBattle(string text = "",
                            string tooltip = "",
                            string location = "",
                            Character mainCharacter = null,
                            Character[] left = null,
                            Character[] right = null,
                            Action onFirstEnter = null,
                            Action onEnter = null,
                            Action onFirstExit = null,
                            Action onExit = null,
                            Action onTick = null
                            ) {
        return new BattlePage(text, tooltip, location, MainCharacter, left, right, onFirstEnter, onEnter, onFirstExit, onExit, onTick);
    }

    ReadPage CreateRead(string text = "",
                        string tooltip = "",
                        string location = "",
                        bool hasInputField = false,
                        Character mainCharacter = null,
                        Character[] left = null,
                        Character[] right = null,
                        Action onFirstEnter = null,
                        Action onEnter = null,
                        Action onFirstExit = null,
                        Action onExit = null,
                        Action onTick = null,
                        Process[] processes = null
                        ) {
        return new ReadPage(text, tooltip, location, hasInputField, MainCharacter, left, right, onFirstEnter, onEnter, onFirstExit, onExit, onTick, processes);
    }

    void GoToLastPage() {
        PageID = pageLinks[_pageIdentificationNumber];
    }

    public void AddTextBox(TextBox t, Action callBack = null) {
        PagePresenter.AddTextBox(t, callBack);
    }

    Process[] ProcessesWithBack(Process[] processes0, params Process[] processes1) {
        Util.Assert(processes0.Length + processes1.Length < ActionGridView.TOTAL_BUTTON_COUNT - 1, "Too many processes!");
        Process[] full = new Process[ActionGridView.TOTAL_BUTTON_COUNT];
        processes0.CopyTo(full, 0);
        processes1.CopyTo(full, processes0.Length);
        full[ActionGridView.TOTAL_BUTTON_COUNT - 1] = new Process("Back", "Go back to the previous page.", GoToLastPage);
        return full;
    }

    Process[] ProcessesWithBack(params Process[] processes) {
        Util.Assert(processes.Length < ActionGridView.TOTAL_BUTTON_COUNT - 1, "Too many processes!");
        Process[] full = new Process[ActionGridView.TOTAL_BUTTON_COUNT];
        processes.CopyTo(full, 0);
        full[ActionGridView.TOTAL_BUTTON_COUNT - 1] = new Process("Back", "Go back to the previous page.", GoToLastPage);
        return full;
    }

    public void Defeat() {
        Page.Tooltip = "You have died.";
        Page.ActionGrid = new Process[] { new Process("Main Menu", "Return to the main menu.", () => { Start(); PageID = "primary"; }) };
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

    void CreateGraph() {

    }
}
