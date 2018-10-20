namespace Language.Common.Utils {
    public class KeyValueCollection<K, V> : IAddKeyValueService<K, V>, ITryGetValueByKeyService<K, V>, ICountService {
        Collection<KeyValueItem> items = new Collection<KeyValueItem>();

        class KeyValueItem {
            readonly K key;
            readonly V value;

            public KeyValueItem(K key, V value) {
                this.key = key;
                this.value = value;
            }
        }

        public int Count { get { return this.items.Count; } }

        public bool Add(K key, V value) {
            return this.items.Add(new KeyValueItem(key, value));
        }

        public bool TryGetValue(K key, out V value) {
            IEnumerator<KeyValueItem> enumerator = this.items.GetEnumerator();
            value = default(V);
            return false;
        }
    }
}
