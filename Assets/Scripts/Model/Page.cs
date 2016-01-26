using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class Page {

    string text;
    string tooltip;
    protected List<Character> leftCharacters;
    protected List<Character> rightCharacters;
    protected List<Process> actionGrid;

    Action onFirstEnterAction;
    Action onEnterAction;
    Action onFirstExitAction;
    Action onExitAction;

    Action onTick;

    bool hasEnteredBefore;
    bool hasExitedBefore;

    public static int idCount = 0;
    int id;

    public Page(string text = "", string tooltip = "", List<Character> left = null, List<Character> right = null,
        Action onFirstEnter = null, Action onEnter = null, Action onFirstExit = null, Action onExit = null,
        List<Process> actionGrid = null, Action onTick = null) {
        this.text = text;
        this.tooltip = tooltip;
        if (left == null) this.leftCharacters = new List<Character>(); else SetLeft(left);
        if (right == null) this.rightCharacters = new List<Character>(); else SetRight(right);
        this.onFirstEnterAction = (onFirstEnterAction == null) ? () => { } : onFirstEnter;
        this.onEnterAction = (onEnter == null) ? () => { } : onEnter;
        this.onFirstExitAction = (onFirstExit == null) ? () => { } : onFirstExit;
        this.onExitAction = (onExit == null) ? () => { } : onExit;
        this.actionGrid = (actionGrid == null) ? new List<Process>() : actionGrid;
        this.onTick = (onTick == null) ? () => { } : onTick;
        this.id = idCount++;
    }

    public void SetText(string text) {
        this.text = text;
    }

    public void OnEnter() {
        if (!hasEnteredBefore && onFirstEnterAction != null) {
            hasEnteredBefore = true;
            OnFirstEnter();
        } else if (onEnterAction != null) {
            onEnterAction.Invoke();
        }
    }

    public void OnFirstEnter() {
        onFirstEnterAction.Invoke();
    }

    public void OnExit() {
        if (!hasExitedBefore && onFirstExitAction != null) {
            hasExitedBefore = true;
            OnFirstExit();
        } else if (onExitAction != null) {
            onExitAction.Invoke();
        }
    }

    public void OnFirstExit() {
        onFirstExitAction.Invoke();
    }

    public string GetText() {
        return text;
    }

    public List<Process> GetActions() {
        return actionGrid;
    }

    public void SetOnEnter(Action p) {
        onEnterAction = p;
    }

    public void SetOnFirstEnter(Action p) {
        onFirstExitAction = p;
    }

    public void SetOnExit(Action p) {
        onExitAction = p;
    }

    public void SetOnFirstExit(Action p) {
        onFirstExitAction = p;
    }

    public List<Character> GetCharacters(bool isRightSide) {
        return isRightSide ? rightCharacters : leftCharacters;
    }

    public void SetAction(Process action, int index) {
        if (0 <= index && index < ActionGridManager.ROWS * ActionGridManager.COLS) {
            actionGrid[index] = action;
        } else {
            throw new IndexOutOfRangeException("Bad input. Index: " + index);
        }
    }

    public void ClearAction(int index) {
        if (0 <= index && index < ActionGridManager.ROWS * ActionGridManager.COLS) {
            actionGrid[index] = null;
        } else {
            throw new IndexOutOfRangeException("Bad input. Index: " + index);
        }
    }

    public void SetActions(List<Process> actions) {
        if (actions.Count > ActionGridManager.ROWS * ActionGridManager.COLS) {
            throw new ArgumentException("Size of params is: " + actions.Count + ", which is greater than " + ActionGridManager.ROWS * ActionGridManager.COLS);
        }
        this.actionGrid = actions;
    }

    public void SetLeft(List<Character> characters) {
        SetSide(characters, false);
    }

    public void SetRight(List<Character> characters) {
        SetSide(characters, true);
    }

    public void SetSide(List<Character> characters, bool side) {
        RepeatedCharacterCheck(characters);
        SetCharacterSides(characters, side);
        if (!side) {
            leftCharacters = characters;
        } else {
            rightCharacters = characters;
        }
    }

    void SetCharacterSides(List<Character> characters, bool side) {
        foreach (Character c in characters) {
            c.Side = side;
        }
    }

    /**
     * Check if repeated characters exist on a side, if so, append A->Z for each repeated character
     * For example: Steve A, Steve B
     */
    void RepeatedCharacterCheck(List<Character> characters) {
        Dictionary<string, List<Character>> repeatedCharacters = new Dictionary<string, List<Character>>();
        foreach (Character c in characters) {
            if (!repeatedCharacters.ContainsKey(c.Name)) {
                List<Character> characterList = new List<Character>();
                characterList.Add(c);
                repeatedCharacters.Add(c.Name, characterList);
            } else {
                repeatedCharacters[c.Name].Add(c);
            }
        }

        foreach (KeyValuePair<string, List<Character>> cPair in repeatedCharacters) {
            if (cPair.Value.Count > 1) {
                for (int i = 0; i < cPair.Value.Count; i++) {
                    Character c = cPair.Value[i];
                    c.Name = string.Format("{0} {1}", c.Name, Util.IntToLetter(i + 1));
                }
            }
        }
    }

    public int GetId() {
        return idCount;
    }

    public void SetTooltip(string tooltip) {
        this.tooltip = tooltip;
    }

    public string GetTooltip() {
        return tooltip;
    }

    public override bool Equals(object obj) {
        // If parameter is null return false.
        if (obj == null) {
            return false;
        }

        // If parameter cannot be cast to Page return false.
        Page p = obj as Page;
        if ((object)p == null) {
            return false;
        }

        // Return true if the fields match:
        return p.GetId() == this.GetId();
    }

    public override int GetHashCode() {
        return id;
    }

    public void SetOnTick(Action action) {
        onTick = action;
    }

    public virtual void Tick() {
        onTick.Invoke();
    }
}
