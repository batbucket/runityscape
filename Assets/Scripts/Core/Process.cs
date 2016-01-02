using System;

public class Process : IProcess {
	Action playAction;
	Action undoAction;

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
