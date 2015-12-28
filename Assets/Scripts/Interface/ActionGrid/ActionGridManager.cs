using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class ActionGridManager : MonoBehaviour {

    public static readonly KeyCode[] HOTKEYS = new KeyCode[]
        {KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R,
         KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.F,
         KeyCode.Z, KeyCode.X, KeyCode.C, KeyCode.V};

    HotkeyButton[] actionButtons;

    public const int ROWS = 3;
    public const int COLS = 4;

    // Use this for initialization
    void Start() {
        actionButtons = new HotkeyButton[HOTKEYS.Length];
        for (int i = 0; i < actionButtons.Length; i++) {
            GameObject actionButton = (GameObject)Instantiate(Resources.Load("ActionButton"));
            Util.parent(actionButton, GameObject.Find("ButtonHolder"));
            actionButton.GetComponent<HotkeyButton>().setHotkey(HOTKEYS[i]);
            actionButtons[i] = actionButton.GetComponent<HotkeyButton>();
        }
    }

    public void setButtonAttributes(IProcess process, string desc, int r, int c) {
        if (!coordinatesInBounds(r, c)) {
            throw new UnityException(string.Format("R: 0<{0}<{0}, C: 0<{0}<{0}. At least one of these are not in bounds.", r, ROWS, c, COLS));
        }
        actionButtons[calculateOffset(r, c)].setProcess(process);
        actionButtons[calculateOffset(r, c)].setDescription(desc);
    }

    public void clearButtonAttributes(int r, int c) {
        if (!coordinatesInBounds(r, c)) {
            throw new UnityException(string.Format("R: 0<{0}<{0}, C: 0<{0}<{0}. At least one of these are not in bounds.", r, ROWS, c, COLS));
        }
        actionButtons[calculateOffset(r, c)].clearProcess();
        actionButtons[calculateOffset(r, c)].clearDescription();
    }

    public void clearAllButtonAttributes() {
        for (int i = 0; i < actionButtons.Length; i++) {
            actionButtons[i].clearProcess();
            actionButtons[i].clearDescription();
        }
    }

    bool coordinatesInBounds(int r, int c) {
        return 0 < r && r < ROWS && 0 < c && c < COLS;
    }

    int calculateOffset(int r, int c) {
        return r * COLS + c;
    }

    // Update is called once per frame
    void Update() {

    }
}
