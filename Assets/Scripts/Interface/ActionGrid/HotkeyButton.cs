using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using UnityEngine.EventSystems;
using WindowsInput;

/**
 * This class represents a button that can be pressed with
 * the mouse and/or the keyboard
 */
public class HotkeyButton : PooledBehaviour {
    [SerializeField]
    private Button button;
    [SerializeField]
    private UIInput input;
    [SerializeField]
    private Text text;
    [SerializeField]
    private Text hotkeyText;
    [SerializeField]
    private BoxCollider2D bc;

    public KeyCode Hotkey { get { return isHotkeyEnabled ? hotkey : KeyCode.None; } set { hotkey = value; } } //The keyboard key that interacts with this button
    public bool IsHotkeyEnabled {
        set {
            this.isHotkeyEnabled = value;
            hotkeyText.text = (value && Hotkey != KeyCode.None ? Hotkey.ToString() : "");
        }
    }

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

    private bool isHotkeyEnabled;
    private string tooltipText;
    private KeyCode hotkey;
    private Color INACTIVE_COLOR = Color.black;
    private Color ACTIVE_COLOR = Color.white;

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

    // Use this for initialization
    private void Awake() {
        IsHotkeyEnabled = true;
    }

    // Update is called once per frame
    private void Update() {
        if (isHotkeyEnabled && Input.GetKeyDown(this.Hotkey)) {
            button.onClick.Invoke();
        }
    }

    private void OnMouseOver() {
        if (this.enabled) {
            Game.Instance.Tooltip.MouseText = tooltipText;
        } else {
            Game.Instance.Tooltip.MouseText = "";
        }
    }

    private void OnMouseExit() {
        Game.Instance.Tooltip.MouseText = "";
    }

    public override void Reset() {
        Hotkey = KeyCode.None;
    }

    private void ActiveAppearance() {
        button.image.color = ACTIVE_COLOR;
        text.color = INACTIVE_COLOR;
        hotkeyText.color = INACTIVE_COLOR;
    }

    private void InactiveAppearance() {
        button.image.color = INACTIVE_COLOR;
        text.color = ACTIVE_COLOR;
        hotkeyText.color = ACTIVE_COLOR;
    }

    private void DisabledAppearance() {
        text.text = "";
        button.image.color = Color.clear;
        hotkeyText.color = Color.clear;
    }
}
