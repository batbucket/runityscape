using Scripts.Model.Interfaces;
using Scripts.View.ObjectPool;
using UnityEngine;

namespace Scripts.View.ActionGrid {

    /// <summary>
    /// This class represents the grid of buttons viewable at the bottom of the screen.
    /// These buttons are accessible via mouse left clicks as well as keyboard input.
    /// </summary>
    public sealed class ActionGridView : MonoBehaviour {
        public const int COLS = 4;

        public const int ROWS = 3;

        public const int TOTAL_BUTTON_COUNT = ROWS * COLS;

        public static readonly KeyCode[] HOTKEYS = new KeyCode[] {
            KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R,
            KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.F,
            KeyCode.Z, KeyCode.X, KeyCode.C, KeyCode.V
            };

        [SerializeField]
        private PooledBehaviour actionButtonPrefab;

        /**
         * Hotkeys that are assigned to the buttons
         */
        private HotkeyButton[] actionButtons;

        /// <summary>
        /// Enables and disables the buttons
        /// </summary>
        public bool IsEnabled {
            set {
                foreach (HotkeyButton b in actionButtons) {
                    b.IsVisible = value;
                }
            }
        }

        /// <summary>
        /// Enables and disables the hotkeys for each button
        /// </summary>
        public bool IsHotkeysEnabled {
            set {
                foreach (HotkeyButton b in actionButtons) {
                    b.IsHotkeyEnabled = value;
                }
            }
        }

        public void ClearAll() {
            SetButtonAttributes();
        }

        public void SetButtonAttributes(params IButtonable[] buttonables) {
            Util.Assert(buttonables.Length <= actionButtons.Length);
            for (int i = 0; i < actionButtons.Length; i++) {
                if (i < buttonables.Length && buttonables[i] != null) {
                    if (buttonables[i].IsInvokable || buttonables[i].IsVisibleOnDisable) {
                        actionButtons[i].IsVisible = true;
                        actionButtons[i].Buttonable = buttonables[i];
                    } else {
                        actionButtons[i].IsVisible = false;
                    }
                } else {
                    actionButtons[i].IsVisible = false;
                }
            }
        }

        // Use this for initialization
        private void Start() {
            ObjectPoolManager.Instance.Register(actionButtonPrefab, TOTAL_BUTTON_COUNT);

            this.actionButtons = new HotkeyButton[HOTKEYS.Length];
            for (int i = 0; i < actionButtons.Length; i++) {
                PooledBehaviour actionButton = ObjectPoolManager.Instance.Get(actionButtonPrefab);
                Util.Parent(actionButton.gameObject, gameObject);
                actionButton.GetComponent<HotkeyButton>().Hotkey = HOTKEYS[i];
                actionButtons[i] = actionButton.GetComponent<HotkeyButton>();
            }
            ClearAll();
        }
    }
}