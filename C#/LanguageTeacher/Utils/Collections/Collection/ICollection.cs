namespace Utils.Collections.Collection {
    public interface IEnumerable<T> {
        IEnumerator<T> GetEnumerator();
    }

    public interface IAdd<T> {
        bool Add(T item);
    }

    public interface ICount {
        int Count { get; }
    }

    public interface IContains<T> {
        bool Contains(T item);
    }
}
