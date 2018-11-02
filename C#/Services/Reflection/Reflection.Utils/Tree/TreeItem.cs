using System.Collections.Generic;

namespace Reflection.Utils.Tree {
    public class TreeItem<T> {
        readonly T value;
        readonly List<TreeItem<T>> children;
        readonly List<TreeItem<T>> parents;

        public TreeItem(T value, List<TreeItem<T>> parents) {
            this.children = new List<TreeItem<T>>();
            this.parents = parents;
            this.value = value;
        }

        public T Value { get { return this.value; } }
        public List<TreeItem<T>> Children { get { return this.children; } }
    }
}
