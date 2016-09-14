using UnityEngine;
using System.Collections;
using System;

public abstract class CharacterEffect : Effect {
    public PortraitView Target {
        get {
            return target;
        }
    }
    protected PortraitView target;

    public CharacterEffect(PortraitView target) : base() {
        this.target = target;
    }

    public abstract void CancelEffect();
}
