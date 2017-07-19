using Scripts.Model.Interfaces;
using Scripts.Presenter;
using Scripts.View.ObjectPool;
using Scripts.View.Tooltip;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.View.ActionGrid {

    /// <summary>
    /// This class represents a button that can be pressed with
    /// the mouse and the keyboard
    /// </summary>
    public class HotkeyButton : PooledBehaviour {
        private static Color ACTIVE_COLOR = Color.white;

        [SerializeField]
        private Tip tip;

        [SerializeField]
        private Button button;

        [SerializeField]
        private Image icon;

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

        [SerializeField]
        private Text text;

        private bool isHotkeyEnabled;

        public IButtonable Buttonable {
            set {
                text.text = value.ButtonText;
                icon.gameObject.SetActive(value.Sprite != null);
                icon.sprite = value.Sprite;
                tip.Sprite = value.Sprite;
                tip.Title = value.ButtonText;
                tip.Body = value.TooltipText;
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => {
                    if (value.IsInvokable) {
                        value.Invoke();
                    }
                });
            }
        }

        public KeyCode Hotkey {
            get {
                return hotkey;
            }
            set {
                hotkeyText.enabled = (value != KeyCode.None);
                string text = "";
                string keyCode = value.ToString();

                // Change "Alpha1" to 1 and etc.
                if (keyCode.Contains("Alpha")) {
                    text = "" + keyCode[keyCode.Length - 1];
                } else {
                    text = keyCode;
                }
                hotkeyText.text = text;
                hotkey = value;
            }
        } //The keyboard key that interacts with this button

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

        public bool IsHotkeyEnabled {
            set {
                this.isHotkeyEnabled = value;
            }
        }

        public override void Reset() {
            Hotkey = KeyCode.None;
            icon.sprite = null;
        }

        private void ActiveAppearance() {
            button.image.color = ACTIVE_COLOR;
            icon.color = ACTIVE_COLOR;
            text.color = INACTIVE_COLOR;
            hotkeyText.color = INACTIVE_COLOR;
        }

        private void DisabledAppearance() {
            text.text = string.Empty;
            tip.enabled = false;
            icon.color = Color.clear;
            button.image.color = Color.clear;
            hotkeyText.color = Color.clear;
        }

        private void InactiveAppearance() {
            button.image.color = INACTIVE_COLOR;
            tip.enabled = true;
            icon.color = ACTIVE_COLOR;
            text.color = ACTIVE_COLOR;
            hotkeyText.color = ACTIVE_COLOR;
        }

        // Update is called once per frame
        private void Update() {
            hotkeyText.gameObject.SetActive(isHotkeyEnabled);
            if (Input.GetKeyDown(this.Hotkey) && isHotkeyEnabled) {
                button.onClick.Invoke();
            }
        }
    }
}