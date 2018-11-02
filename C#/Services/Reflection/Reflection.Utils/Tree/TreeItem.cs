using System.Collections.Generic;

namespace Reflection.Utils.Tree {
    public class TreeItem<T> {
        readonly T value;
        readonly IEnumerable<TreeItem<T>> children;
        readonly IEnumerable<TreeItem<T>> parents;

        public TreeItem(T value, IEnumerable<TreeItem<T>> parents, IEnumerable<TreeItem<T>> children) {
            this.value = value;
            this.children = children;
            this.parents = parents;
        }

        public T Value { get { return this.value; } }
        public IEnumerable<TreeItem<T>> Children { get { return this.children; } }
        public IEnumerable<TreeItem<T>> Parents { get { return this.parents; } }
    }
}
