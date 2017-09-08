using Script.View.Tooltip;
using Scripts.Model.Acts;
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
using System.Collections.Generic;
using Scripts.Model.World;
using Scripts.Game.Pages;
using Scripts.Model.SaveLoad;
using Scripts.View.TextInput;

namespace Scripts.Presenter {

    public class Main : MonoBehaviour {
        public static int VERSION = 2;

        private static Main instance;

        [SerializeField]
        private ActionGridView actionGrid;

        [SerializeField]
        private EffectsManager effect;

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
        private TitleView title;

        [SerializeField]
        private InputView input;

        public static Main Instance { get { return instance; } }

        public InputView Input {
            get {
                return input;
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

        public TitleView Title {
            get {
                return title;
            }
        }

        public PagePresenter PagePresenter {
            get {
                return pagePresenter;
            }
        }

        private void Start() {
            instance = this;

            Page mainMenu = new Page("placeholder");
            pagePresenter = new PagePresenter(mainMenu, textBoxHolder, actionGrid, leftPortraits, rightPortraits, header, sound);

            Menus menus = new Menus();
            pagePresenter.Page = menus.Root;
            IdTable.Init();
        }

        private void Update() {
            if (pagePresenter != null && pagePresenter.Page != null) {
                pagePresenter.Tick();
            }
        }
    }
}