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

        /// <summary>
        /// Hotkeys in use. If there's more buttons it should default to no hotkey.
        /// </summary>
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
        /// Buttons may end up getting added.
        /// </summary>
        private bool isHotkeysEnabled;

        /// <summary>
        /// Enables and disables the hotkeys.
        /// </summary>
        public bool IsHotKeysEnabled {
            set {
                foreach (HotkeyButton b in buttons) {
                    b.IsHotkeyEnabled = value;
                }
                this.isHotkeysEnabled = value;
            }
        }

        /// <summary>
        /// Removes all buttons from grid.
        /// </summary>
        public void ClearAll() {
            for (int i = 0; i < buttons.Count; i++) {
                ObjectPoolManager.Instance.Return(buttons[i]);
            }
            buttons.Clear();
        }

        /// <summary>
        /// Sets the entire grid to match buttonables.
        /// </summary>
        /// <param name="buttonables">List of buttonable objects that will become the new buttons.</param>
        public void SetButtonAttributes(IList<IButtonable> buttonables) {
            for (int i = 0; i < buttonables.Count; i++) {
                HotkeyButton hb;

                // Reuse a button that's already there
                if (i < buttons.Count) {
                    hb = buttons[i];
                } else { // Grab one from the object pool
                    hb = ObjectPoolManager.Instance.Get(actionButtonPrefab);
                    hb.IsHotkeyEnabled = isHotkeysEnabled;
                    Util.Parent(hb.gameObject, gameObject);
                    buttons.Add(hb);
                }

                // Set if not null
                if (buttonables[i] != null) {
                    hb.Buttonable = buttonables[i];
                    hb.IsVisible = buttonables[i].IsInvokable || buttonables[i].IsVisibleOnDisable;
                } else { // Disable otherwise
                    hb.IsVisible = false;
                }

                // Hotkey if within hotkey range.
                if (i < HOTKEYS.Length) {
                    hb.Hotkey = HOTKEYS[i];
                } else { // No hotkey if too many buttons
                    hb.Hotkey = KeyCode.None;
                }
            }
        }

        // Use this for initialization
        private void Start() {
            ObjectPoolManager.Instance.Register(actionButtonPrefab);
            buttons = new List<HotkeyButton>();
        }
    }
}