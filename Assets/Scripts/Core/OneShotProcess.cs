using UnityEngine;
using System.Collections;
using System;

public class OneShotProcess : Process {

    bool wasCalled;

    public override Func<bool> Condition {
        get {
            return () => !wasCalled;
        }
    }

    public OneShotProcess(string name = "", string description = "", Action action = null) : base(name, description, action) {
        this.wasCalled = false;
    }

    public override void Play() {
        if (!wasCalled) {
            this.wasCalled = true;
            base.Play();
        }
    }
}
