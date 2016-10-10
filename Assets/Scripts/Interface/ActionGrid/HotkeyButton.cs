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

    private KeyCode _hotkey;
    public KeyCode Hotkey { get { return IsHotkeyEnabled ? _hotkey : KeyCode.None; } set { _hotkey = value; } } //The keyboard key that interacts with this button

    private Process _process;
    public Process Process { get { return _process; } set { SetProcess(value); } } //Process this button holds

    private bool activated;
    private bool mouseInBounds;

    private Color INACTIVE_COLOR = Color.black;
    private Color ACTIVE_COLOR = Color.white;

    public bool IsHotkeyEnabled { get; set; }

    [SerializeField]
    private Button button;
    [SerializeField]
    private UIInput input;
    [SerializeField]
    private Text text;
    [SerializeField]
    private Text hotkeyText;

    private bool _isVisible;
    public bool IsVisible {
        set {
            _isVisible = value;
        }
        get {
            return _isVisible;
        }
    }

    // Use this for initialization
    private void Awake() {
        activated = false;

        //Mouse inputs can also activate action buttons under the same exact rules as if we were using a keyboard
        input.AddOnMouseDownListener(new Action<PointerEventData>(p => InputSimulator.SimulateKeyDown(Util.KeyCodeToVirtualKeyCode(Hotkey))));
        input.AddOnMouseUpListener(new Action<PointerEventData>(p => { InputSimulator.SimulateKeyUp(Util.KeyCodeToVirtualKeyCode(Hotkey)); activated = false; })); //Set activated to false to fix double button click issue
        IsHotkeyEnabled = true;
        _isVisible = true;
    }

    // Update is called once per frame
    private void Update() {
        ButtonAppearance();
        if (_isVisible) {
            CheckInputs();
        }
    }

    public override void Reset() {
        Hotkey = KeyCode.None;
        Process = new Process();
    }

    public void ClearText() {
        text.text = "";
    }

    private void SetProcess(Process process) {
        if (process == null || !process.Condition.Invoke()) {
            ClearProcess();
        } else {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(process.Play);
            text.text = process.Name;
            this._process = process;
            hotkeyText.text = IsHotkeyEnabled && Hotkey != KeyCode.None ? Hotkey.ToString() : "";
            button.interactable = true;
        }
    }

    public void ClearProcess() {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => { });
        text.text = "";
        this._process = new Process();
        button.interactable = false;
        hotkeyText.text = "";
    }

    private void OnPointerUp() {
        activated = false;
        mouseInBounds = false;
    }

    //Display tooltip on mouse hovering
    private void OnPointerDown() {
        mouseInBounds = true;
    }

    /**
     * HotkeyableButtons have these properties:
     *    1. If its hotkey is input, the Button changes to its pressed-down state
     *    2. Pressing any other button (includes mouse click) while its hotkey is
                being held down will cancel the input (don't go to #3 if true)
     *    3. If the hotkey is released, it counts as a button click (do the associated action)
     */
    private void CheckInputs() {

        //1
        if (Input.GetKeyDown(Hotkey) && Input.inputString.Length == 1) {
            activated = true;
        }

        if (mouseInBounds && button.IsInteractable() && !string.IsNullOrEmpty(Process.Description)) {
            Game.Instance.Tooltip.Text = Process.Description;
        }

        if (activated) {
            if (button.IsInteractable() && !string.IsNullOrEmpty(Process.Description)) {
                Game.Instance.Tooltip.Text = Process.Description;
            }

            //2
            if (Input.anyKeyDown && !Input.GetKeyDown(Hotkey)) {
                activated = false;
            }

            //3
            if (activated && (Input.GetKeyUp(Hotkey))) {
                Process.Play();
                activated = false;
            }
        }
    }

    private void ButtonAppearance() {
        if (text.text.Length == 0 || !IsVisible) {
            DisabledAppearance();
        } else if ((activated || mouseInBounds) && button.interactable) {
            ActiveAppearance();
        } else {
            InactiveAppearance();
        }
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
