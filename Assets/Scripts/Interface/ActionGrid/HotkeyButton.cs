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
public class HotkeyButton : MonoBehaviour {

    public KeyCode Hotkey { get; set; } //The keyboard key that interacts with this button
    Button button; //The button we want a hotkey for
    UIInput input;
    public Text Text { get; set; } //The text on the button
    public Text HotkeyText { get; private set; }
    public IProcess Process { get; set; } //Process this button holds
    public TooltipManager Tooltip { get; set; }

    bool activated;
    bool tooltipActivated;

    Color INACTIVE_COLOR = Color.black;
    Color ACTIVE_COLOR = Color.white;

    public bool IsActive { get { return Process != null && Text.text != null && Text.text.Length > 0; } }
    public bool isEnabled { get { return button.interactable; } set { button.interactable = value; } }

    // Use this for initialization
    void Awake() {
        //hotkey = KeyCode.None;
        button = gameObject.GetComponent<Button>();
        input = gameObject.GetComponent<UIInput>();
        Text = gameObject.GetComponentsInChildren<Text>()[0];
        HotkeyText = gameObject.GetComponentsInChildren<Text>()[1];
        Tooltip = GameObject.Find("Tooltip").GetComponent<TooltipManager>();
        activated = false;

        Text.text = "";
        HotkeyText.text = "";

        //Mouse inputs can also activate action buttons under the same exact rules as if we were using a keyboard
        input.AddOnMouseDownListener(new Action<PointerEventData>(p => InputSimulator.SimulateKeyDown(Util.KeyCodeToVirtualKeyCode(Hotkey))));
        input.AddOnMouseUpListener(new Action<PointerEventData>(p => { InputSimulator.SimulateKeyUp(Util.KeyCodeToVirtualKeyCode(Hotkey)); activated = false; })); //Set activated to false to fix double button click issue
    }

    // Update is called once per frame
    void Update() {
        isEnabled = IsActive;
        HotkeyText.text = button.interactable ? Hotkey.ToString() : "";
        ButtonAppearance();
        if (button.interactable) {
            CheckInputs();
        }
    }

    public void ClearText() {
        Text.text = "";
    }

    void SetProcess(IProcess process) {
        if (process != null) {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => process.Play());
            Text.text = process.Name;
            this.Process = process;
        }
    }

    public void ClearProcess() {
        button.onClick.RemoveAllListeners();
        ClearText();
        this.Process = null;
    }

    void OnMouseExit() {
        activated = false;
        Tooltip.Clear();
    }

    //Display tooltip on mouse hovering
    void OnMouseOver() {
        tooltipActivated = true;
    }

    /**
     * HotkeyableButtons have these properties:
     *    1. If its hotkey is input, the Button changes to its pressed-down state
     *    2. Pressing any other button (includes mouse click) while its hotkey is
                being held down will cancel the input (don't go to #3 if true)
     *    3. If the hotkey is released, it counts as a button click (do the associated action)
     */
    void CheckInputs() {

        //1
        if (Input.GetKeyDown(Hotkey) && Input.inputString.Length == 1) {
            activated = true;
            tooltipActivated = true; //Also display tooltip on button hold
        }

        if (tooltipActivated) {
            Tooltip.Set(Process.Description);
            tooltipActivated = false;
        }

        if (activated) {
            if (Process != null) {
                Tooltip.Set(Process.Description);
            }

            //2
            if (Input.anyKeyDown && !Input.GetKeyDown(Hotkey)) {
                activated = false;
                Tooltip.Clear();
            }

            //3
            if (activated && (Input.GetKeyUp(Hotkey))) {
                button.onClick.Invoke();
                activated = false;
                Tooltip.Clear();
            }
        }
    }

    void ButtonAppearance() {
        if (Text.text.Length == 0) {
            DisabledAppearance();
        } else if (activated && button.interactable) {
            ActiveAppearance();
        } else {
            InactiveAppearance();
        }
    }

    void ActiveAppearance() {
        button.image.color = ACTIVE_COLOR;
        Text.color = INACTIVE_COLOR;
        HotkeyText.color = INACTIVE_COLOR;
    }

    void InactiveAppearance() {
        button.image.color = INACTIVE_COLOR;
        Text.color = ACTIVE_COLOR;
        HotkeyText.color = ACTIVE_COLOR;
    }

    void DisabledAppearance() {
        button.image.color = Color.clear;
        HotkeyText.color = Color.clear;
    }
}
