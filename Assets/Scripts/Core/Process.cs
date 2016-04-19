using System;

public class Process {

    public virtual string Name { get; private set; }
    public virtual string Description { get; private set; }
    public virtual Action Action { get; set; }

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
