using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class Game : MonoBehaviour {
    private static Game instance;
    public static Game Instance { get { return instance; } }

    public Page CurrentPage {
        set {
            pagePresenter.Page = value;
        }
        get {
            return pagePresenter.Page;
        }
    }

    public TimeView Time {
        get {
            return time;
        }
    }

    public TextBoxHolderView TextBoxes {
        get {
            return textBoxHolder;
        }
    }

    public SoundView Sound {
        get {
            return sound;
        }
    }

    public TooltipView Tooltip {
        get {
            return tooltip;
        }
    }

    [SerializeField]
    private TimeView time;
    [SerializeField]
    private HeaderView header;
    [SerializeField]
    private TextBoxHolderView textBoxHolder;
    [SerializeField]
    private PortraitHolderView leftPortraits;
    [SerializeField]
    private PortraitHolderView rightPortraits;
    [SerializeField]
    private ActionGridView actionGrid;
    [SerializeField]
    private TooltipView tooltip;
    [SerializeField]
    private SoundView sound;
    [SerializeField]
    private EffectsManager effect;
    [SerializeField]
    private Scrollbar scrollBar;
    private PagePresenter pagePresenter;

    public const float NORMAL_TEXT_SPEED = 0.001f;

    StartMenu start;

    void Start() {
        instance = this;

        time.IsEnabled = false;

        start = new StartMenu();

        header.Location = "Main Menu";

        pagePresenter = new PagePresenter(start.MainMenu, textBoxHolder, actionGrid, leftPortraits, rightPortraits, header, tooltip, sound);
    }

    public void Cutscene(params Event[] events) {
        StartCoroutine(Timeline(events, true));
    }

    public void Ordered(params Event[] events) {
        StartCoroutine(Timeline(events, false));
    }

    IEnumerator Timeline(Event[] events, bool hideGrid) {
        if (hideGrid) {
            actionGrid.IsEnabled = false;
            pagePresenter.Page.GetAll().ForEach(c => c.IsCharging = false);
        }
        foreach (Event myEvent in events) {
            Event e = myEvent;
            if (!e.IsOneShot || (e.IsOneShot && !e.HasPlayed)) {
                float timer = 0;
                while (!Input.GetKey(KeyCode.LeftControl) && (timer += UnityEngine.Time.deltaTime) < e.Delay) {
                    yield return null;
                }
                e.Action.Invoke();
                while (hideGrid && !Input.GetKey(KeyCode.LeftControl) && !Input.GetKeyDown(KeyCode.Space)) {
                    yield return null;
                }
                e.HasPlayed = true;
            }
        }
        if (hideGrid) {
            actionGrid.IsEnabled = true;
            pagePresenter.Page.GetAll().ForEach(c => c.IsCharging = true);
        }
        yield break;
    }

    public void Cutscene(params Event[][] events) {
        StartCoroutine(Timeline(events, true));
    }

    IEnumerator Timeline(Event[][] events, bool hideGrid) {
        if (hideGrid) {
            actionGrid.IsEnabled = false;
        }
        if (events[0][0].TextBox != null && events[0].IsExactly<TextBox>()) {
            events[0][0].Delay = 0;
        }
        foreach (Event[] myEventGroup in events) {
            Event[] eventGroup = myEventGroup;
            foreach (Event myEvent in eventGroup) {
                Event e = myEvent;
                if (!e.IsOneShot || (e.IsOneShot && !e.HasPlayed)) {
                    float timer = 0;
                    while (!Input.GetKey(KeyCode.LeftControl) && (timer += UnityEngine.Time.deltaTime) < e.Delay) {
                        yield return null;
                    }
                    e.Action.Invoke();
                    while (!Input.GetKey(KeyCode.LeftControl) && !e.HasEnded.Invoke()) {
                        yield return null;
                    }
                    e.HasPlayed = true;
                }
            }
        }
        if (hideGrid) {
            actionGrid.IsEnabled = true;
        }
        yield break;
    }

    public void Defeat() {
        pagePresenter.Page.Tooltip = "You have died.";
        pagePresenter.Page.ActionGrid = new Process[] { new Process("Main Menu", "Return to the main menu.",
            () => {
                Start();
                pagePresenter.Page = start.MainMenu;
                StopAllCoroutines();
                actionGrid.IsEnabled = true;
            }) };
    }

    // Update is called once per frame
    void Update() {
        if (pagePresenter != null) {
            pagePresenter.Tick();
        }
    }
}
