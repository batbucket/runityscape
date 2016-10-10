using System;

public class Process {

    public virtual string Name { get; private set; }
    public virtual string Description { get; private set; }
    public virtual Action Action { get; set; }
    public virtual Func<bool> Condition { get { return _condition; } }

    private Func<bool> _condition;


    public Process(string name,
                   string description = "",
                   Action action = null,
                   Func<bool> condition = null) {
        this.Name = name;
        this.Description = description;
        this.Action = action ?? (() => { });
        this._condition = condition ?? (() => { return true; });
    }

    public Process() {
        this.Name = "";
        this.Description = "";
        this.Action = null;
        this._condition = (() => { return false; });
    }

    public virtual void Play() {
        if (Condition.Invoke()) {
            Action.Invoke();
        }
    }
}
