using Scripts.Model.Interfaces;
using Scripts.View.ObjectPool;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.View.ActionGrid {

    /// <summary>
    /// This class represents the grid of buttons viewable at the bottom of the screen.
    /// These buttons are accessible via mouse left clicks as well as keyboard input.
    /// </summary>
    public sealed class ActionGridView : MonoBehaviour {

        public static readonly KeyCode[] HOTKEYS = new KeyCode[] {
            KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4,
            KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R,
            KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.F,
            KeyCode.Z, KeyCode.X, KeyCode.C, KeyCode.V
            };

        [SerializeField]
        private HotkeyButton actionButtonPrefab;

        /**
         * Hotkeys that are assigned to the buttons
         */
        private List<HotkeyButton> buttons;

        /// <summary>
        /// Enables and disables the buttons
        /// </summary>
        public bool IsEnabled {
            set {
                foreach (HotkeyButton b in buttons) {
                    b.IsVisible = value;
                }
            }
        }

        public bool IsHotKeysEnabled {
            set {
                foreach (HotkeyButton b in buttons) {
                    b.IsHotkeyEnabled = value;
                }
            }
        }

        public void ClearAll() {
            for (int i = 0; i < buttons.Count; i++) {
                ObjectPoolManager.Instance.Return(buttons[i]);
            }
            buttons.Clear();
        }

        public void SetButtonAttributes(IList<IButtonable> buttonables) {
            for (int i = 0; i < buttonables.Count; i++) {
                HotkeyButton hb;

                if (i < buttons.Count) {
                    hb = buttons[i];
                } else {
                    hb = ObjectPoolManager.Instance.Get(actionButtonPrefab);
                    Util.Parent(hb.gameObject, gameObject);
                    buttons.Add(hb);
                }

                if (buttonables[i] != null) {
                    hb.IsVisible = true;
                    hb.Buttonable = buttonables[i];
                } else {
                    hb.IsVisible = false;
                }

                if (i < HOTKEYS.Length) {
                    hb.Hotkey = HOTKEYS[i];
                } else {
                    hb.Hotkey = KeyCode.None;
                }
            }
        }

        // Use this for initialization
        private void Start() {
            ObjectPoolManager.Instance.Register(actionButtonPrefab, 20);
            buttons = new List<HotkeyButton>();
        }
    }
}