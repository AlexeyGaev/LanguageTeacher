using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Reflection.Utils.PropertyTree {
    public class PropertyObjectChildren : IEnumerable<PropertyItem> {
        static PropertyObjectChildren valueCycle = new PropertyObjectChildren(Enumerable.Empty<PropertyItem>(), CycleMode.Value);
        static PropertyObjectChildren referenceCycle = new PropertyObjectChildren(Enumerable.Empty<PropertyItem>(), CycleMode.Reference);
        public static PropertyObjectChildren ValueCycle { get { return valueCycle; } }
        public static PropertyObjectChildren ReferenceCycle { get { return referenceCycle; } }

        readonly IEnumerable<PropertyItem> items;
        CycleMode cycleMode;

        PropertyObjectChildren(IEnumerable<PropertyItem> items, CycleMode cycleMode) {
            this.items = items; 
            this.cycleMode = cycleMode;
        }

        public PropertyObjectChildren(IEnumerable<PropertyItem> items) 
            : this(items, CycleMode.None) {
        }
        
        public CycleMode CycleMode { get { return this.cycleMode; } }

        public IEnumerator<PropertyItem> GetEnumerator() {
            return this.items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
