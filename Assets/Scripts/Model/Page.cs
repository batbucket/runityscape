using System.Collections;
using System;
using System.Collections.Generic;

public class Page : IPage {

    string text;
    List<Character> leftCharacters;
    List<Character> rightCharacters;
    Process[ , ] actions;

    Process onEnterProcess;
    Process onFirstEnterProcess;
    Process onExitProcess;
    Process onFirstExitProcess;

    bool hasEnteredBefore;
    bool hasExitedBefore;

    PageType pageType;

    public Page() {
        actions = new Process[ActionGridManager.ROWS, ActionGridManager.COLS];
    }

    public void setText(string text) {
        this.text = text;
    }

    public void setPageType(PageType pageType) {
        this.pageType = pageType;
    }

    public PageType getPageType() {
        return pageType;
    }

    public void onEnter() {
        if (!hasEnteredBefore) {
            hasEnteredBefore = true;
            onFirstEnter();
        } else {
            onEnterProcess.play();
        }
    }

    public void onFirstEnter() {
        onFirstEnterProcess.play();
    }

    public void onExit() {
        if (!hasExitedBefore) {
            hasExitedBefore = true;
            onFirstExit();
        } else {
            onExitProcess.play();
        }
    }

    public void onFirstExit() {
        onFirstExitProcess.play();
    }

    public string getText() {
        return text;
    }

    public Process[,] getActions() {
        return actions;
    }

    public void setOnEnter(Process p) {
        onEnterProcess = p;
    }

    public void setOnFirstEnter(Process p) {
        onFirstExitProcess = p;
    }

    public void setOnExit(Process p) {
        onExitProcess = p;
    }

    public void setOnFirstExit(Process p) {
        onFirstExitProcess = p;
    }

    public void setAction(Process action, int r, int c) {
        if (0 <= r && r < ActionGridManager.ROWS && 0 <= c && c < ActionGridManager.COLS) {
            actions[r, c] = action;
        } else {
            throw new IndexOutOfRangeException("Bad input. Rows: " + r + " Cols: " + c);
        }
    }

    public void setActions(List<Process> actions) {
        if (actions.Count > ActionGridManager.ROWS * ActionGridManager.COLS) {
            throw new ArgumentException("Size of params is: " + actions.Count + ", which is greater than " + ActionGridManager.ROWS * ActionGridManager.COLS);
        }
        int row = 0;
        int col = 0;
        foreach (Process process in actions) {
            setAction(process, row, col++);
            if (col % ActionGridManager.COLS == 0) {
                row++;
                col = 0;
            }
        }
    }

    public void setLeft(List<Character> characters) {
        leftCharacters = characters;
    }

    public void setRight(List<Character> characters) {
        rightCharacters = characters;
    }
}
