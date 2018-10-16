namespace Utils.Collection {
    public class Collection<T> : ICollection<T> {
        const int DefaultCapacity = 2;

        int startCapacity;
        int currentCapacity;
        int aspect;
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
        
        public void Add(T item) {
            if (this.currentCapacity == this.count) 
                IncreaseCapacity();
            this.items[this.count] = item;
            this.count++;
        }

        public void Clear() {
            Init();
        }

        void Init() {
            this.currentCapacity = this.startCapacity;
            this.aspect = 1;
            this.items = CreateItems();
            this.count = 0;
        }

        void IncreaseCapacity() {
            int oldCapacity = this.currentCapacity;
            this.aspect *= 2;
            this.currentCapacity *= this.aspect;
            T[] newItems = CreateItems();
            for (int i = 0; i < oldCapacity; i++)
                newItems[i] = this.items[i];
            this.items = newItems;
        }

        T[] CreateItems() {
            return new T[this.currentCapacity];
        }
    }
}
