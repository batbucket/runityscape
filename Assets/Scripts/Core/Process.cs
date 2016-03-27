using System;

public class Process : IProcess {

    public string Name { get; private set; }
    public string Description { get; private set; }
    public Action Action { get; set; }

    public Process(string name = "",
                   string description = "",
                   Action action = null) {

        this.Name = name;
        this.Description = description;
        this.Action = action ?? (() => { });
    }

    public void Play() {
        Action.Invoke();
    }
}
