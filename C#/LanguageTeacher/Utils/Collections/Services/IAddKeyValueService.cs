namespace Utils.Collections.Services {
    public interface IAddKeyValueService<K, V> {
        bool Add(K key, V value);
    }
}
