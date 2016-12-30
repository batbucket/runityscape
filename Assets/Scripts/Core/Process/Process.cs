using System;
using UnityEngine;

public class Process : IButtonable {

    private Func<string> name;
    private Func<string> description;
    private Func<string> disabledDesc;
    private Action action;
    private Func<bool> playCondition;
    private bool isVisibleOnDisable;

    public string ButtonText {
        get {
            return
                Util.Color(name.Invoke(), IsInvokable ? Color.white : Color.red);
        }
    }

    public string TooltipText {
        get {
            return
                Util.Color(description.Invoke(), IsInvokable ? Color.white : Color.red);
        }
    }

    public bool IsInvokable {
        get {
            return isInvokable();
        }
    }

    public bool IsVisibleOnDisable {
        get {
            return isVisibleOnDisable;
        }
    }

    public Process(string name,
                   string description = "",
                   Action action = null,
                   Func<bool> playCondition = null,
                   bool isVisibleOnDisable = true) {
        this.name = () => name;
        this.description = () => description;
        this.action = action ?? (() => { });
        this.playCondition = playCondition ?? (() => { return true; });
        this.isVisibleOnDisable = isVisibleOnDisable;
        this.disabledDesc = () => "";
    }

    public Process() {
        this.name = () => "";
        this.description = () => "";
        this.action = (() => { });
        this.playCondition = (() => { return false; });
        this.isVisibleOnDisable = false;
        this.disabledDesc = () => "";
    }

    public virtual void Invoke() {
        if (IsInvokable) {
            action.Invoke();
        }
    }

    protected virtual bool isInvokable() {
        return playCondition.Invoke();
    }
}
