using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Reflection.Utils.PropertyTree {
    public class PropertyObjectChildren : IEnumerable<PropertyItem> {
        static PropertyObjectChildren cycle = new PropertyObjectChildren(Enumerable.Empty<PropertyItem>(), true);
        public static PropertyObjectChildren Cycle { get { return cycle; } }
        
        readonly IEnumerable<PropertyItem> items;
        bool hasCycle;

        PropertyObjectChildren(IEnumerable<PropertyItem> items, bool hasCycle) {
            this.items = items; 
            this.hasCycle = hasCycle;
        }

        public PropertyObjectChildren(IEnumerable<PropertyItem> items) 
            : this(items, false) {
            this.items = items;
            this.hasCycle = false;
        }
                
        public bool HasCycle { get { return this.hasCycle; } }

        public IEnumerator<PropertyItem> GetEnumerator() {
            return this.items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
