using System;

public class Process : IProcess {
    string name;
    string description;
    System.Action playAction;

    public Process(string name, string description, System.Action playAction) {
        this.name = name;
        this.description = description;
        this.playAction = playAction;
    }

    public void setDescription(string description) {
        this.description = description;
    }

    public string getDescription() {
        return description;
    }

    public void setName(string name) {
        this.name = name;
    }

    public string getName() {
        return name;
    }

	public void setPlay(System.Action action) {
		playAction = action;
	}

	public void play() {
        if (playAction != null) {
            playAction.Invoke();
        }
	}
}
