namespace Utils.Collection {
    public interface ICollection<T> {
        int Count { get; }
        void Add(T item);
        void Clear();
        IEnumerator<T> GetEnumerator();
    }
}
