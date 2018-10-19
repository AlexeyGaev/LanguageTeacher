namespace Utils.Collections {
    public class Enumerator<T> : IEnumerator<T> {
        int count;
        int currentIndex;
        T current;
        T[] items;
        
        public Enumerator(T[] items, int count) {
            this.count = count;
            this.items = items;
            Reset();
        }

        public T Current { get { return this.current; } }

        public bool MoveNext() {
            if (currentIndex >= this.count - 1) {
                this.current = default(T);
                return false;
            }
            currentIndex++;
            this.current = items[currentIndex];
            return true;
        }

        public void Reset() {
            this.current = default(T);
            this.currentIndex = -1;
        }
    }
}
