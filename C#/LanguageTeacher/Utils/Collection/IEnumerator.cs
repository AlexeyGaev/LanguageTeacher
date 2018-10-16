﻿namespace Utils.Collection {
    public interface IEnumerator<T> {
        T Current { get; }
        bool MoveNext();
        void Reset();
    }
}
