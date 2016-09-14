using UnityEngine;
using System.Collections;
using System;

public class OneShotProcess : Process {

    bool wasCalled;

    public override Func<bool> Condition {
        get {
            return () => base.Condition.Invoke() && !wasCalled;
        }
    }

    public OneShotProcess(string name = "", string description = "", Action action = null, Func<bool> condition = null) : base(name, description, action, condition) {
        this.wasCalled = false;
    }

    public OneShotProcess(Action action) : base(null, null, action) {
        this.wasCalled = false;
    }

    public override void Play() {
        if (!wasCalled) {
            this.wasCalled = true;
            base.Play();
        }
    }
}
