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
public class ActionGridView : MonoBehaviour {
    public static ActionGridView Instance { get; private set; }

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
        Instance = this;
        this.actionButtons = new HotkeyButton[HOTKEYS.Length];
        for (int i = 0; i < actionButtons.Length; i++) {
            GameObject actionButton = (GameObject)Instantiate(Resources.Load("ActionButton"));
            Util.Parent(actionButton, GameObject.Find("ButtonHolder"));
            actionButton.GetComponent<HotkeyButton>().Hotkey = HOTKEYS[i];
            actionButtons[i] = actionButton.GetComponent<HotkeyButton>();
        }
    }

    /**
     * Set what a Button does
     * and its description (text on the button)
     */
    public void SetButtonAttribute(IProcess process, int r, int c) {
        Util.Assert(CoordinatesInBounds(r, c));
        actionButtons[CalculateOffset(r, c)].SetProcess(process);
    }

    public void SetButtonAttribute(IProcess process, int index) {
        Util.Assert(0 <= index && index < ROWS * COLS);
        actionButtons[index].SetProcess(process);
    }

    public void SetButtonAttributes(IList<Process> list) {
        Util.Assert(list.Count <= actionButtons.Length);
        for (int i = 0; i < list.Count; i++) {
            actionButtons[i].SetProcess(list[i]);
        }
    }

    public void SetButtonAttributes(params Process[] processes) {
        Util.Assert(processes.Length <= actionButtons.Length);
        for (int i = 0; i < processes.Length; i++) {
            actionButtons[i].SetProcess(processes[i]);
        }
    }

    public void Clear(int r, int c) {
        if (!CoordinatesInBounds(r, c)) {
            throw new UnityException(string.Format("R: 0<{0}<{0}, C: 0<{0}<{0}. At least one of these are not in bounds.", r, ROWS, c, COLS));
        }
        actionButtons[CalculateOffset(r, c)].ClearProcess();
        actionButtons[CalculateOffset(r, c)].ClearText();
    }

    public void ClearAll() {
        for (int i = 0; i < actionButtons.Length; i++) {
            actionButtons[i].ClearProcess();
            actionButtons[i].ClearText();
        }
    }

    bool CoordinatesInBounds(int r, int c) {
        return 0 <= r && r < ROWS && 0 <= c && c < COLS;
    }

    /**
     * This is a fancy way to pretend a
     * 1d array is actually 2d
     */
    int CalculateOffset(int r, int c) {
        return r * COLS + c;
    }
}
