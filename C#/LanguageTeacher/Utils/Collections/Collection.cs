using Utils.Collections.Services;

namespace Utils.Collections {
    public class Collection<T> : IEnumerableService<T>, IAddItemService<T>, ICountService {
        const int DefaultCapacity = 2;
        const int aspect = 2;

        int startCapacity;
        int currentCapacity;
        int count;
        T[] items;

        public Collection()
            : this(DefaultCapacity) {
        }
        public Collection(int capacity) {
            this.startCapacity = capacity;
            Init();
        }

        public int Count { get { return this.count; } }

        public IEnumerator<T> GetEnumerator() {
            return new Enumerator<T>(this.items, this.count);
        }

        public bool Add(T item) {
            if (this.count > 0 && this.currentCapacity == this.count) {
                if (!IncreaseCapacity())
                    return false;
            }
            this.items[this.count] = item;
            this.count++;
            return true;
        }

        void Init() {
            this.currentCapacity = this.startCapacity;
            this.items = CreateItems();
            this.count = 0;
        }

        bool IncreaseCapacity() {
            int oldCapacity = this.currentCapacity;
            if (int.MaxValue / aspect < this.currentCapacity)
                return false;
            this.currentCapacity *= aspect;
            T[] newItems = CreateItems();
            CopyItems(this.items, newItems, oldCapacity);
            this.items = newItems;
            return true;
        }

        void CopyItems(T[] source, T[] target, int count) {
            for (int i = 0; i < count; i++)
                target[i] = source[i];
        }

        T[] CreateItems() {
            return new T[this.currentCapacity];
        }
    }
}
