namespace Utils.Collections.Services {
    public interface ITryGetValueByKeyService<K, V> {
        bool TryGetValue(K key, out V value);
    }
}
