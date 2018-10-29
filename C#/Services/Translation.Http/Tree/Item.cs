using System.Collections.Generic;

namespace Translation.Http.Tree {
    public class Item<T> {
        readonly T value;
        readonly List<Item<T>> children = new List<Item<T>>();

        public Item(T value) {
            this.value = value;
        }

        public T Value { get { return this.value; } }
        public List<Item<T>> Children { get { return this.children; } }
    }
}
