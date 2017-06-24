using Scripts.Model.Interfaces;
using System.Collections.Generic;

public class IdNumberEqualityComparer<T> : IEqualityComparer<T> where T : IIdNumberable {
    public int GetHashCode(T value) {
        return value.Id;
    }

    public bool Equals(T left, T right) {
        return left.Id == right.Id;
    }
}