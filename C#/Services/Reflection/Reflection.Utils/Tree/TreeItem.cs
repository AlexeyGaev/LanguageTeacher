using System.Collections.Generic;

namespace Reflection.Utils.Tree {
    public class TreeItem<T> {
        public IEnumerable<TreeItem<T>> Parents { get; set; }
        public T Value { get; set; }
        public IEnumerable<TreeItem<T>> Children { get; set; }
        public bool HasChildrenCycle { get; set; }

        public override string ToString() {
            return Value.ToString();
        }
    }
}
