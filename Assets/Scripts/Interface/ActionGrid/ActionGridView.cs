using UnityEngine;
using System.Linq;
using System.Collections.Generic;

/**
 * This class represents the grid of buttons
 * on the bottom of the game screen
 * Accessible by keyboard keys and the mouse
 */
public sealed class ActionGridView : MonoBehaviour {

    /**
     * Hotkeys that are assigned to the row of buttons
     * on the bottom of the screen
     */
    public static readonly KeyCode[] HOTKEYS = new KeyCode[]
        {KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R,
         KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.F,
         KeyCode.Z, KeyCode.X, KeyCode.C, KeyCode.V
        };
    public const int ROWS = 3;
    public const int COLS = 4;
    public const int TOTAL_BUTTON_COUNT = ROWS * COLS;

    private HotkeyButton[] actionButtons;

    [SerializeField]
    private PooledBehaviour actionButtonPrefab;

    public bool IsHotkeysEnabled {
        set {
            foreach (HotkeyButton b in actionButtons) {
                b.IsHotkeyEnabled = value;
            }
        }
    }

    public bool IsEnabled {
        set {
            foreach (HotkeyButton b in actionButtons) {
                b.IsVisible = value;
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

    public void ClearAll() {
        SetButtonAttributes();
    }
}
