using Scripts.Model.Interfaces;
using Scripts.Presenter;
using Scripts.View.ObjectPool;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.View.ActionGrid {

    /// <summary>
    /// This class represents a button that can be pressed with
    /// the mouse and the keyboard
    /// </summary>
    public class HotkeyButton : PooledBehaviour {
        private Color ACTIVE_COLOR = Color.white;

        /// <summary>
        /// Collider we use to detect when the mouse is over the button
        /// For tooltip changing
        /// </summary>
        [SerializeField]
        private BoxCollider2D bc;

        [SerializeField]
        private Button button;

        private KeyCode hotkey;

        /// <summary>
        /// Text on the button reflecting the hotkey
        /// </summary>
        [SerializeField]
        private Text hotkeyText;

        private Color INACTIVE_COLOR = Color.black;

        /// <summary>
        /// Used to detect keyboard strokes for the purposes of
        /// invoking the button
        /// </summary>
        [SerializeField]
        private UIInput input;

        private bool isHotkeyEnabled;

        [SerializeField]
        private Text text;

        private string tooltipText;

        public IButtonable Buttonable {
            set {
                text.text = value.ButtonText;
                tooltipText = value.TooltipText;
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => {
                    if (value.IsInvokable) {
                        value.Invoke();
                        Game.Instance.Tooltip.MouseText = "";
                    }
                });
            }
        }

        public KeyCode Hotkey { get { return isHotkeyEnabled ? hotkey : KeyCode.None; } set { hotkey = value; } } //The keyboard key that interacts with this button

        public bool IsHotkeyEnabled {
            set {
                this.isHotkeyEnabled = value;
                hotkeyText.text = (value && Hotkey != KeyCode.None ? Hotkey.ToString() : "");
            }
        }

        public bool IsVisible {
            set {
                if (value) {
                    InactiveAppearance();
                } else {
                    DisabledAppearance();
                }
                this.enabled = value;
                button.enabled = value;
            }
        }

        public override void Reset() {
            Hotkey = KeyCode.None;
        }

        protected override string HoverOverText {
            get {
                return tooltipText;
            }
        }

        private void ActiveAppearance() {
            button.image.color = ACTIVE_COLOR;
            text.color = INACTIVE_COLOR;
            hotkeyText.color = INACTIVE_COLOR;
        }

        // Use this for initialization
        private void Awake() {
            IsHotkeyEnabled = true;
        }

        private void DisabledAppearance() {
            text.text = "";
            button.image.color = Color.clear;
            hotkeyText.color = Color.clear;
        }

        private void InactiveAppearance() {
            button.image.color = INACTIVE_COLOR;
            text.color = ACTIVE_COLOR;
            hotkeyText.color = ACTIVE_COLOR;
        }

        // Update is called once per frame
        private void Update() {
            if (isHotkeyEnabled && Input.GetKeyDown(this.Hotkey)) {
                button.onClick.Invoke();
            }
        }
    }
}