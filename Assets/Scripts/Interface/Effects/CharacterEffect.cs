using UnityEngine;
using System.Collections;
using System;

public abstract class CharacterEffect : Effect {
    protected PortraitView target;

    public CharacterEffect(PortraitView target) : base() {
        this.target = target;
    }

    public abstract void CancelEffect();
}
