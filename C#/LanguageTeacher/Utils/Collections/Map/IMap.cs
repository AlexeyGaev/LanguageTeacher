namespace Utils.Collections.Map {
    public interface IMap<K, V> {
        bool Add(K key, V value);
        bool TryGetValue(K key, out V value);
    }
}
