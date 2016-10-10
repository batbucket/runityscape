using UnityEngine;
using System.Collections;
using System;

public class OneShotAnimation : PooledBehaviour {
    public override void Reset() {
    }

    private void Kill() {
        ObjectPoolManager.Instance.Return(this);
    }
}
