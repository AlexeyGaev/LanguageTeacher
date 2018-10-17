namespace Utils.Collection {
    public interface ICollection<T> {
        void Clear();
        bool Add(T item);
        IEnumerator<T> GetEnumerator();
    }
}
