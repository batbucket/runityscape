using System.Collections;
using System;

public class Page {
    string locationName;
    string chapterName;
    string mainQuestBlurb;

    string text;
    //left characters
    //right characters

    Process[ , ] actions;

    public Page() {
        actions = new Process[ActionGridManager.ROWS, ActionGridManager.COLS];
    }

    public void setLocation(string locationName) {
        this.locationName = locationName;
    }

    public void setChapter(string chapterName) {
        this.chapterName = chapterName;
    }

    public void setMainQuestBlurb(string mainQuestBlurb) {
        this.mainQuestBlurb = mainQuestBlurb;
    }

    public void setText(string text) {
        this.text = text;
    }

    public void setProcess(Process p, int r, int c) {
        if (0 <= r && r < ActionGridManager.ROWS && 0 <= c && c < ActionGridManager.COLS) {
            actions[r, c] = p;
        } else {
            throw new IndexOutOfRangeException("Bad input. Rows: " + r + " Cols: " + c);
        }
    }
}
