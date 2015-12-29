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
    Text description; //The text on the button
    Text hotkeyText;

    bool activated;

    Color INACTIVE_COLOR = Color.black;
    Color ACTIVE_COLOR = Color.white;

    // Use this for initialization
    void Awake() {
        //hotkey = KeyCode.None;
        button = gameObject.GetComponent<Button>();
        input = gameObject.GetComponent<UIInput>();
        description = gameObject.GetComponentsInChildren<Text>()[0];
        hotkeyText = gameObject.GetComponentsInChildren<Text>()[1];
        activated = false;

        description.text = "";
        hotkeyText.text = "";

        //Mouse inputs can also activate action buttons under the same exact rules as if we were using a keyboard
        input.AddOnMouseDownListener(new Action<PointerEventData>(p => InputSimulator.SimulateKeyDown(Util.keyCodeToVirtualKeyCode(hotkey))));
        input.AddOnMouseUpListener(new Action<PointerEventData>(p => InputSimulator.SimulateKeyUp(Util.keyCodeToVirtualKeyCode(hotkey))));
    }

    // Update is called once per frame
    void Update() {
        setHotkeyText(button.interactable ? hotkey.ToString() : "");
        determineButtonAppearance();
        if (button.interactable) {
            checkInputs();
        }
    }

    public void setHotkey(KeyCode keycode) {
        hotkey = keycode;
    }

    public void setDescription(string desc) {
        description.text = desc;
    }

    public void clearDescription() {
        description.text = "";
    }

    public void setEnabled(bool enabled) {
        button.interactable = enabled;
    }

    public void setProcess(IProcess process) {
        button.onClick.AddListener(() => process.play());
    }

    public void clearProcess() {
        button.onClick.RemoveAllListeners();
    }

    void setHotkeyText(string newKey) {
        hotkeyText.text = newKey;
    }

    /**
     * HotkeyableButtons have these properties:
     *    1. If its hotkey is input, the Button changes to its pressed-down state
     *    2. Pressing any other button (includes mouse click) while its hotkey is
                being held down will cancel the input (don't go to #3 if true)
     *    3. If the hotkey is released, it counts as a button click
     */
    void checkInputs() {

        //1
        if (Input.GetKeyDown(hotkey) && Input.inputString.Length == 1) {
            activated = true;
        }

        if (activated) {

            //2
            if (Input.anyKeyDown && !Input.GetKeyDown(hotkey)) {
                activated = false;
            }

            //3
            if (activated && (Input.GetKeyUp(hotkey))) {
                button.onClick.Invoke();
                activated = false;
            }
        }
    }

    void determineButtonAppearance() {
        if ((activated && button.interactable)) {
            showActiveAppearance();
        } else {
            showInactiveAppearance();
        }
    }

    void showActiveAppearance() {
        button.image.color = ACTIVE_COLOR;
        description.color = INACTIVE_COLOR;
        hotkeyText.color = INACTIVE_COLOR;
    }

    void showInactiveAppearance() {
        button.image.color = INACTIVE_COLOR;
        description.color = ACTIVE_COLOR;
        hotkeyText.color = ACTIVE_COLOR;
    }
}
