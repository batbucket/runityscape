using Scripts.Model.Interfaces;
using System.Collections.Generic;

/// <summary>
/// Check for equality based on a unique id number.
/// </summary>
/// <typeparam name="T"></typeparam>
public class IdNumberEqualityComparer<T> : IEqualityComparer<T> where T : IIdNumberable {
    /// <summary>
    /// Returns a hash code for this instance.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>
    /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
    /// </returns>
    public int GetHashCode(T value) {
        return value.Id;
    }

    /// <summary>
    /// Checks if left is equal to the right.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns></returns>
    public bool Equals(T left, T right) {
        return left.Id == right.Id;
    }
}