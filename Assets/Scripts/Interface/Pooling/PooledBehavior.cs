using UnityEngine;
using System.Collections;

/// <summary>
/// Represents a MonoBehavior that can be pooled.
/// Pooled objects are Reset() and returned to the pool when "destroyed."
/// </summary>
/// <typeparam name="T">Class that implements this interface.</typeparam>
public abstract class PooledBehaviour : MonoBehaviour {

    /// <summary>
    /// Reset the state of this MonoBehavior.
    /// </summary>
    abstract public void Reset();
}
