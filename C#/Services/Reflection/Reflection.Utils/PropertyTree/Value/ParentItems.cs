using System.Collections.Generic;

namespace Reflection.Utils.PropertyTree {
    public class ParentValues {
        readonly List<object> items;

        public ParentValues(IEnumerable<object> items) {
            this.items = new List<object>(items);
        }
        
        public int Count { get { return this.items.Count; } }
        public bool Contains(object item) {
            return this.items.Contains(item);
        }
    }
}
