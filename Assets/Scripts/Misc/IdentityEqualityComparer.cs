using System.Collections.Generic;
using System.Runtime.CompilerServices;

/// <summary>
/// Check for identity equality, used for various hashsets.
/// </summary>
/// <typeparam name="T">Type to check for identity equality</typeparam>
public class IdentityEqualityComparer<T> : IEqualityComparer<T>
    where T : class {

    /// <summary>
    /// Returns a hash code for this instance.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>
    /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
    /// </returns>
    public int GetHashCode(T value) {
        return RuntimeHelpers.GetHashCode(value);
    }

    /// <summary>
    /// Checks if left and right are equal referentially.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns></returns>
    public bool Equals(T left, T right) {
        return left == right;
    }
}