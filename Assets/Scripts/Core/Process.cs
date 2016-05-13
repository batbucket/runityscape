using System;

public class Process {

    public virtual string Name { get; private set; }
    public virtual string Description { get; private set; }
    public virtual Action Action { get; set; }
    public virtual Func<bool> Condition { get; set; }

    public Process(string name = "",
                   string description = "",
                   Action action = null,
                   Func<bool> condition = null) {

        this.Name = name;
        this.Description = description;
        this.Action = action ?? (() => { });
        this.Condition = condition ?? (() => { return true; });
    }

    public void Play() {
        Action.Invoke();
    }
}
