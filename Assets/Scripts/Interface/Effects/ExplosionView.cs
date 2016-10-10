using UnityEngine;
using System.Collections;
using System;

public class ExplosionView : PooledBehaviour {
    [SerializeField]
    private ParticleSystem ps;

    public bool IsDone {
        get {
            return ps.time >= ps.startLifetime;
        }
    }

    public void Play() {
        ps.Play();
    }

    public override void Reset() {
        ps.time = 0;
    }
}
