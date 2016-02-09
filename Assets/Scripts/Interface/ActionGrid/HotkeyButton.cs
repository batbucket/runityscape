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
    public IProcess Process { get; private set; } //Process this button holds
    public TooltipManager Tooltip { get; set; }

    bool activated;
    bool mouseInBounds;

    Color INACTIVE_COLOR = Color.black;
    Color ACTIVE_COLOR = Color.white;

    // Use this for initialization
    void Awake() {
        //hotkey = KeyCode.None;
        button = gameObject.GetComponent<Button>();
        input = gameObject.GetComponent<UIInput>();
        Text = gameObject.GetComponentsInChildren<Text>()[0];
        HotkeyText = gameObject.GetComponentsInChildren<Text>()[1];
        Tooltip = TooltipManager.Instance;
        activated = false;

        Text.text = "";
        HotkeyText.text = "";

        //Mouse inputs can also activate action buttons under the same exact rules as if we were using a keyboard
        input.AddOnMouseDownListener(new Action<PointerEventData>(p => InputSimulator.SimulateKeyDown(Util.KeyCodeToVirtualKeyCode(Hotkey))));
        input.AddOnMouseUpListener(new Action<PointerEventData>(p => { InputSimulator.SimulateKeyUp(Util.KeyCodeToVirtualKeyCode(Hotkey)); activated = false; })); //Set activated to false to fix double button click issue
        ClearProcess();
    }

    // Update is called once per frame
    void Update() {
        ButtonAppearance();
        CheckInputs();
    }

    public void ClearText() {
        Text.text = "";
    }

    public void SetProcess(IProcess process) {
        if (process == null) {
            ClearProcess();
        } else {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(process.Play);
            Text.text = process.Name;
            this.Process = process;
            HotkeyText.text = Hotkey.ToString();
            button.interactable = true;
        }
    }

    public void ClearProcess() {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => { });
        Text.text = "";
        this.Process = new Process();
        button.interactable = false;
        HotkeyText.text = "";
    }

    void OnMouseExit() {
        activated = false;
        mouseInBounds = false;
    }

    //Display tooltip on mouse hovering
    void OnMouseOver() {
        mouseInBounds = true;
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
        }

        if (mouseInBounds && button.IsInteractable()) {
            Tooltip.Set(Process.Description);
        }

        if (activated) {
            if (button.IsInteractable()) {
                Tooltip.Set(Process.Description);
            }

            //2
            if (Input.anyKeyDown && !Input.GetKeyDown(Hotkey)) {
                activated = false;
            }

            //3
            if (activated && (Input.GetKeyUp(Hotkey))) {
                button.onClick.Invoke();
                activated = false;
            }
        }
    }

    void ButtonAppearance() {
        if (Text.text.Length == 0) {
            DisabledAppearance();
        } else if (activated || mouseInBounds && button.interactable) {
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
