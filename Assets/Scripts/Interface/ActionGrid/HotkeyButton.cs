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

    public KeyCode Hotkey { get { return IsHotkeyEnabled ? _hotkey : KeyCode.None; } set { _hotkey = value; } } //The keyboard key that interacts with this button
    public bool IsHotkeyEnabled;
    public Process Process {
        get {
            return process;
        }
        set {
            SetProcess(value);
        }
    }

    private KeyCode _hotkey;
    private Process process;
    private Color INACTIVE_COLOR = Color.black;
    private Color ACTIVE_COLOR = Color.white;

    private bool mouseIn;

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
        if (Input.GetKeyDown(this.Hotkey)) {
            Process.Play();
        }
        if (process != null && mouseIn) {
            Game.Instance.Tooltip.MouseText = process.Description;
        }
    }

    public void OnPointerEnter(BaseEventData e) {
        mouseIn = true;
    }

    public void OnPointerExit(BaseEventData e) {
        mouseIn = false;
        Game.Instance.Tooltip.MouseText = "";
    }

    public override void Reset() {
        Hotkey = KeyCode.None;
        Process = new Process();
    }

    public void ClearText() {
        text.text = "";
    }

    private void SetProcess(Process process) {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(process.Play);
        text.text = process.Name;
        hotkeyText.text = IsHotkeyEnabled && Hotkey != KeyCode.None ? Hotkey.ToString() : "";
        this.process = process;
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
