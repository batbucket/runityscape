using System;

public class Process {

    public virtual string Name { get; private set; }
    public virtual string Description { get; private set; }
    public virtual Action Action { get; set; }

    Func<bool> _condition;
    public virtual Func<bool> Condition { get { return _condition; } }

    public Process(string name = "",
                   string description = "",
                   Action action = null,
                   Func<bool> condition = null) {

        this.Name = name;
        this.Description = description;
        this.Action = action ?? (() => { });
        this._condition = condition ?? (() => { return true; });
    }

    public virtual void Play() {
        Action.Invoke();
    }
}
