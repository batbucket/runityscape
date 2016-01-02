using System;

public class Process : IProcess {
    string description;
	Action playAction;
	Action undoAction;

    public void setDescription(string description) {
        this.description = description;
    }

    public string getDescription() {
        return description;
    }

	public void setPlay(Action action) {
		playAction = action;
	}

	public void setUndo(Action action) {
		undoAction = action;
	}

	public void play() {
        if (playAction != null) {
            playAction.Invoke();
        }
	}

	public void undo() {
        if (playAction != null) {
            undoAction.Invoke();
        }
	}
}
