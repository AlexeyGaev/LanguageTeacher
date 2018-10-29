using System.Collections.Generic;

namespace Translation.Http.Tree {
    public class TreeItem<T> {
        readonly T value;
        readonly List<TreeItem<T>> children = new List<TreeItem<T>>();

        public TreeItem(T value) {
            this.value = value;
        }

        public T Value { get { return this.value; } }
        public List<TreeItem<T>> Children { get { return this.children; } }
    }
}
