using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine.UI;

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

    [SerializeField]
    Scrollbar _scrollbar;
    public Scrollbar Scrollbar { get { return _scrollbar; } }

    public PagePresenter PagePresenter { get; private set; }
    public Character MainCharacter { get; private set; }

    public const float NORMAL_TEXT_SPEED = 0.001f;

    StartMenu start;

    void Start() {
        _instance = this;
        PagePresenter = new PagePresenter();
        Time.IsEnabled = false;

        start = new StartMenu();

        Header.Blurb = "";
        Header.Chapter = "";
        Header.Location = "";

        MenuButton.Hotkey = KeyCode.None;
        MenuButton.Process = new Process("Main Menu", "Return to the Main Menu.", () => { Start(); PagePresenter.SetPage(start.Primary); });

        PagePresenter.SetPage(start.Primary);
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
        foreach (Event myEvent in events) {
            Event e = myEvent;
            if (!e.isOneShot || (e.isOneShot && !e.HasPlayed)) {
                float timer = 0;
                while (!Input.GetKey(KeyCode.LeftControl) && (timer += UnityEngine.Time.deltaTime) < e.delay) {
                    yield return null;
                }
                e.action.Invoke();
                while (hideGrid && !Input.GetKey(KeyCode.LeftControl) && !Input.GetKeyDown(KeyCode.Space)) {
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

    public GameObject AddTextBox(TextBox t, Action callBack = null) {
        return PagePresenter.AddTextBox(t, callBack);
    }

    public void Defeat() {
        PagePresenter.Page.Tooltip = "You have died.";
        PagePresenter.Page.ActionGrid = new Process[] { new Process("Main Menu", "Return to the main menu.", () => { Start(); PagePresenter.SetPage(start.Primary); }) };
    }

    // Update is called once per frame
    void Update() {
        if (PagePresenter != null) {
            PagePresenter.Tick();
        }
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
