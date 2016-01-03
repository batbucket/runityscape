using System;

public class Process : IProcess {
    string description;
	Action playAction;

    public void setDescription(string description) {
        this.description = description;
    }

    public string getDescription() {
        return description;
    }

	public void setPlay(Action action) {
		playAction = action;
	}

	public void play() {
        if (playAction != null) {
            playAction.Invoke();
        }
	}
}
