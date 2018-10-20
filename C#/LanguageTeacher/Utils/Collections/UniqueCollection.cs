using Utils.Collections.Services;

namespace Utils.Collections {
    public class UniqueCollection<T> : IEnumerableService<T>, IAddUniqueItemService<T> {
        const int DefaultCapacity = 2;
        const int aspect = 2;

        int startCapacity;
        int currentCapacity;
        int count;
        T[] items;

        public UniqueCollection()
            : this(DefaultCapacity) {
        }
        public UniqueCollection(int capacity) {
            this.startCapacity = capacity;
            Init();
        }

        public IEnumerator<T> GetEnumerator() {
            return new Enumerator<T>(this.items, this.count);
        }

        public bool AddUnique(T item) {
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
