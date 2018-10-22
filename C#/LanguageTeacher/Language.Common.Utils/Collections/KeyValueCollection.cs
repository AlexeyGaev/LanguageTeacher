namespace Language.Common.Utils {
    public class KeyValueCollection<K, V> : IAddKeyValueService<K, V>, ITryGetValueByKeyService<K, V>, IKeysEnumeratorService<K>, ICountService {
        Collection<KeyValueItem> items = new Collection<KeyValueItem>();
        UniqueCollection<K> keys = new UniqueCollection<K>();

        class KeyValueItem {
            readonly K key;
            readonly V value;

            public KeyValueItem(K key, V value) {
                this.key = key;
                this.value = value;
            }

            public K Key { get { return this.key; } }
            public V Value { get { return this.value; } }
        }

        public int Count { get { return this.items.Count; } }
        public IEnumerator<K> Keys { get { return this.keys.GetEnumerator(); } }

        public bool Add(K key, V value) {
            return
                this.keys.AddUnique(key) &&
                this.items.Add(new KeyValueItem(key, value));
        }

        public bool TryGetValue(K key, out V value) {
            IEnumerator<KeyValueItem> enumerator = this.items.GetEnumerator();
            while (enumerator.MoveNext()) {
                KeyValueItem item = enumerator.Current;
                if (item.Key.Equals(key)) {
                    value = item.Value;
                    return true;
                }
            }
            value = default(V);
            return false;
        }
    }
}
