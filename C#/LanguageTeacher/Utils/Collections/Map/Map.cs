using Utils.Collections.Collection;

namespace Utils.Collections.Map {
    public class Map<K, V> : IMap<K, V> {
        Collection<MapItem> items = new Collection<MapItem>();
        Collection<K> keys = new Collection<K>();
                
        class MapItem {
            readonly K key;
            readonly V value;

            public MapItem(K key, V value) {
                this.key = key;
                this.value = value;
            }
        }
                
        public bool Add(K key, V value) {
            //TODO
            return false;
        }
        public bool TryGetValue(K key, out V value) {
            //TODO
            value = default(V);
            return false;
        }
    }
}
