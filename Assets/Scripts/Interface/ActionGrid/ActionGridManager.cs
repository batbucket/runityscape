using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

/**
 * This class represents the grid of buttons
 * on the bottom of the game screen
 * Accessible by keyboard keys and the mouse
 */
public class ActionGridManager : MonoBehaviour {

    /**
     * Hotkeys that are assigned to the row of buttons
     * on the bottom of the screen
     */
    public static readonly KeyCode[] HOTKEYS = new KeyCode[]
        {KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R,
         KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.F};

    HotkeyButton[] actionButtons;

    public const int ROWS = 3;
    public const int COLS = 4;

    // Use this for initialization
    void Awake() {
        actionButtons = new HotkeyButton[HOTKEYS.Length];
        for (int i = 0; i < actionButtons.Length; i++) {
            GameObject actionButton = (GameObject)Instantiate(Resources.Load("ActionButton"));
            Util.parent(actionButton, GameObject.Find("ButtonHolder"));
            actionButton.GetComponent<HotkeyButton>().setHotkey(HOTKEYS[i]);
            actionButtons[i] = actionButton.GetComponent<HotkeyButton>();
        }
    }

    /**
     * Set what a Button does
     * and its description (text on the button)
     */
    public void setButtonAttribute(IProcess process, int r, int c) {
        if (!coordinatesInBounds(r, c)) {
            throw new UnityException(string.Format("R: 0<{0}<{0}, C: 0<{0}<{0}. At least one of these are not in bounds.", r, ROWS, c, COLS));
        }
        actionButtons[calculateOffset(r, c)].setProcess(process);
    }

    public void setButtonAttribute(IProcess process, int index) {
        if (!(0 <= index && index < ROWS * COLS)) {
            throw new UnityException(string.Format("{0} is not within the bounds of [{1}, {2})", index, 0, ROWS * COLS));
        }
        actionButtons[index].setProcess(process);
    }

    public void setButtonAttributes(List<Process> list) {
        for (int i = 0; i < actionButtons.Length; i++) {
            actionButtons[i].setProcess(list[i]);
        }
    }

    public void clearButtonAttributes(int r, int c) {
        if (!coordinatesInBounds(r, c)) {
            throw new UnityException(string.Format("R: 0<{0}<{0}, C: 0<{0}<{0}. At least one of these are not in bounds.", r, ROWS, c, COLS));
        }
        actionButtons[calculateOffset(r, c)].clearProcess();
        actionButtons[calculateOffset(r, c)].clearText();
    }

    public void clearAllButtonAttributes() {
        for (int i = 0; i < actionButtons.Length; i++) {
            actionButtons[i].clearProcess();
            actionButtons[i].clearText();
        }
    }

    bool coordinatesInBounds(int r, int c) {
        return 0 <= r && r < ROWS && 0 <= c && c < COLS;
    }

    /**
     * This is a fancy way to pretend a
     * 1d array is actually 2d
     */
    int calculateOffset(int r, int c) {
        return r * COLS + c;
    }
}
