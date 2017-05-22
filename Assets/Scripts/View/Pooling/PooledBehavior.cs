using Scripts.Presenter;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.View.ObjectPool {

    /// <summary>
    /// Represents a MonoBehavior that can be pooled.
    /// Pooled objects are Reset() and returned to the object pool when "destroyed."
    /// So we can retrieve them from there instead of doing costly instantiations.
    /// </summary>
    public abstract class PooledBehaviour : MonoBehaviour {

        /// <summary>
        /// Reset the state of this MonoBehavior.
        /// </summary>
        abstract public void Reset();
    }
}