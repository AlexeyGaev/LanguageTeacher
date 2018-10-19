namespace Utils.Collections {
    public interface IEnumerator<T> {
        T Current { get; }
        bool MoveNext();
        void Reset();
    }
}
