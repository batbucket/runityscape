using Scripts.Model.Pages;
using Scripts.View.ActionGrid;
using Scripts.View.Effects;
using Scripts.View.Other;
using Scripts.View.Portraits;
using Scripts.View.Sounds;
using Scripts.View.TextBoxes;
using Scripts.View.Title;
using UnityEngine;
using UnityEngine.UI;
using Scripts.Game.Pages;
using Scripts.Model.SaveLoad;
using Scripts.View.TextInput;

namespace Scripts.Presenter {

    /// <summary>
    /// Main, totally not god-like class that holds
    /// the various views and ties everything together.
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
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

        [SerializeField]
        private GraphicRaycaster raycaster;

        public static Main Instance { get { return instance; } }

        /// <summary>
        /// Gets the input.
        /// </summary>
        /// <value>
        /// The input.
        /// </value>
        public InputView Input {
            get {
                return input;
            }
        }

        /// <summary>
        /// Gets the sound.
        /// </summary>
        /// <value>
        /// The sound.
        /// </value>
        public SoundView Sound {
            get {
                return sound;
            }
        }

        /// <summary>
        /// Gets the text boxes.
        /// </summary>
        /// <value>
        /// The text boxes.
        /// </value>
        public TextBoxHolderView TextBoxes {
            get {
                return textBoxHolder;
            }
        }

        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public TitleView Title {
            get {
                return title;
            }
        }

        /// <summary>
        /// Gets the page presenter.
        /// </summary>
        /// <value>
        /// The page presenter.
        /// </value>
        public PagePresenter PagePresenter {
            get {
                return pagePresenter;
            }
        }

        public ActionGridView ActionGrid {
            get {
                return actionGrid;
            }
        }

        public bool IsInputEnabled {
            set {
                IsCursorEnabled = value;
                Main.Instance.ActionGrid.IsHotKeysEnabled = value;
            }
        }

        public bool IsCursorEnabled {
            set {
                Cursor.visible = value;
                raycaster.enabled = value;
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
            if (pagePresenter != null) {
                pagePresenter.Tick();
            }
        }
    }
}