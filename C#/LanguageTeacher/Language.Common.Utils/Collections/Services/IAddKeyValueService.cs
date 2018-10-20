namespace Language.Common.Utils {
    public interface IAddKeyValueService<K, V> {
        bool Add(K key, V value);
    }
}
