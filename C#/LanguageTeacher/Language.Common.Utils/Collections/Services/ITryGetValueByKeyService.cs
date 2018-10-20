namespace Language.Common.Utils {
    public interface ITryGetValueByKeyService<K, V> {
        bool TryGetValue(K key, out V value);
    }
}
