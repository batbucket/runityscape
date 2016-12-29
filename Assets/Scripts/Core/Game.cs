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

    public TitleView Title {
        get {
            return title;
        }
    }

    public GoldView Gold {
        get {
            return gold;
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
    [SerializeField]
    private TitleView title;
    private PagePresenter pagePresenter;
    [SerializeField]
    private GoldView gold;

    public const float NORMAL_TEXT_SPEED = 0.001f;

    StartMenu start;

    void Start() {
        instance = this;

        time.IsEnabled = false;

        start = new StartMenu();

        header.Location = "Main Menu";

        pagePresenter = new PagePresenter(start.MainMenu, textBoxHolder, actionGrid, leftPortraits, rightPortraits, header, tooltip, sound);
    }

    public void Cutscene(bool isGridVisible, params Event[] events) {
        StartCoroutine(Timeline(events, isGridVisible));
    }

    private IEnumerator Timeline(Event[] events, bool isGridVisible) {
        IButtonable[] savedGrid = new IButtonable[0];
        if (!isGridVisible) {
            savedGrid = CurrentPage.ActionGrid;
            CurrentPage.ActionGrid = null;
            pagePresenter.Page.GetAll().ForEach(c => c.IsCharging = false);
        }
        for (int i = 0; i < events.Length; i++) {
            Event e = events[i];
            float timer = 0;

            while ((timer += UnityEngine.Time.deltaTime) < e.Delay) {
                yield return null;
            }
            e.Action.Invoke();
            while (!e.HasEnded.Invoke()) {
                yield return null;
            }
            if (!isGridVisible && i < events.Length - 1) {
                bool isAdvanced = false;
                CurrentPage.ActionGrid = new IButtonable[] { new Process("→", "", () => isAdvanced = true) };
                while (!isAdvanced) {
                    yield return null;
                }
                CurrentPage.ActionGrid = new IButtonable[0];
            }
        }
        if (!isGridVisible) {
            CurrentPage.ActionGrid = savedGrid;
            pagePresenter.Page.GetAll().ForEach(c => c.IsCharging = true);
        }
        yield break;
    }

    void Update() {
        if (pagePresenter != null) {
            pagePresenter.Tick();
        }
    }
}
