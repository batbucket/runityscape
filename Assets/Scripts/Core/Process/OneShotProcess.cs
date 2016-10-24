using UnityEngine;
using System.Collections;
using System;

public class OneShotProcess : Process {

    private bool wasCalled;

    public OneShotProcess(string name = "", string description = "", Action action = null, Func<bool> condition = null) : base(name, description, action, condition) { }

    public override void Invoke() {
        if (isInvokable()) {
            base.Invoke();
            wasCalled = true;
        }
    }

    protected override bool isInvokable() {
        return false;
        //return base.isInvokable() && !wasCalled;
    }
}