using Script.View.Tooltip;
using Scripts.Model.Acts;
using Scripts.Model.World.Pages;
using Scripts.Model.World.Serialization;
using Scripts.Model.Interfaces;
using Scripts.Model.Pages;
using Scripts.Model.Processes;
using Scripts.View.ActionGrid;
using Scripts.View.Effects;
using Scripts.View.Other;
using Scripts.View.Portraits;
using Scripts.View.Sounds;
using Scripts.View.TextBoxes;
using Scripts.View.Title;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Presenter {

    public class Game : MonoBehaviour {
        public const float NORMAL_TEXT_SPEED = 0.001f;
        public OtherPresenter Other;
        public StartMenu StartMenu;
        private static Game instance;

        [SerializeField]
        private ActionGridView actionGrid;

        [SerializeField]
        private EffectsManager effect;

        [SerializeField]
        private GoldView gold;

        [SerializeField]
        private HeaderView header;

        [SerializeField]
        private PortraitHolderView leftPortraits;

        private PagePresenter pagePresenter;

        [SerializeField]
        private PortraitHolderView rightPortraits;

        [SerializeField]
        private Scrollbar scrollBar;

        [SerializeField]
        private SoundView sound;

        [SerializeField]
        private TextBoxHolderView textBoxHolder;

        [SerializeField]
        private TimeView time;

        [SerializeField]
        private TitleView title;

        [SerializeField]
        private TooltipView tooltip;

        public static Game Instance { get { return instance; } }

        public Page CurrentPage {
            set {
                pagePresenter.Page = value;
            }
            get {
                return pagePresenter.Page;
            }
        }

        public GoldView Gold {
            get {
                return gold;
            }
        }

        public SoundView Sound {
            get {
                return sound;
            }
        }

        public TextBoxHolderView TextBoxes {
            get {
                return textBoxHolder;
            }
        }

        public TimeView Time {
            get {
                return time;
            }
        }

        public TitleView Title {
            get {
                return title;
            }
        }

        public TooltipView Tooltip {
            get {
                return tooltip;
            }
        }

        public void Cutscene(bool isGridVisible, params Act[] acts) {
            StartCoroutine(Timeline(acts, isGridVisible));
        }

        public void Cutscene(bool isGridVisible, Act[] act0, params Act[] acts1) {
            Act[] all = new Act[act0.Length + acts1.Length];
            for (int i = 0; i < act0.Length; i++) {
                all[i] = act0[i];
            }
            for (int i = act0.Length; i < act0.Length + acts1.Length; i++) {
                all[i] = acts1[i - act0.Length];
            }
            StartCoroutine(Timeline(all, isGridVisible));
        }

        private void Start() {
            if (!Directory.Exists(SaveLoad.SAVE_DIRECTORY)) {
                Directory.CreateDirectory(SaveLoad.SAVE_DIRECTORY);
            }
            instance = this;

            header.Location = "Main Menu";
            Other = new OtherPresenter(gold, time);
            StartMenu = new StartMenu();
            pagePresenter = new PagePresenter(StartMenu, textBoxHolder, actionGrid, leftPortraits, rightPortraits, header, tooltip, sound);
        }

        private IEnumerator Timeline(Act[] acts, bool isGridVisible) {
            IButtonable[] savedGrid = new IButtonable[0];
            bool skip = false;
            if (!isGridVisible) {
                savedGrid = CurrentPage.ActionGrid;
                CurrentPage.ActionGrid = null;
                pagePresenter.Page.GetAll().ForEach(c => c.IsCharging = false);
            }
            for (int i = 0; i < acts.Length; i++) {
                Act a = acts[i];
                float timer = 0;

                while (!skip && (timer += UnityEngine.Time.deltaTime) < a.Delay) {
                    yield return null;
                }
                if (skip && a.SkipEvent != null) {
                    a.SkipEvent.Invoke();
                }
                a.Action.Invoke();
                while (!skip && !a.HasEnded.Invoke()) {
                    yield return null;
                }

                if (!skip && a.RequiresUserAdvance && !isGridVisible && i < acts.Length - 1) {
                    bool isAdvanced = false;
                    CurrentPage.ActionGrid = new IButtonable[] {
                    new Process("→", "", () => isAdvanced = true),
                    null,
                    null,
                    new Process("Skip All", "Skip all events in this series.", () =>
                    {
                        isAdvanced = true;
                        skip = true;
                    }),
                };
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
        }

        private void Update() {
            if (pagePresenter != null) {
                pagePresenter.Tick();
            }
            if (Other != null) {
                Other.Tick();
            }
        }
    }
}