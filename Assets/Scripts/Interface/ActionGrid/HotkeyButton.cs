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

    public KeyCode hotkey; //The keyboard key that interacts with this button
    Button button; //The button we want a hotkey for
    UIInput input;
    Text text; //The text on the button
    Text hotkeyText;
    IProcess process; //Process this button holds
    TooltipManager tooltip;

    bool activated;
    bool tooltipActivated;

    Color INACTIVE_COLOR = Color.black;
    Color ACTIVE_COLOR = Color.white;

    // Use this for initialization
    void Awake() {
        //hotkey = KeyCode.None;
        button = gameObject.GetComponent<Button>();
        input = gameObject.GetComponent<UIInput>();
        text = gameObject.GetComponentsInChildren<Text>()[0];
        hotkeyText = gameObject.GetComponentsInChildren<Text>()[1];
        tooltip = GameObject.Find("Tooltip").GetComponent<TooltipManager>();
        activated = false;

        text.text = "";
        hotkeyText.text = "";

        //Mouse inputs can also activate action buttons under the same exact rules as if we were using a keyboard
        input.AddOnMouseDownListener(new Action<PointerEventData>(p => InputSimulator.SimulateKeyDown(Util.keyCodeToVirtualKeyCode(hotkey))));
        input.AddOnMouseUpListener(new Action<PointerEventData>(p => { InputSimulator.SimulateKeyUp(Util.keyCodeToVirtualKeyCode(hotkey)); activated = false; } )); //Set activated to false to fix double button click issue
    }

    // Update is called once per frame
    void Update() {
        setEnabled(isActive());
        setHotkeyText(button.interactable ? hotkey.ToString() : "");
        determineButtonAppearance();
        if (button.interactable) {
            checkInputs();
        }
    }

    public void setHotkey(KeyCode keycode) {
        hotkey = keycode;
    }

    public void setText(string desc) {
        text.text = desc;
    }

    public void clearText() {
        text.text = "";
    }

    public void setEnabled(bool enabled) {
        button.interactable = enabled;
    }

    public void setProcess(IProcess process) {
        if (process != null) {
            button.onClick.AddListener(() => process.play());
            setText(process.getName());
            this.process = process;
        }
    }

    public void clearProcess() {
        button.onClick.RemoveAllListeners();
        clearText();
        this.process = null;
    }

    void setHotkeyText(string newKey) {
        hotkeyText.text = newKey;
    }

    void OnMouseExit() {
        activated = false;
        tooltip.clear();
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
    void checkInputs() {

        //1
        if (Input.GetKeyDown(hotkey) && Input.inputString.Length == 1) {
            activated = true;
            tooltipActivated = true; //Also display tooltip on button hold
        }

        if (tooltipActivated) {
            tooltip.set(process.getDescription());
            tooltipActivated = false;
        }

        if (activated) {
            if (process != null) {
                tooltip.set(process.getDescription());
            }

            //2
            if (Input.anyKeyDown && !Input.GetKeyDown(hotkey)) {
                activated = false;
                tooltip.clear();
            }

            //3
            if (activated && (Input.GetKeyUp(hotkey))) {
                button.onClick.Invoke();
                activated = false;
                tooltip.clear();
            }
        }
    }

    void determineButtonAppearance() {
        if (text.text.Length == 0) {
            showDisabledAppearance();
        } else if (activated && button.interactable) {
            showActiveAppearance();
        } else {
            showInactiveAppearance();
        }
    }

    void showActiveAppearance() {
        button.image.color = ACTIVE_COLOR;
        text.color = INACTIVE_COLOR;
        hotkeyText.color = INACTIVE_COLOR;
    }

    void showInactiveAppearance() {
        button.image.color = INACTIVE_COLOR;
        text.color = ACTIVE_COLOR;
        hotkeyText.color = ACTIVE_COLOR;
    }

    void showDisabledAppearance() {
        button.image.color = Color.clear;
        hotkeyText.color = Color.clear;
    }

    bool isActive() {
        return process != null && text.text != null && text.text.Length > 0;
    }
}
